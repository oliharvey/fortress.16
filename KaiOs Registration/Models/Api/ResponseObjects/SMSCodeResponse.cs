using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KaiOs_Registration.Models.Api.ResponseObjects
{
    public class SMSCodeResponse
    {
        public int UserId { get; set; }
        public string DevicePhoneNumber { get; set; }
        public string DeviceSmsCode { get; set; }
    }
}