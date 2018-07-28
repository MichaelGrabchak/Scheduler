using Owin;

namespace Scheduler.WebConsole.Hubs
{
    public class SignalRConfig
    {
        public static void SetupSinglaR(IAppBuilder builder)
        {
            builder.MapSignalR();
        }
    }
}
