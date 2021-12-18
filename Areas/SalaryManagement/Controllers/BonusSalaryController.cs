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
    [Route("admin/salary-management/bonus-salary/[action]/{id?}")]
    [Authorize(Roles = RoleName.Administrator +  "," + RoleName.Accountant)]

    public class BonusSalaryController : Controller
    {
        private readonly AppDbContext _context;

        public BonusSalaryController(AppDbContext context)
        {
            _context = context;
        }

        [TempData]
        public string StatusMessage { get; set; }
        [TempData]
        public string StatusDeleteMessage { get; set; }
        [TempData]
        public string StatusEditMessage { get; set; }

        // GET: SalaryManagement/BonusSalary
        public async Task<IActionResult> Index()
        {
            return View(await _context.BonusSalaries.ToListAsync());
        }

        // GET: SalaryManagement/BonusSalary/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bonusSalary = await _context.BonusSalaries
                .FirstOrDefaultAsync(m => m.BonusSalaryId == id);
            if (bonusSalary == null)
            {
                return NotFound();
            }

            return View(bonusSalary);
        }

        // GET: SalaryManagement/BonusSalary/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: SalaryManagement/BonusSalary/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BonusSalaryId,BonusSalaryName,PrizeMoney,StartTime,EndTime")] BonusSalary bonusSalary)
        {
            if (ModelState.IsValid)
            {
                _context.Add(bonusSalary);
                await _context.SaveChangesAsync();
                StatusMessage = "You have created successfully!!!";
                return RedirectToAction(nameof(Index));
            }
            return View(bonusSalary);
        }

        // GET: SalaryManagement/BonusSalary/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bonusSalary = await _context.BonusSalaries.FindAsync(id);
            if (bonusSalary == null)
            {
                return NotFound();
            }
            return View(bonusSalary);
        }

        // POST: SalaryManagement/BonusSalary/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BonusSalaryId,BonusSalaryName,PrizeMoney,StartTime,EndTime")] BonusSalary bonusSalary)
        {
            if (id != bonusSalary.BonusSalaryId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(bonusSalary);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BonusSalaryExists(bonusSalary.BonusSalaryId))
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
            return View(bonusSalary);
        }

        // GET: SalaryManagement/BonusSalary/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bonusSalary = await _context.BonusSalaries
                .FirstOrDefaultAsync(m => m.BonusSalaryId == id);
            if (bonusSalary == null)
            {
                return NotFound();
            }

            return View(bonusSalary);
        }

        // POST: SalaryManagement/BonusSalary/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var bonusSalary = await _context.BonusSalaries.FindAsync(id);
            _context.BonusSalaries.Remove(bonusSalary);
            await _context.SaveChangesAsync();
            StatusDeleteMessage = "You have deleted successfully!!!";
            return RedirectToAction(nameof(Index));
        }

        private bool BonusSalaryExists(int id)
        {
            return _context.BonusSalaries.Any(e => e.BonusSalaryId == id);
        }
    }
}
