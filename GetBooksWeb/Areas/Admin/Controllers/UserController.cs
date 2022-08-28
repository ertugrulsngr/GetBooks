using GetBooks.DataAccess.Data;
using GetBooks.Models.ViewModels;
using GetBooks.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GetBooksWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles=Roles.Admin)]
    public class UserController : Controller
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly SignInManager<IdentityUser> signInManager;

        public UserController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, SignInManager<IdentityUser> signInManager)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.signInManager = signInManager;
        }

        public IActionResult Index()
        {
            List<UserVM> users = new List<UserVM>();
            foreach (var user in userManager.Users.ToList())
            {
                users.Add(new UserVM()
                {
                    Id = user.Id,
                    Email = user.Email,
                    Role = userManager.GetRolesAsync(user).GetAwaiter().GetResult().ToArray()[0],
                });
            }
            return View(users);
        }

        public IActionResult Edit(string? id)
        {
            IdentityUser user = userManager.Users.FirstOrDefault(u => u.Id == id);
            if (user != null)
            {
                UserVM uservm = new UserVM()
                {
                    Id = user.Id,
                    Email=user.Email,
                    Role = userManager.GetRolesAsync(user).GetAwaiter().GetResult().ToArray()[0],
                };
                UserEditVM userEditVM = new UserEditVM()
                {
                    UserVM = uservm,
                    RoleList = roleManager.Roles.Select(x => x.Name).Select(i => new SelectListItem
                    {
                        Text = i,
                        Value = i
                    })
                };
                return View(userEditVM);
            }
            return NotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(UserEditVM uevm)
        {
            
            if (ModelState.IsValid)
            {
                IdentityUser user = userManager.FindByIdAsync(uevm.UserVM.Id).GetAwaiter().GetResult();
                if (user != null)
                {
                    if (uevm.UserVM.Email != user.Email)
                    {
                        userManager.SetEmailAsync(user, uevm.UserVM.Email).GetAwaiter().GetResult();
                    }

                    IList<string> userRoles = userManager.GetRolesAsync(user).GetAwaiter().GetResult();
                    
                    if (!userRoles.Contains(uevm.UserVM.Role))
                    {
                        userManager.RemoveFromRolesAsync(user, userRoles).GetAwaiter().GetResult();
                        userManager.AddToRoleAsync(user, uevm.UserVM.Role).GetAwaiter().GetResult();
                    }

                    TempData["success"] = "User updated succesfully";
                    return RedirectToAction(nameof(Index), new {id = user.Id});
                }
                return NotFound();
            }
            uevm.NewPassword = null;
            return View(uevm);
        }
        
        [HttpDelete]
        public IActionResult Delete(string? userId)
        {
            IdentityUser user = userManager.FindByIdAsync(userId).GetAwaiter().GetResult();
            if (user != null)
            {
                userManager.DeleteAsync(user).GetAwaiter().GetResult();
                return RedirectToAction(nameof(Index));
            }
            return NotFound();
        }
    }
}
