using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaiOs_Registration.Models.Api.ResponseObjects
{
    public class UserRegistrationResponse 
    {
        public int UserId { get; set; }
        public int DeviceId { get; set; }
        public VoucherActivationResponse IncludedActivatedCover { get; set; }
        public Guid? Guid { get; set; }
}
}
