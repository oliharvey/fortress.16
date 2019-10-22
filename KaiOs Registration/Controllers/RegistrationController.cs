using System;
using System.Collections.Generic;
using System.Web.Mvc;
using KaiOs_Registration.Models.Api.RequestObjects;
using KaiOs_Registration.Models.Api.ResponseObjects;
using KaiOs_Registration.Helpers;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net;
using System.Web.Http;
using KaiOs_Registration.Models;
using System.Globalization;
using System.Web.Configuration;
namespace KaiOs_Registration.Controllers
{
    public class RegistrationController : Controller
    {
        public ActionResult RegistrationPage(string returnUrl)
        {
            return View("RegistrationPage", new Registration());
        }
        public ActionResult RegistrationWizard(string returnUrl)
        {
            return View("RegistrationWizard", new Registration());
        }
        public JsonResult CheckPostcode(string postcode, string Country = "")
        {

            bool valid = false;

            if (Country == null)
            {
                return Json("Country is required", JsonRequestBehavior.AllowGet);
            }

            if (ApiValidator.IsPostCodeValid(postcode.TrimEnd(), (Country.ToString() == "United States" ? "US" : "UK")))
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("Postcode is Invalid", JsonRequestBehavior.AllowGet);
            }
        }

        public async Task<GetOfflineTagResponse> GetCode(RegistrationCode registrationCode)
        {
            GetOfflineTagResponse checkResponse = new GetOfflineTagResponse();
            if (!ModelState.IsValid)
                return null;

            OfflineTagCheckRequest checkRequest = new OfflineTagCheckRequest();
            checkRequest.ReferenceCode = registrationCode.ReferenceCode;
            HttpResponseMessage checkResp = await ApiHelper.CallApi("/api/Registration/GetOfflineTag/", checkRequest);

            if (checkResp != null && checkResp.StatusCode != HttpStatusCode.BadRequest && checkResp.StatusCode != HttpStatusCode.InternalServerError)
            {
                checkResponse = await checkResp.Content.ReadAsAsync<GetOfflineTagResponse>();
                return checkResponse;
            }
            return checkResponse;
        }
        public async Task<ActionResult> ValidateCodeNew(RegistrationCode regCodeRequest)
        {
            Registration reg = new Registration() { CurrentStep = 0, ReferenceCode = regCodeRequest.ReferenceCode };
            ValidateReferenceCodeResponse codeResp = await ValidateCodeUpdate(regCodeRequest);

            if (codeResp == null || (codeResp != null && !codeResp.Result))
                return PartialView("_RegistrationWizardForm", reg);

            reg.CurrentStep = 1;
            return PartialView("_RegistrationWizardForm", reg);
        }
        public async Task<ValidateReferenceCodeResponse> ValidateCodeUpdate(RegistrationCode registrationCode)
        {
            ValidateReferenceCodeResponse resp = new ValidateReferenceCodeResponse() { Result = false };
            if (!ModelState.IsValid)
            {
                resp.Message = "Code is required";
                return resp;
            }
            GetOfflineTagResponse checkResponseGet = await GetCode(registrationCode);
            if (checkResponseGet != null)
            {
                if (checkResponseGet.status == "new")
                {
                    //Validate Device
                    UserRegistrationPreValidationRequest prevalidationRequest = new UserRegistrationPreValidationRequest();
                    prevalidationRequest.DeviceMake = checkResponseGet.DeviceMake;
                    prevalidationRequest.DeviceModel = checkResponseGet.DeviceModel;
                    prevalidationRequest.DeviceCapacityRaw = checkResponseGet.DeviceCapacity;
                    prevalidationRequest.CountryIso = checkResponseGet.CountryISO;
                    prevalidationRequest.validateDevice = false;
                    prevalidationRequest.validateUser = false;
                    HttpResponseMessage preValidationResp = await ApiHelper.CallApi("/api/Registration/UserRegistrationPreValidation/", prevalidationRequest);
                    if (preValidationResp != null && preValidationResp.StatusCode != HttpStatusCode.BadRequest && preValidationResp.StatusCode != HttpStatusCode.InternalServerError)
                    {
                        resp.Result = true;
                    }
                    else
                    {
                        resp.Result = false;
                        resp.Message = await populateApiError(preValidationResp, "ResponseCode");
                    }


                    return resp;
                }
                else
                {
                    resp.Message = "Code has already been activated";
                    addModelStateError("VoucherCode", resp.Message);
                    return resp;
                }
            }
            else
            {
                resp.Message = "Code is required";
                addModelStateError("VoucherCode", resp.Message);
                return resp;
            }
        }
        public async Task<ActionResult> Validate(Registration model)
        {
            if (!ModelState.IsValid && model.CurrentStep != 0)
                return PartialView("_RegistrationWizardForm", model);



            if (model.CurrentStep == 1)
            {


                GetOfflineTagResponse checkResponse = new GetOfflineTagResponse();
                RegistrationCode regCodeRequest = new RegistrationCode();
                regCodeRequest.ReferenceCode = model.ReferenceCode;

                checkResponse = await GetCode(regCodeRequest);
                if (checkResponse == null || (checkResponse != null && checkResponse.status != "new"))
                {
                    model.CurrentStep = 1;
                    return PartialView("_RegistrationWizardForm", model);
                }

                UserRegistrationRequest userRegistrationRequest = new UserRegistrationRequest();
                //User information
                userRegistrationRequest.FirstName = model.FirstName;
                userRegistrationRequest.LastName = model.LastName;
                userRegistrationRequest.EmailAddress = model.EmailAddress;
                userRegistrationRequest.Postcode = model.Postcode;
                userRegistrationRequest.Country = model.Country;
                userRegistrationRequest.CountryIso = (model.Country == "United States" ? "US" : "UK");
                userRegistrationRequest.SourceLanguage = "en-US";

                //Device details
                userRegistrationRequest.DeviceMake = checkResponse.DeviceMake;
                userRegistrationRequest.DeviceModel = checkResponse.DeviceMake;
                userRegistrationRequest.DeviceCapacityRaw = checkResponse.DeviceMake;
                userRegistrationRequest.OsVersion = "1.00";
                userRegistrationRequest.Imei = checkResponse.DeviceSerialNo;

                //Salt/Hash
                string newPassGuid = Guid.NewGuid().ToString().Replace("-", "-");
                string randomPassword = newPassGuid.Substring(6, 3) + newPassGuid.Substring(8, 5) + newPassGuid.Substring(4, 3).ToUpper();
                model.Password = randomPassword;
                SaltedHashPassword saltHashPassword = new SaltedHashPassword(randomPassword);

                userRegistrationRequest.Hash = saltHashPassword.Hash;
                userRegistrationRequest.Salt = saltHashPassword.Salt;

                //Submit user registration request
                HttpResponseMessage userRegistrationResp = await ApiHelper.CallApi("/api/Registration/UserRegistration/", userRegistrationRequest);

                if (userRegistrationResp != null && userRegistrationResp.StatusCode != HttpStatusCode.BadRequest && userRegistrationResp.StatusCode != HttpStatusCode.InternalServerError)
                {
                    UserRegistrationResponse userRegistrationResponse = await userRegistrationResp.Content.ReadAsAsync<UserRegistrationResponse>();
                    model.CurrentStep = 2;
                    model.DeviceID = userRegistrationResponse.DeviceId;
                    model.UserID = userRegistrationResponse.UserId;

                    //sms code request
                    SMSCodeRequest smsCodeRequest = new SMSCodeRequest();
                    smsCodeRequest.DeviceId = userRegistrationResponse.DeviceId;
                    smsCodeRequest.UserId = userRegistrationResponse.UserId;
                    smsCodeRequest.DevicePhoneNumber = model.DevicePhoneNumber;
                    smsCodeRequest.Hash = Encryption.GetTodaysEncryptedToken(userRegistrationResponse.UserId);
                    HttpResponseMessage smsResp = await ApiHelper.CallApi("/api/Registration/SmsCodeRequest/", smsCodeRequest);
                    if (smsResp != null && smsResp.StatusCode != HttpStatusCode.BadRequest && smsResp.StatusCode != HttpStatusCode.InternalServerError)
                    {
                        model.CurrentStep = 3;
                        return PartialView("_RegistrationWizardForm", model);

                    }
                    else
                    {
                        string errorMessage = await populateApiError(smsResp, "DevicePhoneNumber");
                    }
                }
                else if (userRegistrationResp != null)
                {
                    string errorMessage = await populateApiError(userRegistrationResp, "");
                }
            }
            else if (model.CurrentStep == 2)
            {
                SMSCodeRequest smsCodeRequest = new SMSCodeRequest();
                smsCodeRequest.DeviceId = model.DeviceID;
                smsCodeRequest.UserId = model.UserID;
                smsCodeRequest.DevicePhoneNumber = model.DevicePhoneNumber;
                smsCodeRequest.Hash = Encryption.GetTodaysEncryptedToken(model.UserID);
                HttpResponseMessage smsResp = await ApiHelper.CallApi("/api/Registration/SmsCodeRequest/", smsCodeRequest);
                if (smsResp != null && smsResp.StatusCode != HttpStatusCode.BadRequest && smsResp.StatusCode != HttpStatusCode.InternalServerError)
                {
                    model.CurrentStep = 3;
                    return PartialView("_RegistrationWizardForm", model);

                }
                else
                {
                    string errorMessage = await populateApiError(smsResp, "DevicePhoneNumber");
                }
            }
            else if (model.CurrentStep == 3)
            {
                if (model.SmsCode == "")
                {
                    ModelState.AddModelError("SmsVerificationCode", "Please enter a verification code");
                    return PartialView("_RegistrationWizardForm", model);
                }

                SmsCodeVerificationRequest smsCodeVerificationRequest = new SmsCodeVerificationRequest();
                smsCodeVerificationRequest.SmsCode = model.SmsCode;
                smsCodeVerificationRequest.DeviceId = model.DeviceID;
                smsCodeVerificationRequest.UserId = model.UserID;
                smsCodeVerificationRequest.DevicePhoneNumber = model.DevicePhoneNumber;
                smsCodeVerificationRequest.Hash = Encryption.GetTodaysEncryptedToken(model.UserID);
                HttpResponseMessage smsResp = await ApiHelper.CallApi("/api/Registration/SmsCodeVerification/", smsCodeVerificationRequest);
                if (smsResp != null && smsResp.StatusCode != HttpStatusCode.BadRequest && smsResp.StatusCode != HttpStatusCode.InternalServerError)
                {
                    SmsCodeVerificationResponse smsResponse = await smsResp.Content.ReadAsAsync<SmsCodeVerificationResponse>();
                    if (smsResponse != null)
                    {
                        model.CurrentStep = 4;
                        return PartialView("_RegistrationWizardForm", model);
                    }

                }
                else
                {
                    string errorMessage = await populateApiError(smsResp, "DevicePhoneNumber");
                }
            }

            else if (model.CurrentStep == 4)
            {
                if (model.VoucherCode == "")
                {
                    ModelState.AddModelError("VoucherCode", "Please enter a voucher code");
                    return PartialView("_RegistrationWizardForm", model);
                }
                UserRegistrationPreValidationRequest prevalidationRequest = new UserRegistrationPreValidationRequest();
                prevalidationRequest.VoucherCode = model.VoucherCode;
                prevalidationRequest.KaiosPartner = int.Parse(WebConfigurationManager.AppSettings["KaiosPartner" + (model.Country == "United States" ? "US" : "UK")]);
                prevalidationRequest.validateVoucher = true;
                HttpResponseMessage preValidationResp = await ApiHelper.CallApi("/api/Registration/UserRegistrationPreValidation/", prevalidationRequest);
                if (preValidationResp != null && preValidationResp.StatusCode != HttpStatusCode.BadRequest && preValidationResp.StatusCode != HttpStatusCode.InternalServerError)
                {
                    //all good
                    CreateKaiosCoverRequest createKaiosCoverRequest = new CreateKaiosCoverRequest();
                    createKaiosCoverRequest.VoucherCode = model.VoucherCode;
                    createKaiosCoverRequest.KaiosPartner = int.Parse(WebConfigurationManager.AppSettings["KaiosPartner" + (model.Country == "United States" ? "US" : "UK")]);
                    createKaiosCoverRequest.Hash = Encryption.GetTodaysEncryptedToken(model.UserID);
                    createKaiosCoverRequest.DeviceId = model.DeviceID;
                    createKaiosCoverRequest.UserId = model.UserID;
                    HttpResponseMessage createKaiosCoverResp = await ApiHelper.CallApi("/api/Registration/CreateKaiosCover/", createKaiosCoverRequest);
                    if (createKaiosCoverResp != null && createKaiosCoverResp.StatusCode != HttpStatusCode.BadRequest && createKaiosCoverResp.StatusCode != HttpStatusCode.InternalServerError)
                    {
                        //all good


                        //Activate offline tag start
                        ActivateOfflineTagRequest activateOfflineTagRequest = new ActivateOfflineTagRequest() { ReferenceCode = model.ReferenceCode, DeviceId = model.DeviceID };
                        HttpResponseMessage activateOfflineTagResp = await ApiHelper.CallApi("/api/Registration/ActivateOfflineTag/", activateOfflineTagRequest);
                        if (activateOfflineTagResp != null && activateOfflineTagResp.StatusCode != HttpStatusCode.BadRequest && activateOfflineTagResp.StatusCode != HttpStatusCode.InternalServerError)
                        {
                            ActivateOfflineTagResponse activateOfflineTagResponse = await activateOfflineTagResp.Content.ReadAsAsync<ActivateOfflineTagResponse>();
                            if (activateOfflineTagResponse != null && activateOfflineTagResponse.Result)
                            {
                                //Activate offline tag end

                                return PartialView("_RegistrationSubmitted", new RegistrationComplete() { Password = model.Password });
                            }
                            else
                            {
                                ModelState.AddModelError("ResponseCode", "Error Activating Reference Code but the user and device have been created");
                            }
                        }
                        else
                        {
                            string errorMessage = await populateApiError(activateOfflineTagResp, "");
                        }

                    }
                    else
                    {
                        //Voucher not validated
                        string errorMessage = await populateApiError(preValidationResp, "VoucherCode");
                    }


                }
                else
                {
                    //Voucher not validated
                    string errorMessage = await populateApiError(preValidationResp, "VoucherCode");
                }
            }
            return PartialView("_RegistrationWizardForm", model);
        }
        private async Task<string> populateApiError(HttpResponseMessage respMessage, string propertyName)
        {
            string error = "";
            ApiError apiErrorResponse = await respMessage.Content.ReadAsAsync<ApiError>();
            if (apiErrorResponse != null)
            {
                HttpResponseMessage apiErrorDescription = await ApiHelper.CallApi("/api/Registration/GetErrorDescription/", new GetErrorDescriptionRequest() { ErrorCode = apiErrorResponse.ErrorCode });

                if (apiErrorDescription != null && apiErrorDescription.StatusCode != HttpStatusCode.BadRequest && apiErrorDescription.StatusCode != HttpStatusCode.InternalServerError)
                {
                    GetErrorDescriptionResponse userRegistrationResponse = await apiErrorDescription.Content.ReadAsAsync<GetErrorDescriptionResponse>();
                    if (userRegistrationResponse != null)
                    {
                        ModelState.AddModelError(propertyName != "" ? propertyName : "Error", userRegistrationResponse.ErrorDescription);
                        error = userRegistrationResponse.ErrorDescription;
                    }
                }
                else
                {
                    ModelState.AddModelError(propertyName != "" ? propertyName : "Error", "Error has occured: Error code - " + apiErrorResponse.ErrorCode);
                    error = "Error has occured: Error code - " + apiErrorResponse.ErrorCode;
                }
            }
            return error;
        }
        private void addModelStateError(string propertyName, string message)
        {
            ModelState.AddModelError(propertyName != "" ? propertyName : "Error", message);
        }
    }
}