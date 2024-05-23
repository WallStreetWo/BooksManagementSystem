using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Collections.Generic;
using BooksManagementSystem.Models;
using System.Linq;
using BooksManagementSystem.Data;

namespace BooksManagementSystem.Controllers;

public class BooksController : Controller
{
    private readonly LibraryDbContext _context;

        // Constructor that accepts the LibraryDbContext
        public BooksController(LibraryDbContext context)
        {
            _context = context;
        }

        // GET: Books - Display a list of all books
        public async Task<IActionResult> Index()
        {
            var books = await _context.GetBooksAsync();
            return View(books);
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
        public async Task<IActionResult> Delete(int? id)
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

        // POST: Books/Delete/5 - Handle the deletion of a book
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _context.DeleteBookAsync(id);
            return RedirectToAction(nameof(Index));
        }
}

