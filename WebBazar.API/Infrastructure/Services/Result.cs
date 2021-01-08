namespace WebBazar.API.Infrastructure.Services
{
    public class Result
    {
        public bool Succeeded { get; protected set; }

        public bool Failure => !this.Succeeded;

        public string Error { get; protected set; }

        public static implicit operator Result(bool succeeded)
            => new Result
            {
                Succeeded = succeeded
            };

        public static implicit operator Result(string error)
            => new Result
            {
                Succeeded = false,
                Error = error
            };
    }

    public class Result<T> : Result
    {
        public T Data { get; private set; }
        
        public static implicit operator Result<T>(bool succeeded)
            => new Result<T>
            {
                Succeeded = succeeded
            };

        public static implicit operator Result<T>(string error)
            => new Result<T>
            {
                Succeeded = false,
                Error = error
            };
        
        public static implicit operator Result<T>(T data)
            => new Result<T>
            {
                Succeeded = true,
                Data = data
            };
    }
}