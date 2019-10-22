using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Globalization;

namespace KaiOs_Registration.Helpers
{
    public static class ApiValidator
    {
        /// <summary>
        /// Checks if the parameter is present 
        /// </summary>
        /// <param name="value"></param>
        /// <returns> Returns true if param is present, false otherwise</returns>
        public static bool ParamIsPresent(string value)
        {
            if (value == null) return false;
            return value.Length != 0;
        }
        /// <summary>
        /// Validates the email address.
        /// </summary>
        /// <param name="emailAddress">The email address.</param>
        /// <returns>True on success, false on fail</returns>
        public static bool IsEmailValid(String emailAddress)
        {
            string pattern = @"^\b[A-Z0-9._%+-]+@[A-Z0-9.-]+\.[A-Z]{2,4}\b$";
            Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);
            return regex.IsMatch(emailAddress);
        }


        /// <summary>
        /// Determines whether a telephone number only contains numbers.
        /// </summary>
        /// <param name="telephoneNumber">The telephone number.</param>
        /// <returns>True on success, false on fail</returns>
        public static bool IsTelephoneNumberValid(String telephoneNumber)
        {
            string pattern = @"^[0-9]*$";
            Regex regex = new Regex(pattern);
            return regex.IsMatch(telephoneNumber);
        }


