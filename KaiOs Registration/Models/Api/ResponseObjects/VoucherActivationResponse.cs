using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KaiOs_Registration.Models.Api.ResponseObjects
{
    public class VoucherActivationResponse
    {
        public int UserId { get; set; }
        public int DeviceId { get; set; }
        public string VoucherCode { get; set; }
        public string CodeResponseStatus { get; set; }
        public string MembershipLength { get; set; }
        public string MembershipTier { get; set; }
        public string StatusMessage { get; set; }
    }
}