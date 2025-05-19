using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace isc.time.report.be.domain.Exceptions
{
    public class ClientFaultException : BaseCustomException
    {

        public ClientFaultException(string message = "Check the request reaload", int code = 400, string stackTrace=null) : base(message, stackTrace, code) 
        {
        
        }



    }
}
