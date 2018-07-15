using System;

using Scheduler.Jobs.Attributes;

namespace Scheduler.Jobs.Extensions
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
            return GetMetadataAttribute(job.GetType())?.Logger ?? null;
        }

        private static JobMetadataAttribute GetMetadataAttribute(Type type)
        {
            return (JobMetadataAttribute)Attribute.GetCustomAttribute(type, typeof(JobMetadataAttribute));
        }
    }
}
