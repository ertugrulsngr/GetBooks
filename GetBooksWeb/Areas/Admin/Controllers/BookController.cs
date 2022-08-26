using GetBooks.DataAccess.IRepository;
using GetBooks.Models;
using Microsoft.AspNetCore.Mvc;

namespace GetBooksWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BookController : Controller
    {
        private IUnitOfWork unitOfWork { get; set; }

        private readonly IWebHostEnvironment webHostEnvironment;
        public BookController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            this.unitOfWork = unitOfWork;
            this.webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            IEnumerable<Book> books = unitOfWork.Book.GetAll();
            return View(books);
        }


        public IActionResult Upsert(int? id)
        {
            Book book;

            if (id == null || id == 0)
            {
                book = new Book();
                return View(book);
            }

            book = unitOfWork.Book.GetFirstOrDefault(u=> u.Id == id);

            if (book == null)
            {
                return NotFound();
            }
            else
            {
                return View(book);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Book book, IFormFile? image)
        {
            if (ModelState.IsValid)
            {
                if (image != null)
                {
                    book.ImageUrl = UploadImage(book.ImageUrl, image);
                }

                if (book.Id == 0)
                {
                    if (image == null)
                    {
                        ModelState.AddModelError("ImageUrl", "Please provide an image");
                        return View(book);
                    }
                    unitOfWork.Book.Add(book);
                    TempData["success"] = "Book created successfully";

                }
                else
                {
                    unitOfWork.Book.Update(book);
                    TempData["success"] = "Book updated successfully";

                }
                unitOfWork.Save();
                return RedirectToAction(nameof(Upsert), new { id = book.Id });
            }

            return View(book);
        }
    
        private string UploadImage(string oldImageUrL, IFormFile image)
        {
            string wwwRootPath = webHostEnvironment.WebRootPath;
            string bookImages= Path.Combine(wwwRootPath, @"uploads\images\book");
            string extension = Path.GetExtension(image.FileName);

            // Delete old image if it's exist
            if (oldImageUrL != null)
            {
                var oldImagePath = Path.Combine(wwwRootPath, oldImageUrL.TrimStart('\\'));
                if (System.IO.File.Exists(oldImagePath))
                {
                    System.IO.File.Delete(oldImagePath);
                }
            }

            // Upload new image
            string newFileName = Guid.NewGuid().ToString();
            using (FileStream filestream = new FileStream(Path.Combine(bookImages, newFileName+extension), FileMode.Create))
            {
                image.CopyTo(filestream);
            }

            return @"\uploads\images\book\" + newFileName + extension;
        }


        #region API_CALLS


        public IActionResult GetAll()
        {
            IEnumerable<Book> books = unitOfWork.Book.GetAll();
            return Json(new { data = books });
        }

        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            Book book = unitOfWork.Book.GetFirstOrDefault(u => u.Id == id);
            if (book != null)
            {
                unitOfWork.Book.Remove(book);
                unitOfWork.Save();
                return Json(new { success = true, message = "Book has deleted succesfully" });
            }
            return Json(new { success = false, message = "An error occured while deleting book" });
        }

        #endregion

    }
}
