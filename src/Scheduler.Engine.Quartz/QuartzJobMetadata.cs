using Scheduler.Core.Jobs.Metadata;

using Quartz;

namespace Scheduler.Engine.Quartz
{
    public class QuartzJobMetadata : JobMetadata
    {
        protected override bool IsScheduleExpressionValid(string schedule)
        {
            return CronExpression.IsValidExpression(schedule);
        }
    }
}
