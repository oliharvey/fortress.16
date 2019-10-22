using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KaiOs_Registration.Models.Api.RequestObjects
{
    public class ActivateOfflineTagRequest
    {
        public string ReferenceCode { get; set; }
        public int DeviceId { get; set; }
    }
}