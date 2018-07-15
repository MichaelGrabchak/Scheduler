using System;
using System.Linq;
using System.Collections.Generic;
using System.Data.Entity;

using Scheduler.Domain.Data.EntityFramework.ContextProviders;
using Scheduler.Domain.Data.Repositories;
using Scheduler.Core.Context;

namespace Scheduler.Infrastructure.Data.EntityFramework.Repositories
{
    public abstract class BaseRepository<TEntity, TIdentity> : IRepository<TEntity, TIdentity>
        where TEntity : class
    {
        protected readonly DbContext _dbContext;
        protected readonly IContext _schedulerContext;
        protected readonly DbSet<TEntity> _dbSet;

        protected BaseRepository(IDbContextProvider dbContextProvider, IContext schedulerContext)
        {
            if(dbContextProvider == null || dbContextProvider.DbContext == null)
            {
                throw new ArgumentNullException(nameof(dbContextProvider));
            }

            _dbContext = dbContextProvider.DbContext;
            _schedulerContext = schedulerContext;
            _dbSet = _dbContext.Set<TEntity>();
        }

        #region Create

        public void Add(TEntity entity)
        {
            AddEntity(entity);
            SaveChanges();
        }

        #endregion

        #region Read

        public IEnumerable<TEntity> GetAll()
        {
            return _dbSet.ToList();
        }

        public TEntity GetById(TIdentity id)
        {
            return _dbSet.Find(id);
        }

        #endregion

        #region Update

        public void Update(TEntity entity)
        {
            UpdateEntity(entity);
            SaveChanges();
        }

        #endregion

        #region Delete

        public void Delete(TEntity entity)
        {
            DeleteEntity(entity);
            SaveChanges();
        }

        #endregion

        #region Create/Update

        public void AddOrUpdate(TEntity entity)
        {
            if (!UpdateEntity(entity))
            {
                AddEntity(entity);
            }
            SaveChanges();
        }

        #endregion

        #region Protected virtual methods

        protected virtual void AddEntity(TEntity entity)
        {
            _dbSet.Add(entity);
        }

        protected virtual bool UpdateEntity(dynamic entity)
        {
            var original = GetById(entity.Id);
            if (original == null)
            {
                return false;
            }

            _dbContext.Entry(original).CurrentValues.SetValues(entity);
            return true;
        }

        protected virtual void DeleteEntity(TEntity entity)
        {
            _dbSet.Remove(entity);
        }

        #endregion

        #region Helpers

        protected void SaveChanges()
        {
            _dbContext.SaveChanges();
        }

        #endregion
    }
}
