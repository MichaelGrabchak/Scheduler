using System;

namespace Scheduler.Jobs.Attributes
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class JobMetadataAttribute : Attribute
    {
        public string Name { get; set; }
        public string Group { get; set; }
        public string Logger { get; set; }
        public string Description { get; set; }

        public JobMetadataAttribute()
        {

        }
    }
}
