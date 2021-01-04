using LoginUserManager.Models;

namespace LoginUserManager
{
    public interface ILoginResult<T>
    {
        bool Success { get; set; }

        string Error { get; set; }

        T Value { get; set; }

        LoginResult<T> ToReturn(T value);

        LoginResult<T> ToReturn(string error);
    }
}
