using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KaiOs_Registration.Models.Api.ResponseObjects
{
    public class GetOfflineTagResponse
    {
        public int id { get; set; }
        public string status { get; set; }
        public int? UserID { get; set; }
        public int? DeviceID { get; set; }

        public string DeviceMake { get; set; }
        public string DeviceModel { get; set; }
        public string DeviceCapacity { get; set; }
        public string DeviceSerialNo { get; set; }
        public string CountryISO { get; set; }

    }
}