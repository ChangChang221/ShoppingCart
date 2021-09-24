
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ShoppingCart.Infrastructure;
using ShoppingCart.Models;
using Microsoft.AspNetCore.Mvc;

namespace ShoppingCart.Controllers
{
    public class CartController : Controller
    {
        private readonly ShoppingCartContext context;

        public CartController(ShoppingCartContext context)
        {
            this.context = context;
        }

        // GET /cart
        public IActionResult Index1()
        {

            List<CartItem> cart = HttpContext.Session.GetJson<List<CartItem>>("Cart") ?? new List<CartItem>();//?? new List<CartItem>() kiểm tra giỏ hàng tạo chưa

            CartViewModel cartVM = new CartViewModel
            {
                CartItems = cart,
                GrandTotal = cart.Sum(x => x.Price * x.Quantity)
            };

               return View(cartVM);
           //return View();

        }
           
        

        // GET /cart/add/5
        public async Task<IActionResult> Add(int id)
        {
            Product product = await context.Products.FindAsync(id);

            List<CartItem> cart = HttpContext.Session.GetJson<List<CartItem>>("Cart") ?? new List<CartItem>();

            CartItem cartItem = cart.Where(x => x.ProductId == id).FirstOrDefault();

            if (cartItem == null)
            {
                cart.Add(new CartItem(product));
            }
            else
            {
                cartItem.Quantity += 1;
            }

            HttpContext.Session.SetJson("Cart", cart);

            if (HttpContext.Request.Headers["X-Requested-With"] != "XMLHttpRequest")
            {
                /*List<CartItem> cart1 = HttpContext.Session.GetJson<List<CartItem>>("Cart") ?? new List<CartItem>();

                CartViewModel cartVM = new CartViewModel
                {
                    CartItems = cart1,
                    GrandTotal = cart1.Sum(x => x.Price * x.Quantity)
                };

                return View("Index",cartVM);*/
                   return RedirectToAction("Index1","Cart");
            }


            return ViewComponent("SmallCart");
        }
        
        // GET /cart/decrease/5
        public IActionResult Decrease(int id)
        {
            List<CartItem> cart = HttpContext.Session.GetJson<List<CartItem>>("Cart");

            CartItem cartItem = cart.Where(x => x.ProductId == id).FirstOrDefault();

            if (cartItem.Quantity > 1)
            {
                --cartItem.Quantity;
            }
            else
            {
                cart.RemoveAll(x => x.ProductId == id);
            }

            if (cart.Count == 0)
            {
                HttpContext.Session.Remove("Cart"); //xóa phiên thẻ đó
            }
            else
            {
                HttpContext.Session.SetJson("Cart", cart);
            }

            return RedirectToAction("Index1");
        }
        
        // GET /cart/remove/5
        public IActionResult Remove(int id)
        {
            List<CartItem> cart = HttpContext.Session.GetJson<List<CartItem>>("Cart");

            cart.RemoveAll(x => x.ProductId == id);

            if (cart.Count == 0)
            {
                HttpContext.Session.Remove("Cart");
            }
            else
            {
                HttpContext.Session.SetJson("Cart", cart);
            }

            return RedirectToAction("Index1");
        }

        // GET /cart/clear
        public IActionResult Clear()
        {
            HttpContext.Session.Remove("Cart");

            //return RedirectToAction("Page", "Pages");
            //return Redirect("/");
            if (HttpContext.Request.Headers["X-Requested-With"] != "XMLHttpRequest")
                return Redirect(Request.Headers["Referer"].ToString());

            return Ok();
        }
    }
}
