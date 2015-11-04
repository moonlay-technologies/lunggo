using Lunggo.CustomerWeb.Areas.UW100.Logic;
using Lunggo.CustomerWeb.Areas.UW100.Models;
using System.Web.Mvc;

namespace Lunggo.CustomerWeb.Areas.UW100.Controllers
{
    public class UW100RouterController : Controller
    {
        //
        // GET: /UW100/UW100Router/
        public ActionResult UW100Index(UW100SearchParamViewModel ViewModel)
        {
            new UW100SearchLogic().GetAllParameter(ViewModel);
            return new UW100SearchController().UW100Index(ViewModel);
        }
	}
}