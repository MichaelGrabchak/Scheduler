using System.Linq;
using System.Collections.Generic;

using CronExpressionDescriptor;

using Scheduler.Domain.Data.Services;
using Scheduler.Engine.Enums;
using Scheduler.Engine.Jobs;
using Scheduler.Engine.Quartz.Extension;
using Scheduler.Engine.Quartz.Listeners;
using Scheduler.Jobs;
using Scheduler.Jobs.Extensions;

using Quartz;
using Quartz.Impl;
using Quartz.Impl.Matchers;

namespace Scheduler.Engine.Quartz
{
    public sealed class QuartzScheduler : BaseScheduler
    {
        private static readonly DependentTriggerListener TriggerListener = new DependentTriggerListener();
        private static readonly DependentJobListener JobListener = new DependentJobListener();

        private static IScheduler _quartzScheduler = GetScheduler(TriggerListener, JobListener);

        public QuartzScheduler(SchedulerSettings settings, JobMetadata metadataManager, IJobDetailService jobDetailService)
            : base(settings, metadataManager, jobDetailService)
        {
            JobListener.ToBeExecuted += (s, e) => OnBeforeJobExecution(e.Job);
            JobListener.Executed += (s, e) => OnJobTriggered(e.Job);
            JobListener.ExecutionVetoed += (s, e) => OnJobSkipped(e.Job);
            JobListener.ExecutionSucceeded += (s, e) => OnJobSucceeded(e.Job);
            JobListener.ExecutionFailed += (s, e) => OnJobFailed(e.Job);
        }

        public override void Pause()
        {
            if (_quartzScheduler?.IsStarted == true)
            {
                _quartzScheduler.PauseAll();

                foreach(var job in GetAllJobs())
                {
                    PauseJob(job.Name, job.Group);
                }

                OnEnginePaused();
            }
        }

        public override void Start()
        {
            if (_quartzScheduler == null || _quartzScheduler?.IsShutdown == true)
            {
                _quartzScheduler = GetScheduler(TriggerListener, JobListener);
            }

            if (_quartzScheduler?.IsStarted == false)
            {
                Discover();

                _quartzScheduler.Start();
            }
            else
            {
                _quartzScheduler?.ResumeAll();

                foreach(var job in GetAllJobs())
                {
                    ResumeJob(job.Name, job.Group);
                }
            }

            OnEngineStarted();
        }

        public override void Stop()
        {
            if (_quartzScheduler?.IsShutdown == false)
            {
                _quartzScheduler.Shutdown(false);
                _quartzScheduler = null;

                OnEngineTerminated();
            }
        }

        public override void ScheduleJob(BaseJob scheduleJob)
        {
            var metadata = Metadata.ExtractData(scheduleJob);

            var jobKey = new JobKey(metadata.Name, metadata.Group);
            if (_quartzScheduler.CheckExists(jobKey).Result)
            {
                // if the job already scheduled, we don't want to re-schedule it
                return;
            }

            var job = JobBuilder.Create<QuartzJob>()
                                .WithIdentity(metadata.Name, metadata.Group)
                                .WithDescription(metadata.Description)
                                .UsingJobData("TypeFullName", metadata.Type.FullName)
                                .Build();

            var trigger = TriggerBuilder.Create()
                                        .WithIdentity($"{metadata.Name}Trigger", metadata.Group)
                                        .StartNow()
                                        .WithCronSchedule(metadata.Schedule, trg => trg
                                            .WithMisfireHandlingInstructionDoNothing())
                                        .ForJob(metadata.Name, metadata.Group)
                                        .Build();

            _quartzScheduler.ScheduleJob(job, trigger);

            OnJobScheduled(JobInfo.Create(metadata.Group, metadata.Name, 
                logger: scheduleJob.GetLogger(),
                scheduleExp: metadata.Schedule,
                schedule: CronExpressionDescriptor.ExpressionDescriptor.GetDescription(metadata.Schedule, new Options() { Locale = "en" }),
                nextFire: trigger.GetNextFireTimeUtc()
            ));

            // TODO: Find better way to schedule paused job
            if(metadata.State == (byte)JobState.Paused)
            {
                PauseJob(metadata.Name, metadata.Group);
            }
        }

