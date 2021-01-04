using LoginUserManager.Models;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LoginUserManager
{
    public interface ILoginUserService
    {
        HttpContext HttpContext { get; set; }

        Task<LoginResult<LoginUser>> Login(string userName, string password);

        Task Logout();

        Task<LoginResult<List<LoginUser>>> GetAllUser();

        Task<LoginResult<LoginUser>> CreateLoginUser(LoginUser newUser);

        Task<LoginResult<LoginUser>> ChangeUserRoleById(int id, string role);

        Task<LoginResult<LoginUser>> ChangeUserPasswordById(int id, string password);

        Task<LoginResult<LoginUser>> GetUserById(int id);

        Task<LoginResult<LoginUser>> DeleteUserById(int id);

        Task<LoginResult<LoginErrorCountLimit>> SetLoginErrorCountLimit(int count);
    }
}
