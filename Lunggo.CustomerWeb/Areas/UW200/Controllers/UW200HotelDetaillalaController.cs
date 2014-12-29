using System.Threading.Tasks;
using Lunggo.CustomerWeb.Areas.UW100.Models;
using Lunggo.CustomerWeb.Areas.UW200.Logic;
using Lunggo.CustomerWeb.Areas.UW200.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Lunggo.CustomerWeb.Areas.UW200.Controllers
{
    public class UW200HotelDetaillalaController : Controller
    {
        //
        // GET: /UW200/UW200HotelDetail/
        public async Task<ActionResult> UW200Index(UW100SearchParamViewModel SearchViewModel)
        {
            var ViewModel = new UW200HotelDetailViewModel();
            ViewModel = await new UW200GetHotelDetail().UW200GetHotelDetailLogic(SearchViewModel);
            ViewModel.SearchParam = SearchViewModel;
            return View(ViewModel);
        }
	}
}