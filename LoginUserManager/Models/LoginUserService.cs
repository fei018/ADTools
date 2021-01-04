using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace LoginUserManager.Models
{
    public class LoginUserService : ILoginUserService
    {
        private readonly ISimpleClient<LoginUser> _tblLoginUser;

        private readonly ISimpleClient<LoginErrorCountLimit> _tblError;

        public HttpContext HttpContext { get; set; }

        public LoginUserService(ILoginUserDb db)
        {
            _tblLoginUser = db.Tbl_LoginUser;
            _tblError = db.Tbl_LoginAllowErrorCount;
        }

        #region 登入
        public async Task<LoginResult<LoginUser>> Login(string userName, string password)
        {
            var result = new LoginResult<LoginUser>();

            //检查 login name
            var dbuser = await _tblLoginUser.AsQueryable().FirstAsync(u => u.Name.Equals(userName, StringComparison.OrdinalIgnoreCase));
            if (dbuser == null) return result.ToReturn("UserName or Password invalid.");

            //检查 锁定, true 表示锁定
            if (dbuser.AccountLocked)
            {
                return result.ToReturn("Account is Lockout.");
            }

            //检查 login错误次数
            if (IsLoginErrorCountNeedToLock(dbuser))
            {
                return result.ToReturn("Account is Lockout.");
            }

            // 检查 密码，错误 LoginErrorCoun +1
            if (IsAccountPasswordTrue(dbuser,password))
            {
                await SignIn(dbuser);
                return result.ToReturn(dbuser);
            }
            else
            {
                return result.ToReturn("UserName or Password invalid.");
            }
        }

        private bool IsLoginErrorCountNeedToLock(LoginUser user)
        {
            var limit = GetLoginErrorCountLimit().Result.Value.Count;

            if (user.LoginErrorCount > limit)
            {
                user.AccountLocked = true;
                _tblLoginUser.Update(user);
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool IsAccountPasswordTrue(LoginUser user, string pass)
        {
            //检查 密码
            if (user.Password == pass)
            {
                user.LoginErrorCount = 0;
                _tblLoginUser.Update(user);
                return true;
            }

            user.LoginErrorCount += 1;
            _tblLoginUser.Update(user);
            return false;
        }

        private async Task SignIn(LoginUser user)
        {
            var claimIndentity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
            claimIndentity.AddClaim(new Claim(ClaimTypes.Name, user.Name));
            claimIndentity.AddClaim(new Claim(ClaimTypes.Role, user.RoleName));

            var principal = new ClaimsPrincipal(claimIndentity);

            //验证参数内容
            var authProperties = new AuthenticationProperties
            {
                AllowRefresh = true,

                //是否永久保存cookie
                IsPersistent = false
            };

            await HttpContext?.SignInAsync(principal, authProperties);
        }
        #endregion

        #region 登出
        public async Task Logout()
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                await HttpContext.SignOutAsync();
            }
        }
        #endregion

        #region Get LoginUser List
        public async Task<LoginResult<List<LoginUser>>> GetAllUser()
        {
            var result = new LoginResult<List<LoginUser>>();

            var all = await _tblLoginUser.AsQueryable().OrderBy(u => u.Id, SqlSugar.OrderByType.Asc).ToListAsync();
            if (all != null && all.Any())
            {
                return result.ToReturn(all);
            }
            return result.ToReturn("No users in database.");
        }
        #endregion

        #region 新建 LoginUser
        public async Task<LoginResult<LoginUser>> CreateLoginUser(LoginUser newUser)
        {
            var result = new LoginResult<LoginUser>();

            var exist = await _tblLoginUser.AsQueryable().AnyAsync(u => u.Name == newUser.Name);
            if (exist)
            {
                return result.ToReturn("User Existed.");
            }

            var user = await _tblLoginUser.AsInsertable(newUser).ExecuteReturnEntityAsync();

            return result.ToReturn(user);
        }
        #endregion

        #region 更改 LoginUser RoleName
        public async Task<LoginResult<LoginUser>> ChangeUserRoleById(int id, string role)
        {
            var result = new LoginResult<LoginUser>();

            var user = await _tblLoginUser.GetByIdAsync(id);
            if (user == null)
            {
                return result.ToReturn($"Id={id} User not exist");
            }

            user.RoleName = role;
            var up = await _tblLoginUser.UpdateAsync(user);
            if (up)
            {
                return result.ToReturn(user);
            }

            return result.ToReturn($"LoginUser: {user.Name} change Role failed.");
        }
        #endregion

        #region 更改 LoginUser Password
        public async Task<LoginResult<LoginUser>> ChangeUserPasswordById(int id, string password)
        {
            var result = new LoginResult<LoginUser>();

            var user = await _tblLoginUser.GetByIdAsync(id);
            if (user == null)
            {
                return result.ToReturn($"Id={id} User not exist.");
            }

            user.Password = password;
            var up = await _tblLoginUser.UpdateAsync(user);
            if (up)
            {
                return result.ToReturn(user);
            }

            return result.ToReturn($"LoginUser: {user.Name} change password failed.");
        }
        #endregion

        #region Get LoginUser
        public async Task<LoginResult<LoginUser>> GetUserById(int id)
        {
            var result = new LoginResult<LoginUser>();
            var user = await _tblLoginUser.GetByIdAsync(id);

            if (user == null)
            {
                return result.ToReturn($"Id={id} User not exist.");
            }

            return result.ToReturn(user);
        }
        #endregion

        #region Delete LoginUser
        public async Task<LoginResult<LoginUser>> DeleteUserById(int id)
        {
            var result = new LoginResult<LoginUser>();

            var del = await _tblLoginUser.DeleteByIdAsync(id);

            if (del)
            {
                return result.ToReturn($"Id={id} User deleted");
            }

            return result.ToReturn($"Id={id} User delete failed");
        }
        #endregion

        #region Set LoginErrorCountLimit
        public async Task<LoginResult<LoginErrorCountLimit>> SetLoginErrorCountLimit(int count)
        {
            var result = new LoginResult<LoginErrorCountLimit>();

            var all = await _tblError.GetListAsync();
            if (all != null && all.Any())
            {
                await _tblError.AsDeleteable().Where(all).ExecuteCommandAsync();
            }

            var allow = new LoginErrorCountLimit { Count = count };
            var set = await _tblError.InsertAsync(allow);
            if (set)
            {
                return result.ToReturn(allow);
            }
            return result.ToReturn("Cannot Set LoginErrorCountLimit.");
        }
        #endregion

        #region Get LoginErrorCountLimit, 错误返回 count = 10
        public async Task<LoginResult<LoginErrorCountLimit>> GetLoginErrorCountLimit()
        {
            var result = new LoginResult<LoginErrorCountLimit>();

            var first = await _tblError.AsQueryable().FirstAsync();
            if (first != null)
            {
                return result.ToReturn(first);
            }

            return result.ToReturn(new LoginErrorCountLimit { Count = 10 });
        }
        #endregion
    }
}
