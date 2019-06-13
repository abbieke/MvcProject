using Dapper;
using MvcProjectTest.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace MvcProjectTest.Repositories
{
    public class BooksRepository
    {
        private static string connString;
        private SqlConnection conn;
        public BooksRepository()
        {
            if (string.IsNullOrEmpty(connString))
            {
                connString = ConfigurationManager.ConnectionStrings["bsmobile"].ConnectionString;
            }

            //conn = new SqlConnection(connString);
        }

        public List<Book> SelectBooks()
        {
            List<Book> books;
            using (conn = new SqlConnection(connString))
            {
                string sql = "Select TOP 8 * From Books As b INNER JOIN Author As a ON b.AuthorID = a.AuthorID Inner Join Category AS c ON b.CategoryID = c.CategoryID ORDER BY b.BooksNo DESC"; 
                books = conn.Query<Book>(sql).ToList();
            }
            return books;
        }

        public List<Book> SelectTopBooks()
        {
            List<Book> books;
            using (conn = new SqlConnection(connString))
            {
                string sql = "Select TOP 8 * From Books As b INNER JOIN Author As a ON b.AuthorID = a.AuthorID Inner Join Category AS c ON b.CategoryID = c.CategoryID ORDER BY b.InStock";
                books = conn.Query<Book>(sql).ToList();
            }
            return books;
        }

        public Book SelectBook(string id)
        {
            using (conn = new SqlConnection(connString))
            {
                string sql = "Select * From Books As b INNER JOIN Author As a ON b.AuthorID = a.AuthorID Inner Join Category AS c ON b.CategoryID = c.CategoryID Inner Join Press AS p ON b.PressID = p.PressID WHERE b.BookID = '" + id +"'";
                var book = conn.QueryFirstOrDefault<Book>(sql);
                return book;
            }
        }

        public List<Category> SelectCategory()
        {
            using (conn = new SqlConnection(connString))
            {
                //CategoryEngName,CategoryName
                string sql = "Select * From Category ";
                var category = conn.Query<Category>(sql).ToList();
                return category;
            }
        }

        public List<Book> SelectCategoryBooks(string id)
        {
            List<Book> books;
            using (conn = new SqlConnection(connString))
            {
                string sql = "Select * From Books As b INNER JOIN Author As a ON b.AuthorID = a.AuthorID Inner Join Category AS c ON b.CategoryID = c.CategoryID WHERE c.CategoryEngName = '" + id + "'OR b.BooksName like N'%" + id + "%'";
                books = conn.Query<Book>(sql).ToList();
                return books;
            }
        }

        public Book GetBookById(string bookId)
        {
            using (conn = new SqlConnection(connString))
            {
                var sql = "SELECT * FROM Books WHERE BookID=@bookId";
                return conn.QueryFirstOrDefault<Book>(sql, new { bookId });
            }
            
        }

        public string GetCategoryEngNameById(int cateId)
        {
            using (conn = new SqlConnection(connString))
            {
                var sql = "SELECT CategoryEngName FROM Category WHERE CategoryID = @cateid";
                return conn.QueryFirstOrDefault<string>(sql, new { cateId });
            }
        }

        public List<Book> GetAllBook()
        {
            List<Book> books;
            using (conn = new SqlConnection(connString))
            {
                string sql = "Select * From Books As b Inner Join Category As c On c.CategoryID = b.CategoryID";
                books = conn.Query<Book>(sql).ToList();
                return books;
            }
        }

        public Author SelectAuthor()
        {
            using (conn = new SqlConnection(connString))
            {
                string sql = "Select Top 1 od.BookID ,b.BooksName,c.CategoryEngName,b.BookImage,a.AuthorName ,SUM(od.Counts)as TopSUM " + 
                             "from [Order Detail] As od " +
                             "Inner Join Books As b On od.BookID = b.BookID " +
                             "Inner Join Author As a On a.AuthorID = b.AuthorID " +
                             "Inner Join Category As c On c.CategoryID = b.CategoryID " +
                             "Group By od.BookID,b.BooksName,c.CategoryEngName,b.BookImage,a.AuthorName " +
                             "Order By SUM(od.Counts)";
                var author = conn.QueryFirstOrDefault<Author>(sql);
                return author;
            }

        }

        public List<BookType> SelectBookType(string Category)
        {
            List<BookType> bookTypes;
            using (conn = new SqlConnection(connString))
            {
                string sql = "Select bt.BookTypeName from Books As b " +
                "Inner Join BookType As bt On b.BookTypeID = bt.BookTypeID " +
                "Inner Join Category As c On c.CategoryID = b.CategoryID " +
                "Where c.CategoryEngName = '" + Category + "'" + 
                "Group By bt.BookTypeName";

                bookTypes = conn.Query<BookType>(sql).ToList();
                return bookTypes;
            }
        }

        public List<BookType> SelectAllBookType()
        {
            List<BookType> bookTypes;
            using (conn = new SqlConnection(connString))
            {
                string sql = "Select bt.BookTypeName from Books As b " +
                             "Inner Join BookType As bt On b.BookTypeID = bt.BookTypeID " +
                             "Group By bt.BookTypeName";

                bookTypes = conn.Query<BookType>(sql).ToList();
                return bookTypes;
            }
        }

        public List<Book> SelectBookTypebooks(string Category,string Type)
        {
            List<Book> books;
            using (conn = new SqlConnection(connString))
            {
                string sql = "Select * from Books As b " +
                "Inner Join BookType As bt On b.BookTypeID = bt.BookTypeID " +
                "Inner Join Category As c On c.CategoryID = b.CategoryID " +
                "Where c.CategoryEngName = '" + Category + "' AND bt.BookTypeName = N'" + Type + "'";

                books = conn.Query<Book>(sql).ToList();
                return books;
            }
        }
        public List<Book> SelectBookTypeAllbooks(string Type)
        {
            List<Book> books;
            using (conn = new SqlConnection(connString))
            {
                string sql = "Select * from Books As b " +
                             "Inner Join BookType As bt On b.BookTypeID = bt.BookTypeID " +
                             "Inner Join Category As c On c.CategoryID = b.CategoryID " +
                             "Where bt.BookTypeName = N'" + Type + "'";

                books = conn.Query<Book>(sql).ToList();
                return books;
            }
        }
        public List<Book> SelectRandomBooks()
        {
            List<Book> books;
            using (conn = new SqlConnection(connString))
            {
                string sql = "Select TOP 8 * From Books As b INNER JOIN Author As a ON b.AuthorID = a.AuthorID Inner Join Category AS c ON b.CategoryID = c.CategoryID ORDER BY NEWID()";
                books = conn.Query<Book>(sql).ToList();
            }
            return books;
        }

        public string GetCategoryName(string engName)
        {
            using (conn = new SqlConnection(connString))
            {
                var sql = "SELECT CategoryName FROM Category WHERE CategoryEngName = '" + engName + "'";
                return conn.QueryFirstOrDefault<string>(sql);
            }
        }

        public void CreateBook(Book book)
        {
            using (conn = new SqlConnection(connString))
            {
                var _bookRepo = new BooksRepository();
                string sql = "insert into Books(BookID,BooksName,PressID,CategoryID,AuthorID,UnitPrice,InStock,Discount,Description,ISBN,BookImage) values(@bookId, @booksName, @pressId, @categoryId,@authorId, @unitPrice, @inStock, @disCount, @desCription, @iSBN, @bookImage)";
                conn.Execute(sql, new
                {

                    bookId = book.BookId,
                    booksName = book.BooksName,
                    pressId = _bookRepo.GetPressIdByName(book.PressName),
                    categoryId = book.CategoryID,
                    authorId = _bookRepo.GetAuthorIdByName(book.AuthorName),
                    unitPrice = book.UnitPrice,
                    inStock = book.InStock,
                    disCount = book.Discount,
                    desCription = book.Description,
                    iSBN = book.ISBN,
                    bookImage = book.BookImage,
                });
            }

        }

        public int GetPressIdByName(string pressName)
        {
            using (conn = new SqlConnection(connString))
            {
                var sql = "select PressID from Press where PressName = @pressName";
                return conn.QueryFirstOrDefault<int>(sql, new {
                    pressName = pressName
                });
            }
        }

        public int GetAuthorIdByName(string authorName)
        {
            using (conn = new SqlConnection(connString))
            {
                var sql = "select AuthorID from Author where AuthorName = @authorName";
                return conn.QueryFirstOrDefault<int>(sql, new
                {
                    authorName = authorName
                });
            }
        }


        public IEnumerable<RealAuthor> GetallRealAuthor()
        {
            IEnumerable<RealAuthor> allauthors;
            using (conn = new SqlConnection(connString))
            {
                string sql = "select  * from Author";
                allauthors = conn.Query<RealAuthor>(sql).ToList();
            }
            return allauthors;
        }

        public IEnumerable<Press> GetallPress()
        {
            IEnumerable<Press> allpress;
            using (conn = new SqlConnection(connString))
            {
                string sql = "select * from Press";
                allpress = conn.Query<Press>(sql).ToList();
            }
            return allpress;
        }

        public void CreatePress(Press press)
        {
            using (conn = new SqlConnection(connString))
            {
                string sql = "insert into Press(PressName,PressPhone,PressAddress) values(@pressName, @pressPhone, @pressAddress)";
                conn.Execute(sql, new
                {
                    pressName=press.PressName,
                    pressPhone=press.PressPhone,
                    pressAddress=press.PressAddress

                });
            }

        }

        public void CreateRealAuthor(RealAuthor realauthor)
        {
            using (conn = new SqlConnection(connString))
            {
                string sql = "insert into Author(AuthorName) values(@author)";
                conn.Execute(sql, new
                {
                    author=realauthor.AuthorName
                });
            }

        }

        public void CreateCategory(Category cate)
        {
            using (conn = new SqlConnection(connString))
            {
                string sql = "insert into Category(CategoryName,CategoryEngName) values(@categoryname, @categoryengName)";
                conn.Execute(sql, new
                {
                    categoryname=cate.CategoryName,
                    categoryengName=cate.CategoryEngName
                });
            }

        }

        public void UpdateBook(Book book)
        {
            using (conn = new SqlConnection(connString))
            {
                var _bookRepo = new BooksRepository();
                string sql = "update[dbo].[Books] set BooksName=@BooksName,PressID=@PressID,CategoryID=@CategoryID,AuthorID=@AuthorID,UnitPrice=@UnitPrice,InStock=@InStock,Discount=@Discount,Description=@Description where BookID=@BookID";
                conn.Execute(sql, new
                {
                    BooksName = book.BooksName,
                    PressID= _bookRepo.GetPressIdByName(book.PressName),
                    CategoryID=book.CategoryID,
                    AuthorID= _bookRepo.GetAuthorIdByName(book.AuthorName),
                    UnitPrice=book.UnitPrice,
                    InStock=book.InStock,
                    Discount=book.Discount,
                    Description=book.Description,
                    BookID = book.BookId
                });
            }
        }

    }
}