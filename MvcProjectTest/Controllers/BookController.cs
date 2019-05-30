using MvcProjectTest.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcProjectTest.Models;

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
        //public ActionResult Index()
        //{
        //    var books = _repo.SelectBooks();
        //    return View(books);
        //}

        public ActionResult Index(string id)
        {
            var book = _repo.SelectCategoryBooks(id);
            return View(book);
        }

        public ActionResult BookDetail(string id)
        {
            var book = _repo.SelectBook(id);
            var books = _repo.SelectTopBooks();
            BookMix mix = new BookMix(){ Book = book, SelectBooks = books };
            //return View();
            return View(mix);
        }
    }
}