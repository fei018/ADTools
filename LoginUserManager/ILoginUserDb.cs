using LoginUserManager.Models;
using Microsoft.AspNetCore.Http;
using SqlSugar;

namespace LoginUserManager
{
    public interface ILoginUserDb
    {
        ISimpleClient<LoginUser> Tbl_LoginUser { get; }

        ISimpleClient<LoginErrorCountLimit> Tbl_LoginAllowErrorCount { get; }
    }
}
