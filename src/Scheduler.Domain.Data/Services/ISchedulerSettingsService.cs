namespace Scheduler.Domain.Data.Services
{
    public interface ISchedulerSettingsService
    {
        void SetSettings<T>(T settings);
        T GetSettings<T>() where T : new();
    }
}
