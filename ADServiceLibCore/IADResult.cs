using ADServiceLibCore.Models;
using ADServiceLibCore.Services;

namespace ADServiceLibCore
{
    public interface IADResult<T>
    {
        bool Success { get; set; }

        string Error { get; set; }

        T Value { get; set; }

        IADResult<T> ToReturn(T value);

        IADResult<T> ToReturn(string error);

        IADResult<T> ToReturn(bool success, T value);
    }
}
