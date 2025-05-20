using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace isc.time.report.be.domain.Models.Response.Shared
{ 
    public class ErrorResponse : BaseResponse
    {

        public ErrorResponse(int code, string message, Error error) : base(code, message) {

            Error = error;
        }
        public Error Error { get; set; }

    }


    public struct Error
    {

        public int Code { get; set; }
        public string ErrorMessage { get; set; }
    }

}
