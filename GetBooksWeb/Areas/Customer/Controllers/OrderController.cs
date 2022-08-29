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
    }
}
