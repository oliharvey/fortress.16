using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace KaiOs_Registration.Models.Api.RequestObjects
{
    public class SMSCodeRequest
    {
        [Required(ErrorMessage = "UserId is required")]
        [Range(0, int.MaxValue, ErrorMessage = "UserId must be a number")]
        public int UserId { get; set; }

        [Required(ErrorMessage = "DeviceId is required")]
        [Range(0, int.MaxValue, ErrorMessage = "DeviceId must be a number")]
        public int DeviceId { get; set; }

        [Required(ErrorMessage = "Device phone number is required")]
        public string DevicePhoneNumber { get; set; }

        //[Required(ErrorMessage = "Message Hash is required")]
        public string Hash { get; set; }
        //[Required(ErrorMessage = "APIVersion is required")]
        public string APIVersion { get; set; }
    }
}