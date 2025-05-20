﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace isc.time.report.be.domain.Exceptions
{
    public class BaseCustomException : Exception
    {
       public int Code { get; set; }
        public override string StackTrace { get; }

        public BaseCustomException(string message, string stackTrace, int code) : base(message) 
        {
            Code = code;
            StackTrace = stackTrace;
        }
    }
}
