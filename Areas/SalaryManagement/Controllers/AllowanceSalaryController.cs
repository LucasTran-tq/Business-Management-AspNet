using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using App.Areas.SalaryManagement.Models;
using App.Models;
using Microsoft.AspNetCore.Authorization;
using App.Data;

namespace AppMvc.Areas.SalaryManagement.Controllers
{
    [Area("SalaryManagement")]
    [Route("admin/salary-management/allowance-salary/[action]/{id?}")]
    [Authorize(Roles = RoleName.Administrator +  "," + RoleName.Accountant)]
    public class AllowanceSalaryController : Controller
    {
        private readonly AppDbContext _context;

        public AllowanceSalaryController(AppDbContext context)
        {
            _context = context;
        }

        [TempData]
        public string StatusMessage { get; set; }
        [TempData]
        public string StatusDeleteMessage { get; set; }
        [TempData]
        public string StatusEditMessage { get; set; }

        // GET: SalaryManagement/AllowanceSalary
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.AllowanceSalaries.Include(a => a.Position);
            return View(await appDbContext.ToListAsync());
        }

        // GET: SalaryManagement/AllowanceSalary/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var allowanceSalary = await _context.AllowanceSalaries
                .Include(a => a.Position)
                .FirstOrDefaultAsync(m => m.AllowanceSalaryId == id);
            if (allowanceSalary == null)
            {
                return NotFound();
            }

            return View(allowanceSalary);
        }

        // GET: SalaryManagement/AllowanceSalary/Create
        public IActionResult Create()
        {
            ViewData["PositionId"] = new SelectList(_context.Positions, "PositionId", "PositionName");
            return View();
        }

        // POST: SalaryManagement/AllowanceSalary/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AllowanceSalaryId,PositionId,AllowanceSalaryName,Allowance,StartTime,EndTime")] AllowanceSalary allowanceSalary)
        {
            if (ModelState.IsValid)
            {
                _context.Add(allowanceSalary);
                await _context.SaveChangesAsync();
                StatusMessage = "You have created successfully!!!";
                return RedirectToAction(nameof(Index));
            }
            ViewData["PositionId"] = new SelectList(_context.Positions, "PositionId", "PositionName", allowanceSalary.PositionId);
            return View(allowanceSalary);
        }

        // GET: SalaryManagement/AllowanceSalary/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var allowanceSalary = await _context.AllowanceSalaries.FindAsync(id);
            if (allowanceSalary == null)
            {
                return NotFound();
            }
            ViewData["PositionId"] = new SelectList(_context.Positions, "PositionId", "PositionName", allowanceSalary.PositionId);
            return View(allowanceSalary);
        }

        // POST: SalaryManagement/AllowanceSalary/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AllowanceSalaryId,PositionId,AllowanceSalaryName,Allowance,StartTime,EndTime")] AllowanceSalary allowanceSalary)
        {
            if (id != allowanceSalary.AllowanceSalaryId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(allowanceSalary);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AllowanceSalaryExists(allowanceSalary.AllowanceSalaryId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                StatusEditMessage = "You have edited successfully!!!";
                return RedirectToAction(nameof(Index));
            }
            ViewData["PositionId"] = new SelectList(_context.Positions, "PositionId", "PositionName", allowanceSalary.PositionId);
            return View(allowanceSalary);
        }

        // GET: SalaryManagement/AllowanceSalary/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var allowanceSalary = await _context.AllowanceSalaries
                .Include(a => a.Position)
                .FirstOrDefaultAsync(m => m.AllowanceSalaryId == id);
            if (allowanceSalary == null)
            {
                return NotFound();
            }

            return View(allowanceSalary);
        }

        // POST: SalaryManagement/AllowanceSalary/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var allowanceSalary = await _context.AllowanceSalaries.FindAsync(id);
            _context.AllowanceSalaries.Remove(allowanceSalary);
            await _context.SaveChangesAsync();
            StatusDeleteMessage = "You have deleted successfully!!!";
            return RedirectToAction(nameof(Index));
        }

        private bool AllowanceSalaryExists(int id)
        {
            return _context.AllowanceSalaries.Any(e => e.AllowanceSalaryId == id);
        }
    }
}
