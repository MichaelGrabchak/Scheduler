using Quartz;

namespace Scheduler.Engine.Quartz.Listeners
{
    public class DependentTriggerListener : ITriggerListener
    {
        public string Name => "DependentTriggerListener";

        public void TriggerComplete(ITrigger trigger, IJobExecutionContext context, SchedulerInstruction triggerInstructionCode)
        {
            
        }

        public void TriggerFired(ITrigger trigger, IJobExecutionContext context)
        {
            
        }

        public void TriggerMisfired(ITrigger trigger)
        {
            
        }

        public bool VetoJobExecution(ITrigger trigger, IJobExecutionContext context)
        {
            var jobDetail = context.JobDetail?.Key;

            if (jobDetail != null)
            {
                return (jobDetail.Name == "HelloWorldJob");
            }

            return false;
        }
    }
}
