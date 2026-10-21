using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cooperative.Models
{
    public class OrderSummary
    {
        public OrderSummary(string encryptionRequest, string accessCode, string checkoutUrl)
        {
            EncryptionRequest = encryptionRequest;
            AccessCode = accessCode;
            CheckoutUrl = checkoutUrl;
        }

        public string EncryptionRequest { get; set; }
        public string AccessCode { get; set; }
        public string CheckoutUrl { get; set; }
    }
}