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

        public ActionResult Index(string name)
        {
            var books = _repo.SelectCategoryBooks(name);
            ViewBag.catEngName = name;
            return View(books);
        }

        [HttpPost]
        public ActionResult Index(string bookKind,string maxValue, string minValue)
        {

            var books = _repo.SelectCategoryBooks(bookKind);
            var result = new List<Book>();
            foreach (var i in books)
            {
                if ((i.UnitPrice >= Convert.ToInt32(minValue)) && (i.UnitPrice <=Convert.ToInt32(maxValue)))
                {
                    result.Add(i);
                }
            }
            ViewBag.cardIndex = 0;
            ViewBag.catEngName = bookKind;
            //return PartialView("BookByRange", result);
            return View(result);
        }


        public ActionResult BookDetail(string id)
        {
            var book = _repo.SelectBook(id);
            var books = _repo.SelectTopBooks();
            BookMix mix = new BookMix(){ Book = book, SelectBooks = books };
            //return View();
            return View(mix);
        }
        //public ActionResult BookByRange(string bookKind, int valmin, int valmax)
        //{
        //    var books = _repo.SelectCategoryBooks(bookKind);
        //    var result = new List<Book>();
        //    foreach (var i in books)
        //    {
        //        if ((i.UnitPrice >= valmin) && (i.UnitPrice <= valmax))
        //        {
        //            result.Add(i);
        //        }
        //    }
        //    return View("Index",result);
        //}
    }
}