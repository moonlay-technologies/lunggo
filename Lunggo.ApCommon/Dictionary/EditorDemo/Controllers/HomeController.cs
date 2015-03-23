using System.Collections.Generic;
using System.Web.Mvc;
using EditorDemo.Models;

namespace EditorDemo.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var initialData = new[] {
                new Gift { Name = "Tall Hat", Price = 39.95 },
                new Gift { Name = "Long Cloak", Price = 120.00 },
            };
            return View(initialData);
        }

        [HttpPost]
        public ActionResult Index(IEnumerable<Gift> gifts)
        {
            return View("Completed", gifts);
        }

        public ViewResult Add()
        {
            return View("GiftEditorRow", new Gift());
        }
    }
}
