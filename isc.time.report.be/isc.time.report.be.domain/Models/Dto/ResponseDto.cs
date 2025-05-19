using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace isc.time.report.be.domain.Models.Dto
{
    public class ResponseDto<T>
    {
        public string TraceId { get; set; }
        public T Data { get; set; }

        public ResponseDto(T data, string traceId)
        {
            TraceId = traceId;
            Data = data;
        }

    }
}
