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
        private IOrderProcessor orderProcessor;

        public CartController(ITourRepository repo, IOrderProcessor proc)
        {
            repository = repo;
            orderProcessor = proc;
        }

        public CartController(ITourRepository repo)
        {
            repository = repo;
        }

        public ViewResult Index(Cart cart, string returnUrl)
        {
            return View(new CartIndexViewModel
            {
                ReturnUrl = returnUrl,
                Cart = cart
            });
        }

        public RedirectToRouteResult AddToCart(Cart cart, int tourId, string returnUrl)
        {
            Tour tour = repository.Tours
            .FirstOrDefault(p => p.TourID == tourId);
            if (tour != null)
            {
                cart.AddItem(tour, 1);
            }
            return RedirectToAction("Index", new { returnUrl });
        }

        public RedirectToRouteResult RemoveFromCart(Cart cart, int tourId, string returnUrl)
        {
            Tour tour = repository.Tours
            .FirstOrDefault(p => p.TourID == tourId);
            if (tour != null)
            {
                cart.RemoveLine(tour);
            }
            return RedirectToAction("Index", new { returnUrl });
        }

        public PartialViewResult Summary(Cart cart)
        {
            return PartialView(cart);
        }

        public ViewResult Checkout()
        {
            return View(new ShippingDetails());
        }

        [HttpPost]
        public ViewResult Checkout(Cart cart, ShippingDetails shippingDetails)
        {
            if (cart.Lines.Count() == 0)
            {
                ModelState.AddModelError("", "Sorry, your cart is empty!");
            }

            if (ModelState.IsValid)
            {
                orderProcessor.ProcessOrder(cart, shippingDetails);
                cart.Clear();
                return View("Completed");
            }
            else
            {
                return View(shippingDetails);
            }
        }
    }
}