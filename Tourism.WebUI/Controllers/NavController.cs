using System.Collections.Generic;
using System.Web.Mvc;
using Tourism.Domain.Abstract;
using System.Linq;

namespace Tourism.WebUI.Controllers
{
    public class NavController : Controller
    {
        private ITourRepository repository;
        public NavController(ITourRepository repo)
        {
            repository = repo;
        }
        public PartialViewResult Menu(string category = null,
        bool horizontalLayout = false)
        {
            ViewBag.SelectedCategory = category;
            IEnumerable<string> categories = repository.Tours
            .Select(x => x.Category)
            .Distinct()
            .OrderBy(x => x);
            string viewName = horizontalLayout ? "MenuHorizontal" : "Menu";
            return PartialView(viewName, categories);
        }
    }
}