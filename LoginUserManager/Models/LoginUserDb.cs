using Microsoft.AspNetCore.Http;
using SqlSugar;

namespace LoginUserManager.Models
{
    public class LoginUserDb : ILoginUserDb
    {
        private ISqlSugarClient _context;

        public LoginUserDb(ISqlSugarClient context)
        {
            _context = context;
        }

        public ISimpleClient<LoginUser> Tbl_LoginUser => new SimpleClient<LoginUser>(_context);

        public ISimpleClient<LoginErrorCountLimit> Tbl_LoginAllowErrorCount => new SimpleClient<LoginErrorCountLimit>(_context);
    }
}
