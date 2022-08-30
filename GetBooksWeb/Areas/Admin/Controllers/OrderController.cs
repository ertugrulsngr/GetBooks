using GetBooks.DataAccess.IRepository;
using GetBooks.Models;
using GetBooks.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace GetBooksWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = Roles.Admin)]
    public class OrderController : Controller
    {
        private readonly IUnitOfWork unitOfWork;
        public OrderController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult GetAll()
        {
            IEnumerable<Order> orders = Enumerable.Empty<Order>();
            orders = unitOfWork.Order.GetAll(includeProperties: "User");
            return Json(new { data = orders });
        }

        public IActionResult Get(int? id)
        {
            Order order = unitOfWork.Order.GetFirstOrDefault(x => x.Id == id);
            if (order == null)
            {
                return Json(new { found = false });
            }
            var items = unitOfWork.CartItem.GetAll(x => x.CartId == order.CartId);
            return Json(new { found = true, data = items });
        }

        public IActionResult ChangeStatus(int? id, int status)
        {
            Order order = unitOfWork.Order.GetFirstOrDefault(x => x.Id == id);
            if (order == null)
            {
                return Json(new { success = false, message = "Order not found" });
            }
            order.Status = (OrderStatus)status;
            unitOfWork.Save();
            return Json(new { success = true, message = "Order status has changed" });
        }
    }
}
