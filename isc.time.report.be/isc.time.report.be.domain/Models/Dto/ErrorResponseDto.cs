using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace isc.time.report.be.domain.Models.Dto
{
    public class ErrorResponseDto
    {
        public string TraceId { get; set; }
        public int Code { get; set; }
        public string Message { get; set; }
        public List<ErrorData> Error { get; set; }

    }

    public class ErrorData
    {
        public int Code { get; set; }
        public string Message { get; set; }
    }
}
