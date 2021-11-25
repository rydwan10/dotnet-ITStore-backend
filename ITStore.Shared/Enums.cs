using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITStore.Shared
{
    public class Enums
    {
        public enum EnumStatusCodes
        {
            [Display(Name ="Continue")]
            Continue = 100,

            [Display(Name = "Ok")]
            Ok = 200,

            [Display(Name = "Created")]
            Created = 201,

            [Display(Name = "Accepted")]
            Accepted = 202,

            [Display(Name = "Bad Request")]
            BadRequest = 400,

            [Display(Name = "Unautorized")]
            Unauthorized = 401,

            [Display(Name = "Forbidden")]
            Forbidden = 403,

            [Display(Name = "Not Found")]
            NotFound = 404,

            [Display(Name = "Conflict")]
            Conflict = 409,

            [Display(Name = "Unprocessable Entity")]
            UnprocessableEntity = 422,

            [Display(Name = "Internal Server Error")]
            InternalServerError = 500,
        }

        public enum EnumProviders
        {
            [Display(Name = "Direct Transfer")]
            DirectTransfer = 1,

            [Display(Name = "Virtual Account")]
            VirtualAccount = 2,

            [Display(Name = "E-Wallet")]
            EWallet = 3,

            [Display(Name = "Mini Market Merchant")]
            MiniMarketMerchant = 4,

            [Display(Name = "E-Commerce Merchant")]
            ECommerceMerchant = 5,

            [Display(Name = "Payment Gateaway")]
            PaymentGetaway = 6
        }

        public enum EnumPaymentStatuses
        {
            [Display(Name = "Created")]
            Created = 0,

            [Display(Name = "Pending")]
            Pending = 1,

            [Display(Name = "Processed")]
            Processed = 2,

            [Display(Name = "Delivering")]
            Delivering = 3,

            [Display(Name = "Delivered")]
            Delivered = 4,

            [Display(Name = "Completed")]
            Completed = 5,

            [Display(Name = "Denied")]
            Denied = 6,

            [Display(Name = "Failed")]
            Failed = 7,

            [Display(Name = "Expired")]
            Expired = 8,

            [Display(Name = "Refunded")]
            Refunded = 9,

            [Display(Name = "Reversed")]
            Reversed = 10,

            [Display(Name = "Canceled Reversal")]
            CanceledReversal = 11,

            [Display(Name = "Voided")]
            Voided = 12,
        }
    }
}
