using System.Linq;
using System.Web.Mvc;
using SportsStore.Domain.Abstract;
using SportsStore.Domain.Entities;
using SportsStore.WebUI.Models;

namespace SportsStore.WebUI.Controllers
{

    public class CartController : Controller
    {
        private IProductsRepository repository;

        public CartController(IProductsRepository repo)
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

        public RedirectToRouteResult AddToCart(int productId, string returnUrl)
        {
            var product = repository.Products
                .FirstOrDefault(p => p.ProductID == productId);

            if (product != null)
            {
                GetCart().AddItem(product, 1);
            }
            return RedirectToAction("Index", new { returnUrl });
        }

        public RedirectToRouteResult RemoveFromCart(int productId, string returnUrl)
        {
            var product = repository.Products
                .FirstOrDefault(p => p.ProductID == productId);

            if (product != null)
            {
                GetCart().RemoveLine(product);
            }
            return RedirectToAction("Index", new { returnUrl });
        }

        private Cart GetCart()
        {

            var cart = (Cart)Session["Cart"];
            if (cart == null)
            {
                cart = new Cart();
                Session["Cart"] = cart;
            }
            return cart;
        }
    }
}