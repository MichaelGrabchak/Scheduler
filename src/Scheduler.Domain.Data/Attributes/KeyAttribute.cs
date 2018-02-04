using System;

namespace Scheduler.Domain.Data.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class KeyAttribute : Attribute
    {
        public KeyAttribute(int columnOrder = 0)
        {
            ColumnOrder = columnOrder;
        }

        public int ColumnOrder { get; private set; }
    }
}
