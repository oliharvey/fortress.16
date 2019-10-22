using System.ComponentModel.DataAnnotations;
namespace KaiOs_Registration.Models.Api.RequestObjects
{
    public class SmsCodeVerificationRequest
    {
        [Required(ErrorMessage = "UserId is required")]
        [Range(0, int.MaxValue, ErrorMessage = "UserId must be a number")]
        public int UserId { get; set; }

        [Required(ErrorMessage = "DeviceId is required")]
        [Range(0, int.MaxValue, ErrorMessage = "DeviceId must be a number")]
        public int DeviceId { get; set; }

        [Required(ErrorMessage = "Device phone number is required")]
        [RegularExpression(@"^[0-9]*$", ErrorMessage = "Device phone number must only be digits")]
        public string DevicePhoneNumber { get; set; }

        [Required(ErrorMessage = "Sms code is required")]
        public string SmsCode { get; set; }

        //[Required(ErrorMessage = "Message Hash is required")]
        public string Hash { get; set; }
        //[Required(ErrorMessage = "APIVersion is required")]
        public string APIVersion { get; set; }
    }
}