        public override void UnscheduleJob(BaseJob scheduleJob)
        {
            var metadata = Metadata.ExtractData(scheduleJob);

            if (metadata != null)
            {
                var details = _quartzScheduler.GetJobDetail(metadata.Name, metadata.Group);

                if (details != null)
                {
                    _quartzScheduler.DeleteJob(details.Key);

                    OnJobUnscheduled(JobInfo.Create(metadata.Group, metadata.Name));
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

                    OnJobUnscheduled(JobInfo.Create(jobGroup, jobName));
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

                    OnJobPaused(JobInfo.Create(jobGroup, jobName,
                        state: JobState.Paused.ToString(),
                        isNextFireTimeSpecified: true
                    ));
                }
            }
        }

        public override void ResumeJob(string jobName, string jobGroup)
        {
            if (!string.IsNullOrEmpty(jobName) && !string.IsNullOrEmpty(jobGroup))
            {
                var details = _quartzScheduler.GetJobDetail(jobName, jobGroup);

                if (details != null)
                {
                    _quartzScheduler.ResumeJob(details.Key);

                    var triggers = _quartzScheduler.GetTriggersOfJob(details.Key);

                    OnJobResumed(JobInfo.Create(jobGroup, jobName,
                        state: JobState.Normal.ToString(),
                        nextFire: triggers.Result.SingleOrDefault()?.GetNextFireTimeUtc()
                    ));
                }
            }
        }

        public override IEnumerable<JobInfo> GetAllJobs()
        {
            var jobs = new List<JobInfo>();

            if (_quartzScheduler != null && !_quartzScheduler.IsShutdown)
            {
                var jobGroups = _quartzScheduler.GetJobGroupNames();

                foreach (string group in jobGroups.Result)
                {
                    var groupMatcher = GroupMatcher<JobKey>.GroupContains(group);
                    var jobKeys = _quartzScheduler.GetJobKeys(groupMatcher);
                    foreach (var jobKey in jobKeys.Result)
                    {
                        var detail = _quartzScheduler.GetJobDetail(jobKey).Result;
                        var triggers = _quartzScheduler.GetTriggersOfJob(jobKey);
                        foreach (var trigger in triggers.Result)
                        {
                            var jobName = jobKey.Name;
                            var jobGroup = trigger.Key.Group;

                            // if group is DEFAULT means that it is temporary trigger group and we don't need to show it in the list of jobs (triggers)
                            if (jobGroup == "DEFAULT")
                            {
                                continue;
                            }

                            jobs.Add(ExtractJobInfo(jobName, jobGroup, detail, trigger));
                        }
                    }
                }
            }

            return jobs;
        }

        #region Helpers

        private JobInfo ExtractJobInfo(string name, string group, IJobDetail jobDetail, ITrigger trigger)
        {
            var jobInfo = JobDetailService.GetJobDetail(name, group);

            var scheduleExpr = string.Empty;
            if (trigger is ICronTrigger cronTrigger)
            {
                scheduleExpr = cronTrigger.CronExpressionString;
            }

            var originJobInfo = JobInfo.Create(
                group, name,
                desc: jobDetail.Description,
                schedule: scheduleExpr,
                state: _quartzScheduler.GetTriggerState(trigger.Key).Result.GetJobState().ToString(),
                actionState: (_quartzScheduler.GetCurrentlyExecutingJobs().Result.ContainsJob(name, group)) 
                    ? JobActionState.Executing.ToString() 
                    : string.Empty,
                nextFire: trigger.GetNextFireTimeUtc(),
                prevFire: trigger.GetPreviousFireTimeUtc()
            );

            return JobInfo.Create(
                group, name,
                desc: jobInfo?.JobDescription ?? originJobInfo.Description,
                schedule: CronExpressionDescriptor.ExpressionDescriptor.GetDescription(
                    jobInfo?.JobSchedule ?? originJobInfo.Schedule, new Options { Locale = "en" }),
                state: originJobInfo.State,
                actionState: originJobInfo.ActionState,
                nextFire: jobInfo?.JobNextRunTime ?? originJobInfo.NextFireTimeUtc,
                prevFire: jobInfo?.JobLastRunTime ?? originJobInfo.PrevFireTimeUtc
            );
        }

        private static IScheduler GetScheduler(ITriggerListener triggerListener, IJobListener jobListener)
        {
            var quartzScheduler = StdSchedulerFactory.GetDefaultScheduler().Result;

            // registering listeners
            quartzScheduler.ListenerManager.AddTriggerListener(triggerListener, GroupMatcher<TriggerKey>.AnyGroup());
            quartzScheduler.ListenerManager.AddJobListener(jobListener, GroupMatcher<JobKey>.AnyGroup());

            return quartzScheduler;
        }

        #endregion
    }
}