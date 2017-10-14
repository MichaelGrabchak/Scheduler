using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;

using Scheduler.Core.Helpers;
using Scheduler.Core.Jobs;

using NLog;

namespace Scheduler.Core.Engine
{
    public abstract class BaseScheduler : ISchedulerEngine
    {
        protected static ILogger Logger = LogManager.GetLogger(Constants.System.DefaultSchedulerLoggerName);
        protected static DateTimeOffset StartTime { get; private set; }
        protected static EngineState State { get; set; }

        protected BaseScheduler()
        {
        }

        protected BaseScheduler(SchedulerSettings settings)
        {
            Init(settings);
        }

        protected event EngineOperationEventHandler EngineStarted;

        protected virtual void OnEngineStarted()
        {
            StartTime = DateTimeOffset.Now;
            State = EngineState.Normal;

            EngineStarted?.Invoke(this, new EngineOperationEventArgs { State = State });
        }

        protected event EngineOperationEventHandler EnginePaused;

        protected virtual void OnEnginePaused()
        {
            State = EngineState.Paused;

            EnginePaused?.Invoke(this, new EngineOperationEventArgs { State = State });
        }

        protected event EngineOperationEventHandler EngineTerminated;

        protected virtual void OnEngineTerminated()
        {
            State = EngineState.Terminated;

            EngineTerminated?.Invoke(this, new EngineOperationEventArgs { State = State });
        }

        protected event SchedulerEventHandler JobsDiscovered;

        protected virtual void OnJobsDiscovered()
        {
            JobsDiscovered?.Invoke(this, null);
        }

        protected event JobOperationEventHandler JobScheduled;

        protected virtual void OnJobScheduled(JobInfo jobInfo)
        {
            JobScheduled?.Invoke(this,
                new JobOperationEventArgs { Job = jobInfo });
        }

        protected event JobOperationEventHandler JobUnscheduled;

        protected virtual void OnJobUnscheduled(JobInfo jobInfo)
        {
            JobUnscheduled?.Invoke(this,
                new JobOperationEventArgs { Job = jobInfo });
        }

        protected event JobOperationEventHandler JobPaused;

        protected virtual void OnJobPaused(JobInfo jobInfo)
        {
            JobPaused?.Invoke(this,
                new JobOperationEventArgs { Job = jobInfo });
        }

        protected event JobOperationEventHandler JobResumed;

        protected virtual void OnJobResumed(JobInfo jobInfo)
        {
            JobResumed?.Invoke(this,
                new JobOperationEventArgs { Job = jobInfo });
        }

        protected event JobOperationEventHandler JobTriggered;

        protected virtual void OnJobTriggered(JobInfo jobInfo)
        {
            JobTriggered?.Invoke(this,
                new JobOperationEventArgs { Job = jobInfo });
        }

        protected event JobOperationEventHandler BeforeJobExecution;

        protected virtual void OnBeforeJobExecution(JobInfo jobInfo)
        {
            BeforeJobExecution?.Invoke(this,
                new JobOperationEventArgs { Job = jobInfo });
        }

        protected event JobOperationEventHandler AfterJobExecution;

        protected virtual void OnAfterJobExecution(JobInfo jobInfo)
        {
            AfterJobExecution?.Invoke(this,
                new JobOperationEventArgs { Job = jobInfo });
        }

        protected event JobOperationEventHandler JobExecutionSucceeded;

        protected virtual void OnJobSucceeded(JobInfo jobInfo)
        {
            JobExecutionSucceeded?.Invoke(this,
                new JobOperationEventArgs { Job = jobInfo });
        }

        protected event JobOperationEventHandler JobExecutionFailed;

        protected virtual void OnJobFailed(JobInfo jobInfo)
        {
            JobExecutionFailed?.Invoke(this,
                new JobOperationEventArgs { Job = jobInfo });
        }

        protected event JobOperationEventHandler JobExecutionSkipped;

        protected virtual void OnJobSkipped(JobInfo jobInfo)
        {
            JobExecutionSkipped?.Invoke(this,
                new JobOperationEventArgs { Job = jobInfo });
        }

        protected string JobsDirectory { get; private set; }

        protected FileSystemWatcher JobsDirectoryWatcher { get; private set; }

