using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Collections.Generic;
using BooksManagementSystem.Models;
using System.Linq;
using BooksManagementSystem.Data;
using X.PagedList;
using X.PagedList.Mvc.Core;

namespace BooksManagementSystem.Controllers;

public class MembersController : Controller
{
    private readonly LibraryDbContext _context;
    public MembersController(LibraryDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index(string sortOrder, string currentFilter, string searchString, int? page)
    {
        ViewData["CurrentSort"] = sortOrder;
        ViewData["NameSortParm"] = string.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
        ViewData["AddressSortParm"] = sortOrder == "Address" ? "address_desc" : "Address";
        ViewData["PhoneSortParm"] = sortOrder == "Phone" ? "phone_desc" : "Phone";

        if (searchString != null)
        {
            page = 1;
        }
        else
        {
            searchString = currentFilter;
        }

        ViewData["CurrentFilter"] = searchString;

        var members = await _context.GetMembersAsync();

        if (!string.IsNullOrEmpty(searchString))
        {
            members = members.Where(m => m.Name.Contains(searchString) || m.Address.Contains(searchString) || m.PhoneNumber.Contains(searchString)).ToList();
        }

        switch (sortOrder)
        {
            case "name_desc":
                members = members.OrderByDescending(m => m.Name).ToList();
            break;
            case "Address":
                members = members.OrderBy(m => m.Address).ToList();
            break;
            case "address_desc":
                members = members.OrderByDescending(m => m.Address).ToList();
            break;
            case "Phone":
                members = members.OrderBy(m => m.PhoneNumber).ToList();
            break;
            case "phone_desc":
                members = members.OrderByDescending(m => m.PhoneNumber).ToList();
                break;
            default:
                members = members.OrderBy(m => m.Name).ToList();
            break;
        }

            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(members.ToPagedList(pageNumber, pageSize));
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