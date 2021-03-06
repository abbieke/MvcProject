﻿using MvcProjectTest.Models;
using MvcProjectTest.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcProjectTest.Controllers
{
    public class HomeController : Controller
    {
        public readonly BooksRepository _repo;
        public HomeController()
        {
            _repo = new BooksRepository();
        }
        // GET: Home
        public ActionResult Index()
        {
            var books = _repo.SelectBooks();
            var randombooks = _repo.SelectRandomBooks();
            var topbooks = _repo.SelectTopBooks();
            var author = _repo.SelectAuthor();
            Mix mix = new Mix() { Books = books, SecBooks = randombooks, ThirdBooks = topbooks, TopAuthor = author};
            return View(mix);
        }

        public ActionResult Test()
        {
            var books = _repo.SelectBooks();
            return View(books);
        }
        public ActionResult AboutUs()
        {
            return View();
        }


    }
}