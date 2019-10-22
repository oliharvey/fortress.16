using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KaiOs_Registration.Models.Api.ResponseObjects
{
    public class SmsCodeVerificationResponse
    {
        public int UserId { get; set; }
        public int DeviceId { get; set; }
        public string DevicePhoneNumber { get; set; }
    }
}