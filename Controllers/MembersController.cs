using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Collections.Generic;
using BooksManagementSystem.Models;
using System.Linq;
using BooksManagementSystem.Data;

namespace BooksManagementSystem.Controllers;

public class MembersController : Controller
{
    private readonly LibraryDbContext _context;
    public MembersController(LibraryDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        return View(await _context.GetMembersAsync());
    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var member = (await _context.GetMembersAsync()).FirstOrDefault(m => m.MemberID == id);

        if (member == null)
        {
            return NotFound();
        }

        return View(member);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("MemberID,Name,Address,PhoneNumber")] Member member)
    {
        if (ModelState.IsValid)
        {
            await _context.AddMemberAsync(member);
            return RedirectToAction(nameof(Index));
        }
        return View(member);
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var member = (await _context.GetMembersAsync()).FirstOrDefault(m => m.MemberID == id);

        if (member == null)
        {
            return NotFound();
        }
        return View(member);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("MemberID,Name,Address,PhoneNumber")] Member member)
    {
        if (id != member.MemberID)
        {
            return BadRequest();
        }

        if (ModelState.IsValid)
        {
            await _context.UpdateMemberAsync(member);
            return RedirectToAction(nameof(Index));
        }
        return View(member);
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var member = (await _context.GetMembersAsync()).FirstOrDefault(m => m.MemberID == id);

        if (member == null)
        {
            return NotFound();
        }

        return View(member);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await _context.DeleteMemberAsync(id);
        return RedirectToAction(nameof(Index));
    }

    private bool MemberExists(int id)
    {
        return _context.GetMembersAsync().Result.Any(e => e.MemberID == id);
    }
}