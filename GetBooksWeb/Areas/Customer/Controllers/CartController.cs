using GetBooks.DataAccess.IRepository;
using GetBooks.Models;
using GetBooks.Models.ViewModels;
using GetBooks.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GetBooksWeb.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize(Roles = Roles.Admin + "," + Roles.Customer)]
    public class CartController : Controller
    {
        IUnitOfWork unitOfWork;
        public CartController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        // API
        public IActionResult AddToCart(int id)
        {
            Book book = unitOfWork.Book.GetFirstOrDefault(x => x.Id == id);
            if (book == null)
            {

                return Json(new {success=false, message="Book not found"});
            }

            string UserId = IdentityUtility.GetUserId(User.Identity);

            Cart cart = unitOfWork.Cart.GetFirstOrDefault(x => x.UserId == UserId && x.Status == CartStatus.InShopping);

            CartItemTemp cartItemTemp;

            if (cart == null)
            {
                cart = CreateNewCart(UserId);
            }

            else
            {
                cartItemTemp = cartItemTemp = unitOfWork.CartItemTemp.GetFirstOrDefault(x => x.CartId == cart.Id && x.BookId == id);
                if (cartItemTemp != null)
                {
                    cartItemTemp.Count++;
                    unitOfWork.Save();

                    return Json(new { success = true, message = "Book has added to cart" });
                }
            }

            cartItemTemp = new CartItemTemp()
            {
                CartId = cart.Id,
                BookId = book.Id,
                Count = 1
            };

            unitOfWork.CartItemTemp.Add(cartItemTemp);
            unitOfWork.Save();

            int itemCount = unitOfWork.CartItemTemp.GetAll(x => x.CartId == cartItemTemp.CartId).ToList().Count;
            HttpContext.Session.SetInt32(Statics.CartItemCount, itemCount);

            return Json(new { success = true, message = "Book has added to cart", newCount = itemCount });
        }

        private Cart CreateNewCart(string UserId)
        {
            Cart cart = new Cart()
            {
                UserId = UserId,
                Status = CartStatus.InShopping,
            };
            unitOfWork.Cart.Add(cart);
            unitOfWork.Save();
            return cart;
        }

        public IActionResult Summary()
        {
            string UserId = IdentityUtility.GetUserId(User.Identity);
            Cart cart = unitOfWork.Cart.GetFirstOrDefault(x => x.UserId == UserId && x.Status == CartStatus.InShopping);

            SummaryVM summaryVM = new SummaryVM();
            summaryVM.CartItems = new List<CartItemTemp>();
            if (cart != null)
            {
                summaryVM.CartId = cart.Id;
                summaryVM.CartItems = unitOfWork.CartItemTemp.GetAll(x => x.CartId == cart.Id, "Book");
            }
            
            return View(summaryVM);
        }

        // API
        public IActionResult IncreaseCount(int id)
        {
            CartItemTemp cartItemTemp = unitOfWork.CartItemTemp.GetFirstOrDefault(x => x.Id == id, "Cart");
            if (cartItemTemp == null)
            {
                return Json(new { success = false, message = "Cart item not found" });
            }

            if (cartItemTemp.Cart.UserId != IdentityUtility.GetUserId(User.Identity))
            {
                return Json(new { sucess = false, message = "Access denied" });
            }

            cartItemTemp.Count++;
            unitOfWork.Save();
            TempData["success"] = "Count increased";
            return Json(new { success = true, message = "Count increased"});
        }

        // API
        public IActionResult DecreaseCount(int id)
        {
            CartItemTemp cartItemTemp = unitOfWork.CartItemTemp.GetFirstOrDefault(x => x.Id == id, "Cart");
            if (cartItemTemp == null)
            {
                return Json(new { success = false, message = "Cart item not found" });
            }

            if (cartItemTemp.Cart.UserId != IdentityUtility.GetUserId(User.Identity))
            {
                return Json(new { sucess = false, message = "Access denied" });
            }

            if (cartItemTemp.Count <= 1)
            {
                unitOfWork.CartItemTemp.Remove(cartItemTemp);
                unitOfWork.Save();

                int itemCount = unitOfWork.CartItemTemp.GetAll(x => x.CartId == cartItemTemp.CartId).ToList().Count;
                HttpContext.Session.SetInt32(Statics.CartItemCount, itemCount);

                TempData["success"] = "Item deleted";
                return Json(new { success = true, message = "Item deleted", newCount = itemCount});
            }
            
            cartItemTemp.Count--;
            unitOfWork.Save();
            TempData["success"] = "Count decreased";
            return Json(new { success = true, message = "Count decreased" });
        }

        // API
        [HttpDelete]
        public IActionResult DeleteItem(int id)
        {
            CartItemTemp cartItemTemp = unitOfWork.CartItemTemp.GetFirstOrDefault(x => x.Id == id, "Cart");
            if (cartItemTemp == null)
            {
                return Json(new { success = false, message = "Cart item not found" });
            }

            if (cartItemTemp.Cart.UserId != IdentityUtility.GetUserId(User.Identity))
            {
                return Json(new { sucess = false, message = "Access denied" });
            }

            unitOfWork.CartItemTemp.Remove(cartItemTemp);
            unitOfWork.Save();

            int itemCount = unitOfWork.CartItemTemp.GetAll(x => x.CartId == cartItemTemp.CartId).ToList().Count;
            HttpContext.Session.SetInt32(Statics.CartItemCount, itemCount);

            TempData["success"] = "Item deleted";
            return Json(new { success = true, message = "Item deleted", newCount = itemCount });
        }

        // API
        public IActionResult ApproveCart(int CartId, string address)
        {
            string UserId = IdentityUtility.GetUserId(User.Identity);

            Cart cart = unitOfWork.Cart.GetFirstOrDefault(x => x.Id == CartId && x.UserId == UserId);
            if (cart == null)
            {
                // Cart is null
                return Json(new { success = false, message = "Cart not found"});
            }

            IEnumerable<CartItemTemp> cartItemTemps = unitOfWork.CartItemTemp.GetAll(x => x.CartId == cart.Id, "Book");
            if (cartItemTemps.Count() <= 0)
            {
                // There is no item in cart
                return Json(new { success = false, message = "There is no item in cart"});
            }

            int TotalPrice = 0;
            foreach (CartItemTemp cartItemTemp in cartItemTemps)
            {
                CartItem cartItem = new CartItem()
                {
                    CartId = cartItemTemp.CartId,
                    BookId = cartItemTemp.BookId,
                    BookTitle = cartItemTemp.Book.Title,
                    ISBN = cartItemTemp.Book.ISBN,
                    Count = cartItemTemp.Count,
                    Price = cartItemTemp.Book.Price,
                };
                TotalPrice += cartItem.Price * cartItem.Count;
                unitOfWork.CartItem.Add(cartItem);
            }
            Order order = new Order()
            {
                CartId = cart.Id,
                UserId = cart.UserId,
                CreatedDate = DateTime.Now,
                Status = OrderStatus.WaitingForApproval,
                TotalPrice = TotalPrice,
                Adress = address
            };

            unitOfWork.CartItemTemp.RemoveRange(cartItemTemps);

            cart.Status = CartStatus.Ordered;

            unitOfWork.Order.Add(order);

            unitOfWork.Save();

            int itemCount = unitOfWork.CartItemTemp.GetAll(x => x.CartId == CartId).ToList().Count;
            HttpContext.Session.SetInt32(Statics.CartItemCount, itemCount);

            return Json(new { success=true, message= "Order is created successfully"});
        }
    }
}
