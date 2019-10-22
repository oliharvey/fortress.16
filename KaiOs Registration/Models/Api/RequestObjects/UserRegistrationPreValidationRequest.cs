using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KaiOs_Registration.Models.Api.RequestObjects
{
    public class UserRegistrationPreValidationRequest
    {
        public string DeviceMake { get; set; }
        public string DeviceModel { get; set; }
        public string DeviceCapacityRaw { get; set; }
        public string CountryIso { get; set; }
        public string EmailAddress { get; set; }
        public string VoucherCode { get; set; }
        public int KaiosPartner { get; set; }
        public bool validateDevice { get; set; }
        public bool validateUser { get; set; }
        public bool validateVoucher { get; set; }
    }
}