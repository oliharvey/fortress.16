using System.ComponentModel.DataAnnotations;
namespace KaiOs_Registration.Models.Api.RequestObjects
{
    public class GetErrorDescriptionRequest
    {
        [Required]
        public string ErrorCode { get; set; }
    }
}