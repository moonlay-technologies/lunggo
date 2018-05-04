﻿using System.Collections.Generic;
using Newtonsoft.Json;

namespace Lunggo.ApCommon.Payment.Processor
{
    internal partial class PaymentProcessorService
    {
        internal class BankTransfer
        {
            [JsonProperty("bank")] public string Bank { get; set; }
        }
    }
}