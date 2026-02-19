namespace isc.time.report.be.domain.Models.Response.Shared
{
    public class SuccessResponse<T> : BaseResponse
    {

        public T Data { get; set; }

        public SuccessResponse(int code, string message, T data) : base(code, message)
        {
            Data = data;
        }


    }
}
