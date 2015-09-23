using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Lunggo.ApCommon.Payment.Constant;
using Lunggo.ApCommon.Payment.Model;

namespace Lunggo.BackendWeb.Models
{
    public class CheckPaymentControllerModel
    {
        public string RsvNo { get; set; }
        public TransferConfirmationReportStatus Status { get; set; }
    }
}