using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Collections.Generic;
using BooksManagementSystem.Models;
using System.Linq;
using BooksManagementSystem.Data;
using X.PagedList;

namespace BooksManagementSystem.Controllers;

public class BooksController : Controller
{
    private readonly LibraryDbContext _context;

        // Constructor that accepts the LibraryDbContext
        public BooksController(LibraryDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["TitleSortParm"] = string.IsNullOrEmpty(sortOrder) ? "title_desc" : "";
            ViewData["AuthorSortParm"] = sortOrder == "Author" ? "author_desc" : "Author";
            ViewData["YearSortParm"] = sortOrder == "Year" ? "year_desc" : "Year";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewData["CurrentFilter"] = searchString;

            var books = await _context.GetBooksAsync();

            if (!string.IsNullOrEmpty(searchString))
            {
                books = books.Where(b => b.Title.Contains(searchString) || b.Author.Contains(searchString)).ToList();
            }

            switch (sortOrder)
            {
                case "title_desc":
                    books = books.OrderByDescending(b => b.Title).ToList();
                    break;
                case "Author":
                    books = books.OrderBy(b => b.Author).ToList();
                    break;
                case "author_desc":
                    books = books.OrderByDescending(b => b.Author).ToList();
                    break;
                case "Year":
                    books = books.OrderBy(b => b.PublicationYear).ToList();
                    break;
                case "year_desc":
                    books = books.OrderByDescending(b => b.PublicationYear).ToList();
                    break;
                    default:
                    books = books.OrderBy(b => b.Title).ToList();
                    break;
            }

            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(books.ToPagedList(pageNumber, pageSize));
        }
        

        // GET: Books/Details/5 - Display details of a specific book
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var books = await _context.GetBooksAsync();
            var book = books.FirstOrDefault(b => b.BookID == id);

            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // GET: Books/Create - Show the form to create a new book
        public IActionResult Create()
        {
            return View();
        }

        // POST: Books/Create - Handle the creation of a new book
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,Author,PublicationYear")] Book book)
        {
            if (ModelState.IsValid)
            {
                await _context.AddBookAsync(book);
                return RedirectToAction(nameof(Index));
            }
            return View(book);
        }

        // GET: Books/Edit/5 - Show the form to edit an existing book
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var books = await _context.GetBooksAsync();
            var book = books.FirstOrDefault(b => b.BookID == id);

            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // POST: Books/Edit/5 - Handle the update of an existing book
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BookID,Title,Author,PublicationYear")] Book book)
        {
            if (id != book.BookID)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                await _context.UpdateBookAsync(book);
                return RedirectToAction(nameof(Index));
            }
            return View(book);
        }

        // GET: Books/Delete/5 - Show the form to confirm deletion of a book
        public async Task<IActionResult> DeleteBook(int id)
        {
            var book = await _context.GetBooksAsync();
            var bookToDelete = book.FirstOrDefault(b => b.BookID == id);
            if (bookToDelete != null)
            {
                await _context.DeleteBookAsync(id);
            }
            return RedirectToAction(nameof(Index));
        }

}

