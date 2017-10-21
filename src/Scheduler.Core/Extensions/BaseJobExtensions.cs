using System;

using Scheduler.Core.Jobs;
using Scheduler.Core.Attributes;

namespace Scheduler.Core.Extensions  
{
    public static class BaseJobExtensions
    {
        public static string GetName(this BaseJob job)
        {
            return GetMetadataAttribute(job.GetType())?.Name ?? job.GetType().Name;
        }

        public static string GetGroup(this BaseJob job)
        {
            return GetMetadataAttribute(job.GetType())?.Group ?? job.GetType().Namespace;
        }

        public static string GetDescription(this BaseJob job)
        {
            return GetMetadataAttribute(job.GetType())?.Description ?? string.Empty;
        }

        public static string GetLogger(this BaseJob job)
        {
            return GetMetadataAttribute(job.GetType())?.Logger ?? Constants.System.DefaultSchedulerLoggerName;
        }

        private static JobMetadataAttribute GetMetadataAttribute(Type type)
        {
            return (JobMetadataAttribute)Attribute.GetCustomAttribute(type, typeof(JobMetadataAttribute));
        }
    }
}
