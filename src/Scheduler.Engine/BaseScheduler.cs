using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;

using Scheduler.Core.Loader;
using Scheduler.Domain.Data.Services;
using Scheduler.Engine.Enums;
using Scheduler.Engine.Jobs;
using Scheduler.Jobs;
using Scheduler.Jobs.Extensions;
using Scheduler.Logging;
using Scheduler.Logging.Loggers;

namespace Scheduler.Engine
{
    public abstract class BaseScheduler : ISchedulerEngine
    {
        public static string FullyQualifiedName { get; private set; }
        public static string Version { get; private set; }
        public static string InstanceId { get; private set; }
        public static string InstanceName { get; private set; }

        protected static ILogger Logger = LogManager.GetLogger();
        protected static DateTimeOffset StartTime { get; private set; }
        protected static EngineState State { get; set; }

        protected readonly JobMetadata Metadata;
        protected readonly IJobDetailService JobDetailService;

        protected BaseScheduler(SchedulerSettings settings, JobMetadata metadataManager, IJobDetailService jobDetailService)
        {
            JobDetailService = jobDetailService;
            Metadata = metadataManager;

            Init(settings);
        }

        protected event EngineOperationEventHandler EngineStarted;

        protected virtual void OnEngineStarted()
        {
            FullyQualifiedName = GetType().FullName;
            Version = AssemblyName.GetAssemblyName(Assembly.GetExecutingAssembly().Location).Version.ToString();

            StartTime = DateTimeOffset.Now;
            State = EngineState.Normal;

            Logger.Info("The scheduler engine has started.");
            Logger.Info($"  Scheduler Type: {FullyQualifiedName}, Version: {Version}");

            EngineStarted?.Invoke(this, new EngineOperationEventArgs { State = State });
        }

        protected event EngineOperationEventHandler EnginePaused;

        protected virtual void OnEnginePaused()
        {
            State = EngineState.Paused;

            Logger.Info("The scheduler engine has put on hold.");

            EnginePaused?.Invoke(this, new EngineOperationEventArgs { State = State });
        }

        protected event EngineOperationEventHandler EngineTerminated;

        protected virtual void OnEngineTerminated()
        {
            FullyQualifiedName = null;
            Version = null;

            StartTime = DateTimeOffset.MinValue;
            State = EngineState.Terminated;

            Logger.Warn("The scheduler engine has shut down.");

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
            Logger.Info($"The job '{jobInfo.Group}.{jobInfo.Name}' has been scheduled. Schedule: {jobInfo.Schedule}");

            JobScheduled?.Invoke(this,
                new JobOperationEventArgs { Job = jobInfo });

            var jobDetail = jobInfo.ToJobDetail();
            jobDetail.StatusId = (byte)JobState.Normal;

            JobDetailService.UpdateJobDetail(jobDetail);
        }

        protected event JobOperationEventHandler JobUnscheduled;

        protected virtual void OnJobUnscheduled(JobInfo jobInfo)
        {
            Logger.Info($"The job '{jobInfo.Group}.{jobInfo.Name}' has been unscheduled");

            JobUnscheduled?.Invoke(this,
                new JobOperationEventArgs { Job = jobInfo });

            var jobDetail = jobInfo.ToJobDetail();
            jobDetail.StatusId = (byte)JobState.Paused;
            jobDetail.JobNextRunTime = null;
            jobDetail.JobNextRunTimeSpecified = true;

            JobDetailService.UpdateJobDetail(jobDetail, updateChangedOnly: true);
        }

        protected event JobOperationEventHandler JobPaused;

        protected virtual void OnJobPaused(JobInfo jobInfo)
        {
            Logger.Info($"The job '{jobInfo.Group}.{jobInfo.Name}' has been put on hold");

            JobPaused?.Invoke(this,
                new JobOperationEventArgs { Job = jobInfo });

            var jobDetail = jobInfo.ToJobDetail();
            jobDetail.StatusId = (byte)JobState.Paused;
            jobDetail.JobNextRunTime = null;
            jobDetail.JobNextRunTimeSpecified = true;

            JobDetailService.UpdateJobDetail(jobDetail, updateChangedOnly: true);
        }

        protected event JobOperationEventHandler JobResumed;

        protected virtual void OnJobResumed(JobInfo jobInfo)
        {
            Logger.Info($"The job '{jobInfo.Group}.{jobInfo.Name}' has been resumed. Next fire time: {jobInfo.NextFireTimeUtc}");

            JobResumed?.Invoke(this,
                new JobOperationEventArgs { Job = jobInfo });

            var jobDetail = jobInfo.ToJobDetail();
            jobDetail.StatusId = (byte)JobState.Normal;

            JobDetailService.UpdateJobDetail(jobDetail, updateChangedOnly: true);
        }

