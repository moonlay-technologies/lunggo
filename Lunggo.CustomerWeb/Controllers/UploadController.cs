using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Lunggo.ApCommon.Payment;
using Lunggo.Framework.SharedModel;
using RestSharp.Extensions;
using FileInfo = Lunggo.Framework.SharedModel.FileInfo;

namespace Lunggo.CustomerWeb.Controllers
{
    public class UploadController : Controller
    {   
        [HttpPost]
        public ActionResult Receipt(string rsvNo, HttpPostedFileBase file)
        {
            if (file.ContentLength > 0)
            {
                PaymentService.GetInstance().SubmitTransferReceipt(rsvNo, new FileInfo
                {
                    FileData = file.InputStream.ReadAsBytes(),
                    ContentType = file.ContentType,
                    FileName = file.FileName
                });
            }
            return null;
        }
    }
}