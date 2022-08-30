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
    public class OrderController : Controller
    {
        IUnitOfWork unitOfWork;
        public OrderController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            string UserId = IdentityUtility.GetUserId(User.Identity);
            IEnumerable<Order> order = unitOfWork.Order.GetAll(x => x.UserId == UserId);
            return View(order);
        }

        public IActionResult Details(int? id)
        {
            string userId = IdentityUtility.GetUserId(User.Identity);
            Order order = unitOfWork.Order.GetFirstOrDefault(x => x.Id == id && x.UserId == userId);
            if (order == null)
            {
                TempData["error"] = "Order not found";
                return RedirectToAction(nameof(Index));
            }
            Cart cart = unitOfWork.Cart.GetFirstOrDefault(x => x.Id == order.CartId);
            if (cart == null)
            {
                TempData["error"] = "Order of cart not found";
                return RedirectToAction(nameof(Index));
            }
            OrderVM orderVM = new OrderVM()
            {
                order = order,
                cartItems = unitOfWork.CartItem.GetAll(x => x.CartId == cart.Id)
            };
            return View(orderVM);
        }
    }
}
