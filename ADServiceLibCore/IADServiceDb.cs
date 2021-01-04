using ADServiceLibCore.Models;
using Microsoft.AspNetCore.Http;
using SqlSugar;

namespace ADServiceLibCore
{
    public interface IADServiceDb
    {
        HttpContext HttpContext { get; }

        ISimpleClient<DomainInfo> Tbl_DomainInfo { get; }
    }
}
