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
    [Route("admin/salary-management/basic-salary/[action]/{id?}")]
    [Authorize(Roles = RoleName.Administrator +  "," + RoleName.Accountant)]

    public class BasicSalaryController : Controller
    {
        private readonly AppDbContext _context;

        public BasicSalaryController(AppDbContext context)
        {
            _context = context;
        }

        [TempData]
        public string StatusMessage { get; set; }
        [TempData]
        public string StatusDeleteMessage { get; set; }
        [TempData]
        public string StatusEditMessage { get; set; }

        // GET: SalaryManagement/BasicSalary
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.BasicSalaries.Include(b => b.ContractType);
            return View(await appDbContext.ToListAsync());
        }

        // GET: SalaryManagement/BasicSalary/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var basicSalary = await _context.BasicSalaries
                .Include(b => b.ContractType)
                .FirstOrDefaultAsync(m => m.BasicSalaryId == id);
            if (basicSalary == null)
            {
                return NotFound();
            }

            return View(basicSalary);
        }

        // GET: SalaryManagement/BasicSalary/Create
        public IActionResult Create()
        {
            ViewData["ContractTypeId"] = new SelectList(_context.ContractTypes, "ContractTypeId", "ContractTypeName");
            return View();
        }

        // POST: SalaryManagement/BasicSalary/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BasicSalaryId,ContractTypeId,BasicSalaryName,Money,StartTime,EndTime")] BasicSalary basicSalary)
        {
            if (ModelState.IsValid)
            {
                _context.Add(basicSalary);
                await _context.SaveChangesAsync();
                StatusMessage = "You have created successfully!!!";
                return RedirectToAction(nameof(Index));
            }
            ViewData["ContractTypeId"] = new SelectList(_context.ContractTypes, "ContractTypeId", "ContractTypeName", basicSalary.ContractTypeId);
            return View(basicSalary);
        }

        // GET: SalaryManagement/BasicSalary/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var basicSalary = await _context.BasicSalaries.FindAsync(id);
            if (basicSalary == null)
            {
                return NotFound();
            }
            ViewData["ContractTypeId"] = new SelectList(_context.ContractTypes, "ContractTypeId", "ContractTypeName", basicSalary.ContractTypeId);
            return View(basicSalary);
        }

        // POST: SalaryManagement/BasicSalary/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BasicSalaryId,ContractTypeId,BasicSalaryName,Money,StartTime,EndTime")] BasicSalary basicSalary)
        {
            if (id != basicSalary.BasicSalaryId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(basicSalary);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BasicSalaryExists(basicSalary.BasicSalaryId))
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
            ViewData["ContractTypeId"] = new SelectList(_context.ContractTypes, "ContractTypeId", "ContractTypeName", basicSalary.ContractTypeId);
            return View(basicSalary);
        }

        // GET: SalaryManagement/BasicSalary/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var basicSalary = await _context.BasicSalaries
                .Include(b => b.ContractType)
                .FirstOrDefaultAsync(m => m.BasicSalaryId == id);
            if (basicSalary == null)
            {
                return NotFound();
            }

            return View(basicSalary);
        }

        // POST: SalaryManagement/BasicSalary/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var basicSalary = await _context.BasicSalaries.FindAsync(id);
            _context.BasicSalaries.Remove(basicSalary);
            await _context.SaveChangesAsync();
            StatusDeleteMessage = "You have deleted successfully!!!";
            return RedirectToAction(nameof(Index));
        }

        private bool BasicSalaryExists(int id)
        {
            return _context.BasicSalaries.Any(e => e.BasicSalaryId == id);
        }
    }
}
