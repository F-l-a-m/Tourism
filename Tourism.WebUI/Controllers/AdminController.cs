using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tourism.Domain.Abstract;
using Tourism.Domain.Entities;

namespace Tourism.WebUI.Controllers
{

    [Authorize]
    public class AdminController : Controller
    {
        private ITourRepository repository;

        public AdminController(ITourRepository repo)
        {
            repository = repo;
        }

        public ViewResult Index()
        {
            return View(repository.Tours);
        }

        public ViewResult Edit(int tourId)
        {
            Tour tour = repository.Tours
            .FirstOrDefault(p => p.TourID == tourId);
            return View(tour);
        }

        [HttpPost]
        public ActionResult Edit(Tour tour, HttpPostedFileBase image = null)
        {
            if (ModelState.IsValid)
            {
                if (image != null)
                {
                    tour.ImageMimeType = image.ContentType;
                    tour.ImageData = new byte[image.ContentLength];
                    image.InputStream.Read(tour.ImageData, 0, image.ContentLength);
                }
                repository.SaveTour(tour);
                TempData["message"] = string.Format("{0} has been saved", tour.Name);
                return RedirectToAction("Index");
            }
            else
            {
                // there is something wrong with the data values
                return View(tour);
            }
        }

        public ViewResult Create()
        {
            return View("Edit", new Tour());
        }

        [HttpPost]
        public ActionResult Delete(int tourId)
        {
            Tour deletedTour = repository.DeleteTour(tourId);

            if (deletedTour != null)
            {
                TempData["message"] = string.Format("{0} was deleted", deletedTour.Name);
            }
            return RedirectToAction("Index");
        }
    }
}