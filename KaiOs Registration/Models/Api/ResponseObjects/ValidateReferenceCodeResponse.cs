using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KaiOs_Registration.Models.Api.ResponseObjects
{
    public class ValidateReferenceCodeResponse
    {
        public bool Result { get; set; }
        public string Message { get; set; }
    }
}