        protected event JobOperationEventHandler JobTriggered;

        protected virtual void OnJobTriggered(JobInfo jobInfo)
        {
            Logger.Info($"The job '{jobInfo.Group}.{jobInfo.Name}' has been triggered");

            JobTriggered?.Invoke(this,
                new JobOperationEventArgs { Job = jobInfo });

            var jobDetail = jobInfo.ToJobDetail();
            JobDetailService.UpdateJobDetail(jobDetail, updateChangedOnly: true);
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
            Logger.Info($"The job '{jobInfo.Group}.{jobInfo.Name}' has finished with result: SUCCESS");

            JobExecutionSucceeded?.Invoke(this,
                new JobOperationEventArgs { Job = jobInfo });
        }

        protected event JobOperationEventHandler JobExecutionFailed;

        protected virtual void OnJobFailed(JobInfo jobInfo)
        {
            Logger.Info($"The job '{jobInfo.Group}.{jobInfo.Name}' has finished with result: FAILURE");

            JobExecutionFailed?.Invoke(this,
                new JobOperationEventArgs { Job = jobInfo });
        }

        protected event JobOperationEventHandler JobExecutionSkipped;

        protected virtual void OnJobSkipped(JobInfo jobInfo)
        {
            Logger.Info($"The job '{jobInfo.Group}.{jobInfo.Name}' has been SKIPPED");

            JobExecutionSkipped?.Invoke(this,
                new JobOperationEventArgs { Job = jobInfo });

            var jobDetail = jobInfo.ToJobDetail();
            JobDetailService.UpdateJobDetail(jobDetail, updateChangedOnly: true);
        }

        protected string JobsDirectory { get; private set; }

        protected FileSystemWatcher JobsDirectoryWatcher { get; private set; }

        public virtual void Discover()
        {
            Logger.Info("Loading the assemblies...");

            // Reload assemblies
            AssemblyLoaderManager.LoadAssemblies(JobsDirectory, cleanLoad: true);

            Logger.Info("Discovering the jobs...");

            // discovering new jobs
            var discoveredJobs = AssemblyLoaderManager.GetImplementorsOf<BaseJob>();

            Logger.Info($"Found {discoveredJobs.Count} job(s)");

            if(discoveredJobs.Count > 0)
            {
                Logger.Debug($"Discovered jobs:{Environment.NewLine}{string.Join(Environment.NewLine, discoveredJobs.Select(job => $"- Group: {job.GetGroup()}, Name: {job.GetName()}, Schedule: {job.Schedule}"))}");
            }

            // Currently scheduled jobs
            var scheduledJobs = GetAllJobs().Where(job => job.Group != "DEFAULT"); // exclude triggers which are running right now

            if(scheduledJobs.Count() > 0)
            {
                Logger.Info("Revisiting scheduled jobs...");
                Logger.Debug($"Currently scheduled jobs:{Environment.NewLine}{string.Join(Environment.NewLine, scheduledJobs)}");
            }

            // Un-scheduling old jobs
            foreach (var scheduledJob in scheduledJobs)
            {
                if (!discoveredJobs.Any(job => job.GetType().Name.Equals(scheduledJob.Name, StringComparison.InvariantCultureIgnoreCase)
                                             && job.GetType().Namespace.Equals(scheduledJob.Group, StringComparison.InvariantCultureIgnoreCase)))
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
                        Logger.Warn(ex, $"Could not schedule the job with name '{job.GetName()}' due to an error: ");
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
                Name = FullyQualifiedName,
                State = State.ToString(),
                StartDate = StartTime,
                Version = Version,
                InstanceId = InstanceId,
                InstanceName = InstanceName
            };
        }

        private void Init(SchedulerSettings settings)
        {
            if (settings != null)
            {
                WireEventHandlers(settings);
                SetupJobsDirectory(settings);

                InitBasicSettings(settings);
            }
        }

        private void InitBasicSettings(SchedulerSettings settings)
        {
            if (settings.StartEngineImmediately)
            {
                Start();
            }

            InstanceId = settings.InstanceId;
            InstanceName = settings.InstanceName;
        }

        private void WireEventHandlers(SchedulerSettings settings)
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

                    JobsDirectoryWatcher.IncludeSubdirectories = true;
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