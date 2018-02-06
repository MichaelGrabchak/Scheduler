using Scheduler.Jobs;
using Scheduler.Jobs.Attributes;

namespace MyAmazing.TaskLib
{
    [JobMetadata(Name = "GoodbyeWorldJobName", Description = "Expected Result: Failure")]
    public class GoodbyeWorldJob : BaseJob
    {
        public override string Schedule => "0 0/5 * 1/1 * ? *";

        public override void ExecuteJob()
        {
            throw new System.NotImplementedException("Not implemented. Though, let's see what happens with this job...");
        }
    }
}
