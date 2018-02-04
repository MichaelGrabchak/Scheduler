using System;

namespace Scheduler.Domain.Data.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class NotMappedAttribute : Attribute
    {
        public NotMappedAttribute()
        {
        }
    }
}
