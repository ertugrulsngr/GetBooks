using GetBooks.DataAccess.IRepository;
using GetBooks.Models;
using GetBooks.Utility;
using Microsoft.AspNetCore.Mvc;

namespace GetBooksWeb.ViewComponents
{
    public class ShoppingCartViewComponent : ViewComponent
    {
        private readonly IUnitOfWork unitOfWork;
        public ShoppingCartViewComponent(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            string userId = IdentityUtility.GetUserId(User.Identity);
            if (userId == null)
            {
                HttpContext.Session.Clear();
                return View(0);
            }
            int? itemCountInSession = HttpContext.Session.GetInt32(Statics.CartItemCount);
            if (itemCountInSession == null)
            {
                Cart cart = unitOfWork.Cart.GetFirstOrDefault(x => x.UserId == userId && x.Status == CartStatus.InShopping);
                if (cart == null)
                {
                    HttpContext.Session.SetInt32(Statics.CartItemCount, 0);
                    return View(0);
                }
                else
                {
                    int itemCount = unitOfWork.CartItemTemp.GetAll(x => x.CartId == cart.Id).ToList().Count;
                    HttpContext.Session.SetInt32(Statics.CartItemCount, itemCount);
                    return View(itemCount);
                }
            }
            else
            {
                return View(itemCountInSession);
            }
        }
    }
}
