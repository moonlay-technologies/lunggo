﻿using Lunggo.ApCommon.Campaign.Constant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.ApCommon.Campaign.Model
{
    public class VoucherResponse
    {
        public String VoucherCode { get; set; }
        public String Email { get; set; }
        public decimal OriginalPrice { get; set; }
        public decimal DiscountedPrice { get; set; }
        public decimal TotalDiscount { get; set; }
        private VoucherValidationStatusType _updateStatus = VoucherValidationStatusType.Undefined;
        public VoucherValidationStatusType UpdateStatus 
        {
            get { return _updateStatus; }
            set { _updateStatus = value; }
        }
    }
}
