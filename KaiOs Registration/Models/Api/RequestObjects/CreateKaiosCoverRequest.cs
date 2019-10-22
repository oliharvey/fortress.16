using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KaiOs_Registration.Models.Api.RequestObjects
{
    public class CreateKaiosCoverRequest
    {
        public int UserId { get; set; }
        public int DeviceId { get; set; }
        public string VoucherCode { get; set; }
        public int KaiosPartner { get; set; }
        public string Hash { get; set; }
    }
}