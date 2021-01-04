using ADServiceLibCore.Models;
using SqlSugar;

namespace ADToolsDatabaseManager
{
    public class ADToolsDatabase
    {
        public void CodeFirstCreateDb()
        {
            ReaderJsonConfig config = new ReaderJsonConfig();

            var db = new SqlSugarClient(new ConnectionConfig()
            {
                DbType = SqlSugar.DbType.SqlServer,
                ConnectionString = config.ConnectionString,
                InitKeyType = InitKeyType.Attribute,
                IsAutoCloseConnection = true
            });

            db.DbMaintenance.CreateDatabase();
            db.CodeFirst.SetStringDefaultLength(50).InitTables(typeof(DomainInfo));
        }
    }
}
