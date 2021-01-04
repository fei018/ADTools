using ADServiceLibCore;
using ADServiceLibCore.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using SqlSugar;

namespace ServiceCenter.Models
{
    public class ADToolsService : IADToolsService
    {
        public ADToolsService(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _ADToolsDbContext = GetADToolsDbContext(configuration.GetConnectionString("ADToolsDb"));
            _httpContext = httpContextAccessor.HttpContext;
        }

        private HttpContext _httpContext;

        #region ADToolsDbContext ( ADToolsDb 数据库上下文)
        private ISqlSugarClient _ADToolsDbContext;

        private ISqlSugarClient GetADToolsDbContext(string connString)
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
        #endregion

        #region public services provide
        public IADService ADService => new ADService(new ADServiceDb(_ADToolsDbContext,_httpContext));

        #endregion
    }
}
