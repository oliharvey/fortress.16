using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using KaiOs_Registration.Helpers;
using KaiOs_Registration.CustomAttributes;
namespace KaiOs_Registration.Models.Api.RequestObjects
{
    public class UserRegistrationRequest
    {
        private string _postcode;

        [Required(ErrorMessage = "First Name is required")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Last Name is required")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Email Address is required")]
        [EmailAddress(ErrorMessage = "Email address invalid")]
        public string EmailAddress { get; set; }
        public string Password { get; set; }
        [Required(ErrorMessage = "Postcode is required")]
        [CustomPostcodeValidator("CountryIso", ErrorMessage = "Postcode is invalid")]
        public string Postcode
        {
            get { return this._postcode.TrimEnd(); }
            set { this._postcode = value; }
        }
        [Required(ErrorMessage = "Country is required")]
        public string Country { get; set; }
        [Required(ErrorMessage = "Country Iso is required")]
        public string CountryIso { get; set; }
        [Required(ErrorMessage = "Device Make is required")]
        public string DeviceMake { get; set; }
        [Required(ErrorMessage = "Device Model is required")]
        public string DeviceModel { get; set; }
        //[Required(ErrorMessage = "Device Model Raw is required")]
        public string DeviceModelRaw { get; set; }
        //[Required(ErrorMessage = "Device Capacity Raw is required")]
        public string DeviceCapacityRaw { get; set; }
        [Required(ErrorMessage = "Os Version is required")]
        public string OsVersion { get; set; }
        [Required(ErrorMessage = "Source Language is required")]
        public string SourceLanguage { get; set; }
        public string Imei { get; set; }
        public string AltImei { get; set; }
        public bool ApplyCover { get; set; }

        //New Registration Process
        public bool? Over18 { get; set; }

        public byte[] Salt { get; set; }
        public byte[] Hash { get; set; }
    }
}
