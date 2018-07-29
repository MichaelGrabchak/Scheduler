namespace Scheduler.Domain.Data.Services
{
    public interface ISettingsService
    {
        void SetSettings<T>(T settings);
        T GetSettings<T>() where T : new();
    }
}
