using System.Collections.Generic;

using Scheduler.Core.Jobs;
using Scheduler.Core.Engine;
using Scheduler.Engine.Quartz.Extension;
using Scheduler.Engine.Quartz.Listeners;

using Quartz;
using Quartz.Impl;
using Quartz.Impl.Matchers;

namespace Scheduler.Engine.Quartz
{
    public sealed class QuartzScheduler : BaseScheduler
    {
        private static DependentTriggerListener _triggerListener = new DependentTriggerListener();
        private static DependentJobListener _jobListener = new DependentJobListener();

        private static IScheduler _quartzScheduler = GetScheduler(_triggerListener, _jobListener);

        public QuartzScheduler(SchedulerSettings settings)
            : base(settings)
        {
            if (settings != null)
            {
                if (settings.BeforeJobExecution != null)
                    _jobListener.ToBeExecuted += settings.BeforeJobExecution;

                if (settings.JobExecutionSkipped != null)
                    _jobListener.ExecutionVetoed += settings.JobExecutionSkipped;

                if (settings.JobExecutionSucceeded != null)
                    _jobListener.ExecutionSucceeded += settings.JobExecutionSucceeded;

                if (settings.JobExecutionFailed != null)
                    _jobListener.ExecutionFailed += settings.JobExecutionFailed;
            }
        }

        public override void Pause()
        {
            if (_quartzScheduler?.IsStarted == true)
            {
                _quartzScheduler.PauseAll();

                OnEnginePaused();
            }
        }

        public override void Start()
        {
            if (_quartzScheduler == null || _quartzScheduler?.IsShutdown == true)
            {
                _quartzScheduler = GetScheduler(_triggerListener, _jobListener);
            }

            if (_quartzScheduler?.IsStarted == false)
            {
                Discover();

                _quartzScheduler.Start();

                OnEngineStarted();
            }
            else
            {
                _quartzScheduler?.ResumeAll();

                OnEngineStarted();
            }
        }

        public override void Stop()
        {
            if (_quartzScheduler?.IsShutdown == false)
            {
                _quartzScheduler.Shutdown();

                OnEngineTerminated();
            }
        }

        public override void ScheduleJob(BaseJob scheduleJob)
        {
            var jobData = JobData.ExtractData(scheduleJob);

            var jobKey = new JobKey(jobData.Name, jobData.Group);
            if (_quartzScheduler.CheckExists(jobKey))
            {
                // if the job already scheduled, we don't want to re-schedule it
                return;
            }

            var job = JobBuilder.Create<QuartzJob>()
                                .WithIdentity(jobData.Name, jobData.Group)
                                .WithDescription(jobData.Description)
                                .UsingJobData("TypeFullName", jobData.Type.FullName)
                                .Build();

            var trigger = TriggerBuilder.Create()
                                        .WithIdentity($"{jobData.Name}Trigger", jobData.Group)
                                        .StartNow()
                                        .WithCronSchedule(jobData.Schedule, trg => trg
                                            .WithMisfireHandlingInstructionDoNothing())
                                        .ForJob(jobData.Name, jobData.Group)
                                        .Build();

            _quartzScheduler.ScheduleJob(job, trigger);

            OnJobScheduled(new JobInfo {
                Name = jobData.Name,
                Group = jobData.Group,
                Schedule = CronExpressionDescriptor.ExpressionDescriptor.GetDescription(jobData.Schedule),
                State = JobState.Normal.ToString(),
                NextFireTimeUtc = trigger.GetNextFireTimeUtc()
            });
        }

        public override void UnscheduleJob(BaseJob scheduleJob)
        {
            var jobData = JobData.ExtractData(scheduleJob);

            if (jobData != null)
            {
                var details = _quartzScheduler.GetJobDetail(jobData.Name, jobData.Group);

                if (details != null)
                {
                    _quartzScheduler.DeleteJob(details.Key);

                    OnJobUnscheduled(new JobInfo {
                        Name = jobData.Name,
                        Group = jobData.Group
                    });
                }
            }
        }

        public override void UnscheduleJob(string jobName, string jobGroup)
        {
            if (!string.IsNullOrEmpty(jobName) && !string.IsNullOrEmpty(jobGroup))
            {
                var details = _quartzScheduler.GetJobDetail(jobName, jobGroup);

                if (details != null)
                {
                    _quartzScheduler.DeleteJob(details.Key);

                    OnJobUnscheduled(new JobInfo {
                        Name = jobName,
                        Group = jobGroup
                    });
                }
            }
        }

