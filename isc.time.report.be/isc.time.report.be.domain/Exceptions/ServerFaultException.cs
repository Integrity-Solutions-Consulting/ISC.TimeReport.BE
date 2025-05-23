﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace isc.time.report.be.domain.Exceptions
{
    public class ServerFaultException : BaseCustomException
    {
        public Exception InnerException { get; set; }
        public ServerFaultException(string message = "unespected error", int code = 500, Exception innerException=null, string stackTrace=null ) : base(message, stackTrace, code) 
        {
            InnerException = innerException;
        }
    }
}
