using Lunggo.CustomerWeb.Areas.UW100.Logic;
using Lunggo.CustomerWeb.Areas.UW100.Models;
using System.Web.Mvc;

namespace Lunggo.CustomerWeb.Areas.UW100.Controllers
{
    public class UW100SearchController : Controller
    {
        //
        // GET: /UW100/Search/
        [HttpGet]
        public ActionResult SearchForm()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SearchForm(UW100SearchParamViewModel viewModel)
        {
            string RedirectUrl = Url.RouteUrl("UW100RouteSearchHotelResult",
                new
                {
                    CountryArea = viewModel.CountryArea,
                    ProvinceArea = viewModel.ProvinceArea,
                    LargeArea = viewModel.LargeArea,
                    Keyword = viewModel.Keyword,
                    StringChkInDate = viewModel.StringChkInDate,
                    StringChkOutDate = viewModel.StringChkOutDate,
                    GuestCount = viewModel.GuestCount,
                    RoomCount = viewModel.RoomCount
                });
            return RedirectPermanent(RedirectUrl);
        }
        public ActionResult UW100Index(UW100SearchParamViewModel viewModel)
        {
            return new UW100SearchLogic().SearchLogic(viewModel);
            //return View("../UW100Search/UW100Index", new UW100SearchLogic().Search(viewModel));
        }
	}
}