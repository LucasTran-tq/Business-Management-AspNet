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
    [Route("admin/salary-management/overtime-salary/[action]/{id?}")]
    [Authorize(Roles = RoleName.Administrator +  "," + RoleName.Accountant)]

    public class OvertimeSalaryController : Controller
    {
        private readonly AppDbContext _context;

        public OvertimeSalaryController(AppDbContext context)
        {
            _context = context;
        }

        [TempData]
        public string StatusMessage { get; set; }
        [TempData]
        public string StatusDeleteMessage { get; set; }
        [TempData]
        public string StatusEditMessage { get; set; }

        // GET: SalaryManagement/OvertimeSalary
        public async Task<IActionResult> Index()
        {
            return View(await _context.OvertimeSalaries.ToListAsync());
        }

        // GET: SalaryManagement/OvertimeSalary/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var overtimeSalary = await _context.OvertimeSalaries
                .FirstOrDefaultAsync(m => m.OvertimeSalaryId == id);
            if (overtimeSalary == null)
            {
                return NotFound();
            }

            return View(overtimeSalary);
        }

        // GET: SalaryManagement/OvertimeSalary/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: SalaryManagement/OvertimeSalary/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("OvertimeSalaryId,OvertimeSalaryName,moneyPerSession,StartTime,EndTime")] OvertimeSalary overtimeSalary)
        {
            if (ModelState.IsValid)
            {
                _context.Add(overtimeSalary);
                await _context.SaveChangesAsync();
                StatusMessage = "You have created successfully!!!";
                return RedirectToAction(nameof(Index));
            }
            return View(overtimeSalary);
        }

        // GET: SalaryManagement/OvertimeSalary/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var overtimeSalary = await _context.OvertimeSalaries.FindAsync(id);
            if (overtimeSalary == null)
            {
                return NotFound();
            }
            return View(overtimeSalary);
        }

        // POST: SalaryManagement/OvertimeSalary/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("OvertimeSalaryId,OvertimeSalaryName,moneyPerSession,StartTime,EndTime")] OvertimeSalary overtimeSalary)
        {
            if (id != overtimeSalary.OvertimeSalaryId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(overtimeSalary);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OvertimeSalaryExists(overtimeSalary.OvertimeSalaryId))
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
            return View(overtimeSalary);
        }

        // GET: SalaryManagement/OvertimeSalary/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var overtimeSalary = await _context.OvertimeSalaries
                .FirstOrDefaultAsync(m => m.OvertimeSalaryId == id);
            if (overtimeSalary == null)
            {
                return NotFound();
            }

            return View(overtimeSalary);
        }

        // POST: SalaryManagement/OvertimeSalary/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var overtimeSalary = await _context.OvertimeSalaries.FindAsync(id);
            _context.OvertimeSalaries.Remove(overtimeSalary);
            await _context.SaveChangesAsync();
            StatusDeleteMessage = "You have deleted successfully!!!";
            return RedirectToAction(nameof(Index));
        }

        private bool OvertimeSalaryExists(int id)
        {
            return _context.OvertimeSalaries.Any(e => e.OvertimeSalaryId == id);
        }
    }
}
