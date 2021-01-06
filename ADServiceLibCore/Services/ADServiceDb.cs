using ADServiceLibCore.Models;
using Microsoft.AspNetCore.Http;
using SqlSugar;

namespace ADServiceLibCore.Services
{
    public class ADServiceDb : IADServiceDb
    {
        public ADServiceDb(ISqlSugarClient context, HttpContext httpContext)
        {
            Tbl_DomainInfo = new SimpleClient<DomainInfo>(context);
            HttpContext = httpContext;
        }

        public ISimpleClient<DomainInfo> Tbl_DomainInfo { get; private set; }

        public HttpContext HttpContext { get; private set; }
    }
}
