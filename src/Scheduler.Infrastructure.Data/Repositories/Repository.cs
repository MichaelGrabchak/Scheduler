using System.Collections.Generic;
using System.Data;
using System.Linq;

using Dapper;

using Scheduler.Domain.Data.Enums;
using Scheduler.Domain.Data.Repositories;
using Scheduler.Infrastructure.Data.Utils;

namespace Scheduler.Infrastructure.Data.Repositories
{
    public class Repository<T> : IRepository<T>
    {
        private readonly TableMap<T> _tableMap;

        protected string TableName => _tableMap?.TableName;

        protected IDbTransaction Transaction { get; private set; }

        protected IDbConnection Connection => Transaction.Connection;

        #region Public methods

        public Repository(IDbTransaction transaction)
        {
            _tableMap = TableMap<T>.Create();
            Transaction = transaction;
        }

        public virtual IEnumerable<T> GetAll()
        {
            return Connection.Query<T>(
                sql: BuildSqlSelect(),
                transaction: Transaction
            );
        }

        public virtual T GetByKey(object key)
        {
            return Connection.Query<T>(
                sql: string.Format("{0} WHERE {1}", BuildSqlSelect(), BuildSqlKeyConstraint()),
                param: key,
                transaction: Transaction
            ).FirstOrDefault();
        }

        public virtual void Add(T obj)
        {
            var columnsPart = string.Join(", ", _tableMap.PropertyMaps
                .Where(x => x.DatabaseGeneratedOption == DatabaseGeneratedOption.None)
                .Select(x => x.ColumnName));

            var paramsPart = string.Join(", ", _tableMap.PropertyMaps
                .Where(x => x.DatabaseGeneratedOption == DatabaseGeneratedOption.None)
                .Select(x => string.Format("@{0}", x.PropertyName)));

            var sql = string.Format("INSERT INTO {0}({1}) VALUES({2})", _tableMap.TableName, columnsPart, paramsPart);

            var identity = _tableMap.PropertyMaps.FirstOrDefault(x => x.DatabaseGeneratedOption == DatabaseGeneratedOption.Identity);
            if (identity == null)
            {
                Connection.Execute(
                    sql: sql,
                    param: obj,
                    transaction: Transaction
                );
            }
            else
            {
                sql = string.Format("{0}; SELECT SCOPE_IDENTITY()", sql);
                var key = Connection.ExecuteScalar(
                    sql: sql,
                    param: obj,
                    transaction: Transaction
                );
                var prop = typeof(T).GetProperty(identity.PropertyName);
                if (prop != null)
                    prop.SetValue(obj, key);
            }
        }

        public virtual void Update(T obj)
        {
            var setsPart = string.Join(" AND ", _tableMap.PropertyMaps.Where(x => x.DatabaseGeneratedOption == DatabaseGeneratedOption.None).Select(x => string.Format("{0} = @{1}", x.ColumnName, x.PropertyName)));
            var sql = string.Format("UPDATE {0} SET {1} WHERE {2}", _tableMap.TableName, setsPart, BuildSqlKeyConstraint());
            Connection.Execute(
                sql: sql,
                param: obj,
                transaction: Transaction
            );
        }

        public virtual void Delete(object key)
        {
            Connection.Execute(
                sql: string.Format("DELETE FROM {0} WHERE {1}", _tableMap.TableName, BuildSqlKeyConstraint()),
                param: key,
                transaction: Transaction
            );
        }

        #endregion

        #region Private methods

        protected string BuildSqlSelect()
        {
            return string.Format("SELECT {0} FROM {1}", string.Join(", ", _tableMap.PropertyMaps
                    .Select(x => x.ColumnName == x.PropertyName
                        ? x.ColumnName
                        : string.Format("{0} AS {1}", x.ColumnName, x.PropertyName)
                    )
                ), _tableMap.TableName);
        }

        protected string BuildSqlKeyConstraint()
        {
            return string.Join(" AND ", _tableMap.PropertyMaps
                .Where(x => x.KeyOrder.HasValue)
                .OrderBy(x => x.KeyOrder.Value)
                .Select(x => string.Format("{0} = @{1}", x.ColumnName, x.PropertyName))
            );
        }

        #endregion
    }
}
