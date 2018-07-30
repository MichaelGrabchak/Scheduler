using System.Threading;
using System.Threading.Tasks;

using Quartz;

namespace Scheduler.Engine.Quartz.Listeners
{
    public class DependentTriggerListener : ITriggerListener
    {
        public string Name => "DependentTriggerListener";

        public Task TriggerFired(ITrigger trigger, IJobExecutionContext context, CancellationToken cancellationToken = new CancellationToken())
        {
            return Task.Run(() =>
            {

            }, cancellationToken);
        }

        public Task TriggerMisfired(ITrigger trigger, CancellationToken cancellationToken = new CancellationToken())
        {
            return Task.Run(() =>
            {

            }, cancellationToken);
        }

        public Task TriggerComplete(ITrigger trigger, IJobExecutionContext context, SchedulerInstruction triggerInstructionCode, CancellationToken cancellationToken = new CancellationToken())
        {
            return Task.Run(() =>
            {

            }, cancellationToken);
        }

        public Task<bool> VetoJobExecution(ITrigger trigger, IJobExecutionContext context, CancellationToken cancellationToken = new CancellationToken())
        {
            return Task.Run(() =>
            {
                //var jobDetail = context.JobDetail?.Key;

                //if (jobDetail != null)
                //{
                //    return (jobDetail.Name == "HelloWorldJob");
                //}

                return false;
            }, cancellationToken);
        }
    }
}
