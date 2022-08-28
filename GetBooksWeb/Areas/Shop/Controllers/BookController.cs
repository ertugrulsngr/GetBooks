using GetBooks.DataAccess.IRepository;
using GetBooks.Models;
using GetBooksWeb.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace GetBooksWeb.Areas.Shop.Controllers
{
    [Area("Shop")]
    public class BookController : Controller
    {
        private readonly ILogger<BookController> _logger;

        private readonly IUnitOfWork unitOfWork;
        public BookController(ILogger<BookController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            this.unitOfWork = unitOfWork;

        }
        public IActionResult Index()
        {
            IEnumerable<Book> books = unitOfWork.Book.GetAll();
            return View(books);
        }

        public IActionResult Details(int bookId)
        {
            Book book = unitOfWork.Book.GetFirstOrDefault(u => u.Id == bookId);
            return View(book);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
