namespace isc.time.report.be.domain.Exceptions
{
    //public class BaseCustomException : Exception
    //{
    //   public int Code { get; set; }
    //    public override string StackTrace { get; }

    //    public BaseCustomException(string message, string stackTrace, int code) : base(message) 
    //    {
    //        Code = code;
    //        StackTrace = stackTrace;
    //    }
    //}

    public class BaseCustomException : Exception
    {
        public int Code { get; set; }

        public BaseCustomException(string message, int code = 500)
            : base(message)
        {
            Code = code;
        }

        public BaseCustomException(string message, int code, Exception innerException)
            : base(message, innerException)
        {
            Code = code;
        }
    }
}
