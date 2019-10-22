using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace KaiOs_Registration.Models
{
    public class Enumerations
    {
        public enum Countries
        {
            [Description("United States")]
            UnitedStates,
            [Description("United Kingdom")]
            UnitedKingdom
        }
        public enum ErrorValue
        {
            [Description("User Exists")]
            UserExists,
            [Description("Device Exists")]
            DeviceExists,
            [Description("Error validating voucher")]
            VoucherValidationError,
            [Description("Error activating voucher")]
            VoucherActivationError,
            [Description("Device does not exist")]
            DeviceDoesNotExist,
            [Description("User does not exist")]
            UserDoesNotExist,
            [Description("Voucher does not exist")]
            VoucherDoesNotExist,
            [Description("Sms code invalid")]
            SmsCodeInvalid,
            [Description("Error validating Email code")]
            EmailCodeInvalid,
            [Description("Error adding phone number to device")]
            ErrorAddingPhoneNumberToDevice,
            [Description("Error creating user")]
            ErrorCreatingUser,
            [Description("Error creating device")]
            ErrorCreatingDevice,
            [Description("Login details incorrect")]
            LoginDetailsIncorrect,
            [Description("Device not linked to user")]
            DeviceUserLinkError,
            [Description("Error logging data")]
            ErrorLoggingData,
            [Description("Invalid Request")]
            InvalidRequest,
            [Description("Country Iso is required")]
            CountryIsoIsRequired,
            [Description("Postcode is invalid")]
            PostcodeIsInvalid,
            [Description("Error updating location")]
            LocationUpdateError,
            [Description("Error updating push notification token")]
            PushNotificationTokenUpdateError,
            [Description("Error sending Sms")]
            SmsSendError,
            [Description("Error sending Verification")]
            VerificationSendError,
            [Description("Error sending push notification")]
            SendPushNotificationError,
            [Description("No push notification token for device")]
            NoPushNotificationTokenForDevice,
            [Description("Error getting last known location")]
            LocationLastKnownError,
            [Description("Invalid voucher code")]
            InvalidVoucherCode,
            [Description("Device Model Raw does not exist")]
            DeviceModelRawDoesNotExist,
            [Description("Device Capacity Raw is invalid")]
            DeviceCapacityRawIsInvalid,
            [Description("Device Level not found")]
            DeviceLevelNotFound,
            [Description("Cannot transfer device has active cover")]
            CannotTransferDeviceHasActiveCover,
            [Description("No unknown device exists for missing devices")]
            NoUnknownDevice,
            [Description("User and Device countries do not match")]
            UserDeviceCountryMismatch,
            [Description("Cannot transfer to an unknown device")]
            CannotTransferToAnUnknownDevice,
            [Description("Post Hash Invalid")]
            PostHashInvalid,
            [Description("No Charge Token specified")]
            NoChargeTokenSpecified,
            [Description("Stripe Exception occurred")]
            StripeExceptionOccurred,
            [Description("Stripe Charge response is null")]
            StripeChargeResponseNull,
            [Description("Stripe Webhook endpoint error")]
            StripeWebhookEndpointError,
            [Description("Payment is not valid for this user and device")]
            PaymentNotValidForUserDevice,
            [Description("Payment does not exist")]
            PaymentDoesNotExist,
            [Description("Pricing model does not exist")]
            PricingModelDoesNotExist,
            [Description("Stripe Charge Failed")]
            StripeChargeFailed,
            [Description("Payment Status is incorrect")]
            StripePaymentStatus,
            [Description("Billing price does not matched supplied pricing model")]
            BillModelMismatch,
            [Description("No payments/voucher supplied")]
            NoPaymentsVouchers,
            [Description("An active subscription already exists against this device")]
            ActiveSubscriptionError,
            [Description("Stripe Error - Could not create the customer on stripe")]
            StripeCustomerCreationError,
            [Description("Stripe Error - Could not create the customer on stripe: Token Invalid")]
            StripeCustomerTokenCreationError,
            [Description("Stripe Error - Could not create the plan on stripe")]
            StripePlanCreationError,
            [Description("Stripe Error - Could not create the subscription on stripe")]
            StripeSubscriptionCreationError,
            [Description("Stripe Error - Could not update stripe customer token")]
            StripeUpdateTokenError,
            [Description("Ensure that no other vouchers/payments are used when validating/activating recurring voucher/payment")]
            MultipleSubscriptionVouchers,
            [Description("Subsidised subscription vouchers should be activated through cover activation")]
            SubsidisedVoucherActviationError,
            [Description("Non subscription passed")]
            NonSubscriptionGenerateTransactionVoucherActviationError,
            [Description("Paid for subscription vouchers should be activated through generate transaction")]
            PaidForVoucherActivationError,
            [Description("Billing option required for subscription vouchers")]
            SubscriptionVouchersBilingOptionError,

            [Description("Your card was declined.")]
            StriepCardDeclined,
            [Description("Your card's security code is incorrect.")]
            StriepCardCodeIncorrect,
            [Description("Your card has expired.")]
            StriepCardExpired,
            [Description("An error occurred while processing your card. Try again in a little bit.")]
            StriepCardProcessError,
            [Description("Your card number is incorrect.")]
            StriepCardNumberIncorrect,
            [Description("Active Cover With Different Partner")]
            ActiveCoverWithDifferentPartner,
            [Description("Device has no active cover")]
            DeviceHasNoActiveCover,
            [Description("Invalid Partner")]
            InvalidPartner,
            [Description("Invalid Business Id")]
            InvalidBusinessId,
            [Description("Invalid Tier")]
            InvalidTier,
            [Description("Invalid Voucher family")]
            InvalidFamily,
            [Description("Number of Days not set")]
            InvalidNumberOfDays,
            [Description("Please select a device to freedom from")]
            FreedomDeviceNotSupplied,
            [Description("Fortress freedom device not found")]
            FreedomDeviceNotFound,
            [Description("No contacts found")]
            NoContactsFound,
            [Description("Multiple Email Addresses Exists")]
            MultipleEmailAddressAlreadyExists,
            [Description("Device details don't match the voucher")]
            DeviceVoucherMismatch,
            [Description("Access is blocked from this ip")]
            BusinessPortalIPError,
            [Description("PartnerID is invalid")]
            BusinessPortalInvalidPartnerID,
            [Description("Level is invalid")]
            BusinessPortalInvalidLevelID,
            [Description("Tier is invalid")]
            BusinessPortalInvalidTierID,
            [Description("Device does not exist")]
            BusinessPortalDeviceDoesNotExist,
            [Description("No matching pricing model id")]
            BusinessPortalNoMatchingPricingModel,
            [Description("Email address exists")]
            BusinessPortalEmailAddressExists,
            [Description("Incomplete device details")]
            BusinessPortalIncompleteDeviceDetails,
            [Description("Incomplete voucher details")]
            BusinessPortalIncompleteBillingVoucherDetails,
            [Description("Invalid voucher type")]
            BusinessPortalInvalidVoucherType,
            [Description("Invalid SKU")]
            BusinessPortalInvalidSKU,
            [Description("Invalid Pricing Model")]
            BusinessPortalInvalidPricingModel,
            [Description("Error Creating Voucher")]
            ErrorCreatingVoucher,
            [Description("There is insufficient invoiced capacity for the requested quantity.")]
            AdvanceInvoiceSKUInsufficientCapacity,
            [Description("Email account already verified")]
            EmailAlreadyVerified
        }
    }
}