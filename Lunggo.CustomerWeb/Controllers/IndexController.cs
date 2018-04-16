using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Lunggo.Framework.BlobStorage;
using Lunggo.Framework.Filter;
using Lunggo.Framework.SharedModel;

namespace Lunggo.CustomerWeb.Controllers
{
    public class IndexController : Controller
    {
        public ActionResult TeaserPage()
        {
            return View();
        }

        public ActionResult TeaserPageSubscribe(string input)
        {
            var a = new BlobWriteDto
            {
                FileBlobModel = new FileBlobModel
                {
                    Container = "LandingPageContainer",
                    FileInfo = new FileInfo
                    {
                        ContentType = "",
                        FileData = Encoding.UTF8.GetBytes(input),
                        FileName = input
                    }
                },
                SaveMethod = SaveMethod.Force
            };
            BlobStorageService.GetInstance().WriteFileToBlob(a);
            return new EmptyResult();
        }

        // GET: Index
        //[DeviceDetectionFilter]
        public ActionResult Index(string destination)
        {
            if (destination == null)
                return View();
            else
            {
                ViewBag.Destination = destination;
                return View();
            }
        }

        //[DeviceDetectionFilter]
        [Route("tiket-pesawat")]
        public ActionResult IndexFlight(string destination)
        {
            ViewBag.IndexType = "flight";
            if (destination == null)
                return View("Index");
            else
            {
                ViewBag.Destination = destination;
                return View("Index");
            }
        }

        //[DeviceDetectionFilter]
        [Route("hotel")]
        public ActionResult IndexHotel(string destination)
        {
            ViewBag.IndexType = "hotel";
            if (destination == null)
                return View("Index");
            else
            {
                ViewBag.Destination = destination;
                return View("Index");
            }
        }
    }
}