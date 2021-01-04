namespace LoginUserManager.Models
{
    public class LoginResult<T>
    {
        public bool Success { get; set; }

        private string _error;

        public string Error
        {
            get { return _error; }
            set { Success = false; _error = "Error: " + value; }
        }

        private T _value;

        public T Value
        {
            get { return _value; }
            set { Success = true; _value = value; }
        }


        public LoginResult<T> ToReturn(T value)
        {
            Value = value;
            return this;
        }

        public LoginResult<T> ToReturn(string error)
        {
            Error = error;
            return this;
        }
    }
}