        public override void TriggerJob(string jobName, string jobGroup)
        {
            if (!string.IsNullOrEmpty(jobName) && !string.IsNullOrEmpty(jobGroup))
            {
                var details = _quartzScheduler.GetJobDetail(jobName, jobGroup);

                if (details != null)
                {
                    _quartzScheduler.TriggerJob(details.Key);
                }
            }
        }

        public override void PauseJob(string jobName, string jobGroup)
        {
            if (!string.IsNullOrEmpty(jobName) && !string.IsNullOrEmpty(jobGroup))
            {
                var details = _quartzScheduler.GetJobDetail(jobName, jobGroup);

                if (details != null)
                {
                    _quartzScheduler.PauseJob(details.Key);

                    OnJobPaused(new JobInfo {
                        Name = jobName,
                        Group = jobGroup,
                        State = JobState.Paused.ToString()
                    });

                    return;
                }
            }

            OnJobResumed(new JobInfo {
                Name = jobName,
                Group = jobGroup,
                State = JobState.Normal.ToString()
            });
        }

        public override void ResumeJob(string jobName, string jobGroup)
        {
            if (!string.IsNullOrEmpty(jobName) && !string.IsNullOrEmpty(jobGroup))
            {
                var details = _quartzScheduler.GetJobDetail(jobName, jobGroup);

                if (details != null)
                {
                    _quartzScheduler.ResumeJob(details.Key);

                    OnJobResumed(new JobInfo {
                        Name = jobName,
                        Group = jobGroup,
                        State = JobState.Normal.ToString()
                    });

                    return;
                }
            }

            OnJobPaused(new JobInfo {
                Name = jobName,
                Group = jobGroup,
                State = JobState.Paused.ToString()
            });
        }

        public override IEnumerable<JobInfo> GetAllJobs()
        {
            var jobs = new List<JobInfo>();

            if (_quartzScheduler != null && !_quartzScheduler.IsShutdown)
            {
                var jobGroups = _quartzScheduler.GetJobGroupNames();

                foreach (string group in jobGroups)
                {
                    var groupMatcher = GroupMatcher<JobKey>.GroupContains(group);
                    var jobKeys = _quartzScheduler.GetJobKeys(groupMatcher);
                    foreach (var jobKey in jobKeys)
                    {
                        var detail = _quartzScheduler.GetJobDetail(jobKey);
                        var triggers = _quartzScheduler.GetTriggersOfJob(jobKey);
                        foreach (var trigger in triggers)
                        {
                            var jobName = jobKey.Name;
                            var jobGroup = trigger.Key.Group;

                            // if group is DEFAULT means that it is temporary trigger group and we don't need to show it in the list of jobs (triggers)
                            if (jobGroup == "DEFAULT")
                            {
                                continue;
                            }

                            var description = detail.Description;

                            var scheduleString = string.Empty;
                            if (trigger is ICronTrigger)
                            {
                                scheduleString = CronExpressionDescriptor
                                    .ExpressionDescriptor
                                    .GetDescription(
                                        (trigger as ICronTrigger).CronExpressionString);
                            }

                            var triggerState = (!_quartzScheduler.GetCurrentlyExecutingJobs().ContainsJob(jobName, jobGroup))
                                ? _quartzScheduler.GetTriggerState(trigger.Key).GetJobState().ToString()
                                : JobState.Executing.ToString();

                            jobs.Add(new JobInfo
                            {
                                Group = jobGroup,
                                Name = jobName,
                                Description = detail.Description,
                                Schedule = scheduleString,
                                State = triggerState,
                                NextFireTimeUtc = trigger.GetNextFireTimeUtc(),
                                PrevFireTimeUtc = trigger.GetPreviousFireTimeUtc()
                            });
                        }
                    }
                }
            }

            return jobs;
        }

        private static IScheduler GetScheduler(ITriggerListener triggerListener, IJobListener jobListener)
        {
            var quartzScheduler = StdSchedulerFactory.GetDefaultScheduler();

            // registering listeners
            quartzScheduler.ListenerManager.AddTriggerListener(triggerListener, GroupMatcher<TriggerKey>.AnyGroup());
            quartzScheduler.ListenerManager.AddJobListener(jobListener, GroupMatcher<JobKey>.AnyGroup());

            return quartzScheduler;
        }
    }
}