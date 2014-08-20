using Lunggo.CustomerWeb.Areas.UW100.Models;
using Lunggo.CustomerWeb.Areas.UW200.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Lunggo.CustomerWeb.Areas.UW200.Controllers
{
    public class UW200HotelDetailController : Controller
    {
        //
        // GET: /UW200/UW200HotelDetail/
        public ActionResult UW200Index(UW100SearchParamViewModel SearchViewModel)
        {
            UW200HotelDetailViewModel ViewModel = new UW200HotelDetailViewModel();
            ViewModel.SearchParam = SearchViewModel;
            return View(ViewModel);
        }
	}
}