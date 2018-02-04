using Scheduler.Domain.Data.Enums;
using System;

namespace Scheduler.Domain.Data.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class DatabaseGeneratedAttribute : Attribute
    {
        public DatabaseGeneratedAttribute(DatabaseGeneratedOption databaseGeneratedOption)
        {
            DatabaseGeneratedOption = databaseGeneratedOption;
        }

        public DatabaseGeneratedOption DatabaseGeneratedOption { get; private set; }
    }
}
