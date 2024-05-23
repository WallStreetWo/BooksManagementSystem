using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Collections.Generic;
using BooksManagementSystem.Models;
using System.Linq;
using BooksManagementSystem.Data;

namespace BooksManagementSystem.Controllers;

public class BorrowingsController : Controller
{
    private readonly LibraryDbContext _context;

        public BorrowingsController(LibraryDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.GetBorrowingsAsync());
        }

        public async Task<IActionResult> Details(int? id)
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

            return View(borrowing);
        }

        public IActionResult Create()
        {
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
            return View(borrowing);
        }

        public async Task<IActionResult> Delete(int? id)
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

            return View(borrowing);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _context.DeleteBorrowingAsync(id);
            return RedirectToAction(nameof(Index));
        }

        private bool BorrowingExists(int id)
        {
            return _context.GetBorrowingsAsync().Result.Any(e => e.BorrowingID == id);
        }
}