        public virtual void Discover()
        {
            // Reload assemblies
            AssemblyLoaderManager.LoadAssemblies(Constants.Scheduler.DefaultJobsPath, cleanLoad: true);

            // discovering new jobs
            var discoveredJobs = AssemblyLoaderManager.GetImplementorsOf<BaseJob>();

            // unschedule old jobs
            var scheduledJobs = GetAllJobs().Where(_ => _.Group != "DEFAULT"); // exclude triggers which are running right now

            foreach (var scheduledJob in scheduledJobs)
            {
                if (!discoveredJobs.Any(_ => _.GetType().Name.Equals(scheduledJob.Name, StringComparison.InvariantCultureIgnoreCase)
                                             && _.GetType().Namespace.Equals(scheduledJob.Group, StringComparison.InvariantCultureIgnoreCase)))
                {
                    UnscheduleJob(scheduledJob.Name, scheduledJob.Group);
                }
            }

            // schedule new jobs
            if (discoveredJobs != null && discoveredJobs.Count > 0)
            {
                ScheduleJobs(discoveredJobs);
                OnJobsDiscovered();
            }
        }

        public abstract void Pause();
        public abstract void Start();
        public abstract void Stop();

        public abstract void ScheduleJob(BaseJob scheduleJob);
        public abstract void UnscheduleJob(BaseJob scheduleJob);
        public abstract void UnscheduleJob(string jobName, string jobGroup);

        public virtual void ScheduleJobs(IEnumerable<BaseJob> jobsToSchedule)
        {
            if (jobsToSchedule != null)
            {
                foreach (var job in jobsToSchedule)
                {
                    try
                    {
                        ScheduleJob(job);
                    }
                    catch (Exception ex)
                    {
                        Logger.Warn(ex, $"Could not schedule the job with name '{job.GetType().Name}' due to an error: ");
                    }
                }
            }
        }

        public abstract void TriggerJob(string jobName, string jobGroup);
        public abstract void PauseJob(string jobName, string jobGroup);
        public abstract void ResumeJob(string jobName, string jobGroup);

        public abstract IEnumerable<JobInfo> GetAllJobs();

        public virtual void Dispose()
        {
            Stop();
        }

        public EngineDetails GetEngineInfo()
        {
            return new EngineDetails
            {
                Name = GetType().FullName,
                State = State.ToString(),
                StartDate = StartTime,
                Version = AssemblyName.GetAssemblyName(Assembly.GetExecutingAssembly().Location).Version.ToString()
            };
        }

        private void Init(SchedulerSettings settings)
        {
            if (settings != null)
            {
                InitBasicSettings(settings);
                InitEvents(settings);
                SetupJobsDirectory(settings);
            }
        }

        private void InitBasicSettings(SchedulerSettings settings)
        {
            if (settings.StartEngineImmediately)
            {
                Start();
            }
        }

        private void InitEvents(SchedulerSettings settings)
        {
            if (settings.EngineStarted != null)
                EngineStarted += settings.EngineStarted;

            if (settings.EnginePaused != null)
                EnginePaused += settings.EnginePaused;

            if (settings.EngineTerminated != null)
                EngineTerminated += settings.EngineTerminated;

            if (settings.JobsDiscovered != null)
                JobsDiscovered += settings.JobsDiscovered;

            if (settings.JobScheduled != null)
                JobScheduled += settings.JobScheduled;

            if (settings.JobUnscheduled != null)
                JobUnscheduled += settings.JobUnscheduled;

            if (settings.JobPaused != null)
                JobPaused += settings.JobPaused;

            if (settings.JobResumed != null)
                JobResumed += settings.JobResumed;

            if (settings.JobTriggered != null)
                JobTriggered += settings.JobTriggered;

            if (settings.BeforeJobExecution != null)
                BeforeJobExecution += settings.BeforeJobExecution;

            if (settings.AfterJobExecution != null)
                AfterJobExecution += settings.AfterJobExecution;

            if (settings.JobExecutionSucceeded != null)
                JobExecutionSucceeded += settings.JobExecutionSucceeded;

            if (settings.JobExecutionFailed != null)
                JobExecutionFailed += settings.JobExecutionFailed;

            if (settings.JobExecutionSkipped != null)
                JobExecutionSkipped += settings.JobExecutionSkipped;
        }

        private void SetupJobsDirectory(SchedulerSettings settings)
        {
            if (!string.IsNullOrEmpty(settings.JobsDirectory))
            {
                JobsDirectory = settings.JobsDirectory;

                if (!Directory.Exists(JobsDirectory))
                {
                    Directory.CreateDirectory(JobsDirectory);
                }

                if (settings.EnableJobsDirectoryTracking)
                {
                    JobsDirectoryWatcher = new FileSystemWatcher(JobsDirectory, "*.dll");

                    JobsDirectoryWatcher.Changed += OnJobsDirectoryChanged;
                    JobsDirectoryWatcher.Created += OnJobsDirectoryChanged;
                    JobsDirectoryWatcher.Deleted += OnJobsDirectoryChanged;

                    JobsDirectoryWatcher.EnableRaisingEvents = true;
                }
            }
        }

        private void OnJobsDirectoryChanged(object source, FileSystemEventArgs e)
        {
            Discover();
        }
    }
}