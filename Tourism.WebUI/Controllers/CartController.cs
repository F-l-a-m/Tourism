using System.Linq;
using System.Web.Mvc;
using Tourism.Domain.Abstract;
using Tourism.Domain.Entities;
using Tourism.WebUI.Models;

namespace Tourism.WebUI.Controllers
{
    public class CartController : Controller
    {
        private ITourRepository repository;

        public CartController(ITourRepository repo)
        {
            repository = repo;
        }

        public ViewResult Index(string returnUrl)
        {
            return View(new CartIndexViewModel
            {
                Cart = GetCart(),
                ReturnUrl = returnUrl
            });
        }

        public RedirectToRouteResult AddToCart(int tourId, string returnUrl)
        {
            Tour tour = repository.Tours
            .FirstOrDefault(p => p.TourID == tourId);
            if (tour != null)
            {
                GetCart().AddItem(tour, 1);
            }
            return RedirectToAction("Index", new { returnUrl });
        }

        public RedirectToRouteResult RemoveFromCart(int tourId, string returnUrl)
        {
            Tour tour = repository.Tours
            .FirstOrDefault(p => p.TourID == tourId);
            if (tour != null)
            {
                GetCart().RemoveLine(tour);
            }
            return RedirectToAction("Index", new { returnUrl });
        }

        private Cart GetCart()
        {
            Cart cart = (Cart)Session["Cart"];
            if (cart == null)
            {
                cart = new Cart();
                Session["Cart"] = cart;
            }
            return cart;
        }
    }
}