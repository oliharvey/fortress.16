using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using KaiOs_Registration.CustomAttributes;
namespace KaiOs_Registration.Models
{
    public class Registration
    {
        public int DeviceID { get; set; }
        public int UserID { get; set; }
        public int CurrentStep { get; set; }
        public string Password { get; set; }
        [Required]
        [DisplayName("Reference Code")]
        public string ReferenceCode { get; set; }
        [StringLength(100)]
        [DisplayName("First Name")]
        [Required]
        public string FirstName { get; set; }
        [StringLength(100)]
        [DisplayName("Last Name")]
        [Required]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Country is required")]
        public string Country { get; set; }
        [Required(ErrorMessage = "Email Address is required")]
        [EmailAddress(ErrorMessage = "Email address invalid")]
        [DisplayName("Email Address")]
        public string EmailAddress { get; set; }
        [Required(ErrorMessage = "Postcode is required")]
        [System.Web.Mvc.Remote("CheckPostcode", "Registration", AdditionalFields = "Country")]
        public string Postcode { get; set; }
        [Required(ErrorMessage = "Phone number is required")]
        [DisplayName("Device Phone Number")]
        public string DevicePhoneNumber { get; set; }
        
        [DisplayName("SMS Validation Code")]
        public string SmsCode { get; set; }
        [DisplayName("Voucher Code")]
        public string VoucherCode { get; set; }
        public static IEnumerable<SelectListItem> GetCountrySelectItems()
        {
            yield return new SelectListItem { Text = "United States", Value = "United States" };
            yield return new SelectListItem { Text = "United Kingdom", Value = "United Kingdom" };
        }

    }
}