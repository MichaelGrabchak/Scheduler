using System.Configuration;

namespace Scheduler.Core.Context
{
    public class DataWarehouseContext : IDataWarehouseContext
    {
        public string ConnectionString { get; }

        public DataWarehouseContext()
        {
            ConnectionString = ConfigurationManager.ConnectionStrings[Constants.System.DataWarehouse.ConnectionStringKey].ConnectionString;
        }
    }
}