        /// <summary>
        /// Determines whether a postcode is valid for a specific culture
        /// </summary>
        /// <param name="postCode">The post code.</param>
        /// <param name="twoLetterIsoCountry"></param>
        /// <returns>True on success, false on fail</returns>
        public static bool IsPostCodeValid(String postCode, String twoLetterIsoCountry)
        {
            Dictionary<String, String> countryPostCodeRegex = new Dictionary<String, String>();
            countryPostCodeRegex.Add("GB", @"^(([a-zA-Z]\d ?\d[a-zA-Z]{2})|([a-zA-Z]\d{2} ?\d[a-zA-Z]{2})|([a-zA-Z]{2}\d ?\d[a-zA-Z]{2})|([a-zA-Z]{2}\d{2} ?\d[a-zA-Z]{2})|([a-zA-Z]\d[a-zA-Z] ?\d[a-zA-Z]{2})|([a-zA-Z]{2}\d[a-zA-Z] ?\d[a-zA-Z]{2}))$");
            countryPostCodeRegex.Add("JE", @"^JE\d[\dA-Z]?[ ]?\d[ABD-HJLN-UW-Z]{2}$");
            countryPostCodeRegex.Add("GG", @"^GY\d[\dA-Z]?[ ]?\d[ABD-HJLN-UW-Z]{2}$");
            countryPostCodeRegex.Add("IM", @"^IM\d[\dA-Z]?[ ]?\d[ABD-HJLN-UW-Z]{2}$");
            countryPostCodeRegex.Add("US", @"^\d{5}([ \-]\d{4})?$");
            countryPostCodeRegex.Add("CA", @"^[ABCEGHJKLMNPRSTVXY]\d[ABCEGHJ-NPRSTV-Z][ ]?\d[ABCEGHJ-NPRSTV-Z]\d$");
            countryPostCodeRegex.Add("DE", @"^\d{5}$");
            countryPostCodeRegex.Add("JP", @"^\d{3}-\d{4}$");
            countryPostCodeRegex.Add("FR", @"^\d{2}[ ]?\d{3}$");
            countryPostCodeRegex.Add("AU", @"^\d{4}$");
            countryPostCodeRegex.Add("IT", @"^\d{5}$");
            countryPostCodeRegex.Add("CH", @"^\d{4}$");
            countryPostCodeRegex.Add("AT", @"^\d{4}$");
            countryPostCodeRegex.Add("ES", @"^\d{5}$");
            countryPostCodeRegex.Add("NL", @"^\d{4}[ ]?[A-Z]{2}$");
            countryPostCodeRegex.Add("BE", @"^\d{4}$");
            countryPostCodeRegex.Add("DK", @"^\d{4}$");
            countryPostCodeRegex.Add("SE", @"^\d{3}[ ]?\d{2}$");
            countryPostCodeRegex.Add("NO", @"^\d{4}$");
            countryPostCodeRegex.Add("BR", @"^\d{5}[\-]?\d{3}$");
            countryPostCodeRegex.Add("PT", @"^\d{4}([\-]\d{3})?$");
            countryPostCodeRegex.Add("FI", @"^\d{5}$");
            countryPostCodeRegex.Add("AX", @"^22\d{3}$");
            countryPostCodeRegex.Add("KR", @"^\d{3}[\-]\d{3}$");
            countryPostCodeRegex.Add("CN", @"^\d{6}$");
            countryPostCodeRegex.Add("TW", @"^\d{3}(\d{2})?$");
            countryPostCodeRegex.Add("SG", @"^\d{6}$");
            countryPostCodeRegex.Add("DZ", @"^\d{5}$");
            countryPostCodeRegex.Add("AD", @"^AD\d{3}$");
            countryPostCodeRegex.Add("AR", @"^([A-HJ-NP-Z])?\d{4}([A-Z]{3})?$");
            countryPostCodeRegex.Add("AM", @"^(37)?\d{4}$");
            countryPostCodeRegex.Add("AZ", @"^\d{4}$");
            countryPostCodeRegex.Add("BH", @"^((1[0-2]|[2-9])\d{2})?$");
            countryPostCodeRegex.Add("BD", @"^\d{4}$");
            countryPostCodeRegex.Add("BB", @"^(BB\d{5})?$");
            countryPostCodeRegex.Add("BY", @"^\d{6}$");
            countryPostCodeRegex.Add("BM", @"^[A-Z]{2}[ ]?[A-Z0-9]{2}$");
            countryPostCodeRegex.Add("BA", @"^\d{5}$");
            countryPostCodeRegex.Add("IO", @"^BBND 1ZZ$");
            countryPostCodeRegex.Add("BN", @"^[A-Z]{2}[ ]?\d{4}$");
            countryPostCodeRegex.Add("BG", @"^\d{4}$");
            countryPostCodeRegex.Add("KH", @"^\d{5}$");
            countryPostCodeRegex.Add("CV", @"^\d{4}$");
            countryPostCodeRegex.Add("CL", @"^\d{7}$");
            countryPostCodeRegex.Add("CR", @"^\d{4,5}|\d{3}-\d{4}$");
            countryPostCodeRegex.Add("HR", @"^\d{5}$");
            countryPostCodeRegex.Add("CY", @"^\d{4}$");
            countryPostCodeRegex.Add("CZ", @"^\d{3}[ ]?\d{2}$");
            countryPostCodeRegex.Add("DO", @"^\d{5}$");
            countryPostCodeRegex.Add("EC", @"^([A-Z]\d{4}[A-Z]|(?:[A-Z]{2})?\d{6})?$");
            countryPostCodeRegex.Add("EG", @"^\d{5}$");
            countryPostCodeRegex.Add("EE", @"^\d{5}$");
            countryPostCodeRegex.Add("FO", @"^\d{3}$");
            countryPostCodeRegex.Add("GE", @"^\d{4}$");
            countryPostCodeRegex.Add("GR", @"^\d{3}[ ]?\d{2}$");
            countryPostCodeRegex.Add("GL", @"^39\d{2}$");
            countryPostCodeRegex.Add("GT", @"^\d{5}$");
            countryPostCodeRegex.Add("HT", @"^\d{4}$");
            countryPostCodeRegex.Add("HN", @"^(?:\d{5})?$");
            countryPostCodeRegex.Add("HU", @"^\d{4}$");
            countryPostCodeRegex.Add("IS", @"^\d{3}$");
            countryPostCodeRegex.Add("IN", @"^\d{6}$");
            countryPostCodeRegex.Add("ID", @"^\d{5}$");
            countryPostCodeRegex.Add("IL", @"^\d{5}$");
            countryPostCodeRegex.Add("JO", @"^\d{5}$");
            countryPostCodeRegex.Add("KZ", @"^\d{6}$");
            countryPostCodeRegex.Add("KE", @"^\d{5}$");
            countryPostCodeRegex.Add("KW", @"^\d{5}$");
            countryPostCodeRegex.Add("LA", @"^\d{5}$");
            countryPostCodeRegex.Add("LV", @"^\d{4}$");
            countryPostCodeRegex.Add("LB", @"^(\d{4}([ ]?\d{4})?)?$");
            countryPostCodeRegex.Add("LI", @"^(948[5-9])|(949[0-7])$");
            countryPostCodeRegex.Add("LT", @"^\d{5}$");
            countryPostCodeRegex.Add("LU", @"^\d{4}$");
            countryPostCodeRegex.Add("MK", @"^\d{4}$");
            countryPostCodeRegex.Add("MY", @"^\d{5}$");
            countryPostCodeRegex.Add("MV", @"^\d{5}$");
            countryPostCodeRegex.Add("MT", @"^[A-Z]{3}[ ]?\d{2,4}$");
            countryPostCodeRegex.Add("MU", @"^(\d{3}[A-Z]{2}\d{3})?$");
            countryPostCodeRegex.Add("MX", @"^\d{5}$");
            countryPostCodeRegex.Add("MD", @"^\d{4}$");
            countryPostCodeRegex.Add("MC", @"^980\d{2}$");
            countryPostCodeRegex.Add("MA", @"^\d{5}$");
            countryPostCodeRegex.Add("NP", @"^\d{5}$");
            countryPostCodeRegex.Add("NZ", @"^\d{4}$");
            countryPostCodeRegex.Add("NI", @"^((\d{4}-)?\d{3}-\d{3}(-\d{1})?)?$");
            countryPostCodeRegex.Add("NG", @"^(\d{6})?$");
            countryPostCodeRegex.Add("OM", @"^(PC )?\d{3}$");
            countryPostCodeRegex.Add("PK", @"^\d{5}$");
            countryPostCodeRegex.Add("PY", @"^\d{4}$");
            countryPostCodeRegex.Add("PH", @"^\d{4}$");
            countryPostCodeRegex.Add("PL", @"^\d{2}-\d{3}$");
            countryPostCodeRegex.Add("PR", @"^00[679]\d{2}([ \-]\d{4})?$");
            countryPostCodeRegex.Add("RO", @"^\d{6}$");
            countryPostCodeRegex.Add("RU", @"^\d{6}$");
            countryPostCodeRegex.Add("SM", @"^4789\d$");
            countryPostCodeRegex.Add("SA", @"^\d{5}$");
            countryPostCodeRegex.Add("SN", @"^\d{5}$");
            countryPostCodeRegex.Add("SK", @"^\d{3}[ ]?\d{2}$");
            countryPostCodeRegex.Add("SI", @"^\d{4}$");
            countryPostCodeRegex.Add("ZA", @"^\d{4}$");
            countryPostCodeRegex.Add("LK", @"^\d{5}$");
            countryPostCodeRegex.Add("TJ", @"^\d{6}$");
            countryPostCodeRegex.Add("TH", @"^\d{5}$");
            countryPostCodeRegex.Add("TN", @"^\d{4}$");
            countryPostCodeRegex.Add("TR", @"^\d{5}$");
            countryPostCodeRegex.Add("TM", @"^\d{6}$");
            countryPostCodeRegex.Add("UA", @"^\d{5}$");
            countryPostCodeRegex.Add("UY", @"^\d{5}$");
            countryPostCodeRegex.Add("UZ", @"^\d{6}$");
            countryPostCodeRegex.Add("VA", @"^00120$");
            countryPostCodeRegex.Add("VE", @"^\d{4}$");
            countryPostCodeRegex.Add("ZM", @"^\d{5}$");
            countryPostCodeRegex.Add("AS", @"^96799$");
            countryPostCodeRegex.Add("CC", @"^6799$");
            countryPostCodeRegex.Add("CK", @"^\d{4}$");
            countryPostCodeRegex.Add("RS", @"^\d{6}$");
            countryPostCodeRegex.Add("ME", @"^8\d{4}$");
            countryPostCodeRegex.Add("CS", @"^\d{5}$");
            countryPostCodeRegex.Add("YU", @"^\d{5}$");
            countryPostCodeRegex.Add("CX", @"^6798$");
            countryPostCodeRegex.Add("ET", @"^\d{4}$");
            countryPostCodeRegex.Add("FK", @"^FIQQ 1ZZ$");
            countryPostCodeRegex.Add("NF", @"^2899$");
            countryPostCodeRegex.Add("FM", @"^(9694[1-4])([ \-]\d{4})?$");
            countryPostCodeRegex.Add("GF", @"^9[78]3\d{2}$");
            countryPostCodeRegex.Add("GN", @"^\d{3}$");
            countryPostCodeRegex.Add("GP", @"^9[78][01]\d{2}$");
            countryPostCodeRegex.Add("GS", @"^SIQQ 1ZZ$");
            countryPostCodeRegex.Add("GU", @"^969[123]\d([ \-]\d{4})?$");
            countryPostCodeRegex.Add("GW", @"^\d{4}$");
            countryPostCodeRegex.Add("HM", @"^\d{4}$");
            countryPostCodeRegex.Add("IQ", @"^\d{5}$");
            countryPostCodeRegex.Add("KG", @"^\d{6}$");
            countryPostCodeRegex.Add("LR", @"^\d{4}$");
            countryPostCodeRegex.Add("LS", @"^\d{3}$");
            countryPostCodeRegex.Add("MG", @"^\d{3}$");
            countryPostCodeRegex.Add("MH", @"^969[67]\d([ \-]\d{4})?$");
            countryPostCodeRegex.Add("MN", @"^\d{6}$");
            countryPostCodeRegex.Add("MP", @"^9695[012]([ \-]\d{4})?$");
            countryPostCodeRegex.Add("MQ", @"^9[78]2\d{2}$");
            countryPostCodeRegex.Add("NC", @"^988\d{2}$");
            countryPostCodeRegex.Add("NE", @"^\d{4}$");
            countryPostCodeRegex.Add("VI", @"^008(([0-4]\d)|(5[01]))([ \-]\d{4})?$");
            countryPostCodeRegex.Add("PF", @"^987\d{2}$");
            countryPostCodeRegex.Add("PG", @"^\d{3}$");
            countryPostCodeRegex.Add("PM", @"^9[78]5\d{2}$");
            countryPostCodeRegex.Add("PN", @"^PCRN 1ZZ$");
            countryPostCodeRegex.Add("PW", @"^96940$");
            countryPostCodeRegex.Add("RE", @"^9[78]4\d{2}$");
            countryPostCodeRegex.Add("SH", @"^(ASCN|STHL) 1ZZ$");
            countryPostCodeRegex.Add("SJ", @"^\d{4}$");
            countryPostCodeRegex.Add("SO", @"^\d{5}$");
            countryPostCodeRegex.Add("SZ", @"^[HLMS]\d{3}$");
            countryPostCodeRegex.Add("TC", @"^TKCA 1ZZ$");
            countryPostCodeRegex.Add("WF", @"^986\d{2}$");
            countryPostCodeRegex.Add("XK", @"^\d{5}$");
            countryPostCodeRegex.Add("YT", @"^976\d{2}$");

            Boolean ret = false;

            String pattern = null;
            if (countryPostCodeRegex.TryGetValue(twoLetterIsoCountry, out pattern))
            {
                Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);
                ret = regex.IsMatch(postCode);
            }
            return ret;
        }
    }
}
