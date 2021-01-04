using ADServiceLibCore;
using ADServiceLibCore.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using SqlSugar;

namespace ServiceCenter.Models
{
    public class ADToolsServiceContext : IADToolsServiceContext
    {
        public ADToolsServiceContext(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            var connstring = configuration.GetConnectionString("ADToolsDb");

            _context = GetSqlSugarClient(connstring);
        }

        private ISqlSugarClient _context;

        private ISqlSugarClient GetSqlSugarClient(string connString)
        {
            var client = new SqlSugarClient(new ConnectionConfig()
            {
                ConnectionString = connString,
                DbType = DbType.SqlServer,
                IsAutoCloseConnection = true,
                InitKeyType = InitKeyType.Attribute
            });
            return client;
        }


        public IADServiceDb Db_ADService => new ADServiceDb(_context);
    }
}
