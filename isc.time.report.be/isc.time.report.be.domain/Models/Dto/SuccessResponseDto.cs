namespace isc.time.report.be.domain.Models.Dto
{
    public class SuccessResponse<T>
    {
        public string TraceId { get; set; }
        public T Data { get; set; }
    }
}
