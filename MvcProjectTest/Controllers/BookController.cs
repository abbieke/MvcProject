using MvcProjectTest.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcProjectTest.Controllers
{
    public class BookController : Controller
    {
        public readonly BooksRepository _repo;
        public BookController()
        {
            _repo = new BooksRepository();
        }
        // GET: Book
        public ActionResult Index()
        {
            var books = _repo.SelectBooks();
            return View(books);
        }

        public ActionResult BookDetail()
        {
            return View();
        }
    }
}