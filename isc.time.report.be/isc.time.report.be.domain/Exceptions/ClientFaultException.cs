using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace isc.time.report.be.domain.Exceptions
{
    //public class ClientFaultException : BaseCustomException
    //{

    //    public ClientFaultException(string message = "Check the request reaload", int code = 400, string stackTrace=null) : base(message, stackTrace, code) 
    //    {

    //    }



    //}


    public class ClientFaultException : BaseCustomException
    {
        public ClientFaultException(string message = "Check the request", int code = 400)
            : base(message, code)
        {
        }

        public ClientFaultException(string message, int code, Exception innerException)
            : base(message, code, innerException)
        {
        }
    }
}
