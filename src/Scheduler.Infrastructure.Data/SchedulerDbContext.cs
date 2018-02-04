using System;
using System.Data;
using System.Data.SqlClient;

using Scheduler.Core.Context;
using Scheduler.Domain.Data;
using Scheduler.Domain.Data.BusinessEntities;
using Scheduler.Domain.Data.Repositories;
using Scheduler.Infrastructure.Data.Repositories;

namespace Scheduler.Infrastructure.Data
{
    public class SchedulerDbContext : IDbContext
    {
        private IDbConnection _connection;
        private IDbTransaction _transaction;
        private bool _disposed;

        private IRepository<JobDetail> _jobDetailRepository;
        private IRepository<SchedulerInstance> _schedulerInstanceRepository;
        private IRepository<SchedulerInstanceSetting> _schedulerInstanceSettingRepository;

        public SchedulerDbContext(ISchedulerContext context)
        {
            _connection = new SqlConnection(context.ConnectionString);
            _connection.Open();
            _transaction = _connection.BeginTransaction();
        }

        public IRepository<JobDetail> JobDetails => _jobDetailRepository ?? (_jobDetailRepository = new Repository<JobDetail>(_transaction));

        public IRepository<SchedulerInstance> SchedulerInstances => _schedulerInstanceRepository ?? (_schedulerInstanceRepository = new Repository<SchedulerInstance>(_transaction));

        public IRepository<SchedulerInstanceSetting> SchedulerInstanceSettings => _schedulerInstanceSettingRepository ?? (_schedulerInstanceSettingRepository = new Repository<SchedulerInstanceSetting>(_transaction));

        public void Commit()
        {
            try
            {
                _transaction.Commit();
            }
            catch
            {
                _transaction.Rollback();
                throw;
            }
            finally
            {
                _transaction.Dispose();
                _transaction = _connection.BeginTransaction();
                ResetRepositories();
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~SchedulerDbContext()
        {
            Dispose(false);
        }

        #region Private methods

        private void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    if (_transaction != null)
                    {
                        _transaction.Dispose();
                        _transaction = null;
                    }
                    if (_connection != null)
                    {
                        _connection.Dispose();
                        _connection = null;
                    }
                }
                _disposed = true;
            }
        }

        private void ResetRepositories()
        {
            _jobDetailRepository = null;
            _schedulerInstanceRepository = null;
            _schedulerInstanceSettingRepository = null;
        }

        #endregion
    }
}
