using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KaiOs_Registration.Models.Api.ResponseObjects
{
    public class ApiError
    {
        public ApiError(string errorCode)
        {
            ErrorCode = errorCode;
        }

        public string ErrorCode { get; set; }
    }
}