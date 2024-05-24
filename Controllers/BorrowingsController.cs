using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Collections.Generic;
using BooksManagementSystem.Models;
using System.Linq;
using BooksManagementSystem.Data;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BooksManagementSystem.Controllers
{
    public class BorrowingsController : Controller
    {
        private readonly LibraryDbContext _context;

        public BorrowingsController(LibraryDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var borrowings = await _context.GetBorrowingsAsync();
            var books = await _context.GetBooksAsync();
            var members = await _context.GetMembersAsync();

            var borrowingList = borrowings.Select(b => new
            {
                b.BorrowingID,
                BookTitle = books.FirstOrDefault(book => book.BookID == b.BookID)?.Title,
                MemberName = members.FirstOrDefault(member => member.MemberID == b.MemberID)?.Name,
                b.BorrowDate,
                b.ReturnDate
            }).ToList();

            return View(borrowingList);
        }

        public async Task<IActionResult> Create()
        {
            var books = await _context.GetBooksAsync();
            ViewBag.Books = new SelectList(books, "BookID", "Title");

            var members = await _context.GetMembersAsync();
            ViewBag.Members = new SelectList(members, "MemberID", "Name");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BorrowingID,BookID,MemberID,BorrowDate,ReturnDate")] Borrowing borrowing)
        {
            if (ModelState.IsValid)
            {
                await _context.AddBorrowingAsync(borrowing);
                return RedirectToAction(nameof(Index));
            }

            var books = await _context.GetBooksAsync();
            ViewBag.Books = new SelectList(books, "BookID", "Title", borrowing.BookID);

            var members = await _context.GetMembersAsync();
            ViewBag.Members = new SelectList(members, "MemberID", "Name", borrowing.MemberID);

            return View(borrowing);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var borrowing = (await _context.GetBorrowingsAsync()).FirstOrDefault(b => b.BorrowingID == id);
            if (borrowing == null)
            {
                return NotFound();
            }

            var books = await _context.GetBooksAsync();
            ViewBag.Books = new SelectList(books, "BookID", "Title", borrowing.BookID);

            var members = await _context.GetMembersAsync();
            ViewBag.Members = new SelectList(members, "MemberID", "Name", borrowing.MemberID);

            return View(borrowing);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BorrowingID,BookID,MemberID,BorrowDate,ReturnDate")] Borrowing borrowing)
        {
            if (id != borrowing.BorrowingID)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                await _context.UpdateBorrowingAsync(borrowing);
                return RedirectToAction(nameof(Index));
            }

            var books = await _context.GetBooksAsync();
            ViewBag.Books = new SelectList(books, "BookID", "Title", borrowing.BookID);

            var members = await _context.GetMembersAsync();
            ViewBag.Members = new SelectList(members, "MemberID", "Name", borrowing.MemberID);

            return View(borrowing);
        }
    }
}
