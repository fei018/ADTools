namespace ADServiceLibCore.Models
{
    public class ADResult<T>:IADResult<T>
    {
        public bool Success { get; set; }

        public string Error { get; set; }

        public T Value { get; set; }


        public IADResult<T> ToReturn(T value)
        {
            Success = true;
            Value = value;
            return this;
        }

        public IADResult<T> ToReturn(string error)
        {
            Success = false;
            Error = "Error: "+ error;
            return this;
        }

        public IADResult<T> ToReturn(bool success, T value)
        {
            Success = success;
            Value = value;
            return this;
        }
    }
}
