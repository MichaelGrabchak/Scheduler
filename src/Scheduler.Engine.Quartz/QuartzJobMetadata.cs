using Scheduler.Domain.Data.Services;
using Scheduler.Engine.Jobs;

using Quartz;

namespace Scheduler.Engine.Quartz
{
    public class QuartzJobMetadata : JobMetadata
    {
        public QuartzJobMetadata(IJobDetailService jobDetailService) : base(jobDetailService)
        {

        }

        protected override bool IsScheduleExpressionValid(string schedule)
        {
            return CronExpression.IsValidExpression(schedule);
        }
    }
}
