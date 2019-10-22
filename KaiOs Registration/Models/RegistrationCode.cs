using System.ComponentModel.DataAnnotations;
namespace KaiOs_Registration.Models
{
    public class RegistrationCode
    {
        [Required]
        public string ReferenceCode { get; set; }        
    }
}