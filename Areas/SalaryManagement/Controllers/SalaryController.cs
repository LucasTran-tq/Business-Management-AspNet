using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using App.Areas.SalaryManagement.Models;
using App.Models;

namespace AppMvc.Areas.SalaryManagement.Controllers
{
    [Area("SalaryManagement")]
    [Route("admin/salary-management/salary/[action]/{id?}")]

    public class SalaryController : Controller
    {
        private readonly AppDbContext _context;

        public SalaryController(AppDbContext context)
        {
            _context = context;
        }

        // GET: SalaryManagement/Salary
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.Salaries.Include(s => s.AllowanceSalary).Include(s => s.BasicSalary).Include(s => s.BonusSalary).Include(s => s.Employee).Include(s => s.OvertimeSalary);
            return View(await appDbContext.ToListAsync());
        }

        // GET: SalaryManagement/Salary/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var salary = await _context.Salaries
                .Include(s => s.AllowanceSalary)
                .Include(s => s.BasicSalary)
                .Include(s => s.BonusSalary)
                .Include(s => s.Employee)
                .Include(s => s.OvertimeSalary)
                .FirstOrDefaultAsync(m => m.SalaryId == id);
            if (salary == null)
            {
                return NotFound();
            }

            return View(salary);
        }

        
        // GET: SalaryManagement/Salary/Create
        public IActionResult Create()
        {
            ViewData["AllowanceSalaryId"] = new SelectList(_context.AllowanceSalaries, "AllowanceSalaryId", "AllowanceSalaryName");
            ViewData["BasicSalaryId"] = new SelectList(_context.BasicSalaries, "BasicSalaryId", "BasicSalaryName");
            ViewData["BonusSalaryId"] = new SelectList(_context.BonusSalaries, "BonusSalaryId", "BonusSalaryName");
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "EmployeeId", "EmployeeName");
            ViewData["OvertimeSalaryId"] = new SelectList(_context.OvertimeSalaries, "OvertimeSalaryId", "OvertimeSalaryName");
            return View();
        }

        // POST: SalaryManagement/Salary/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SalaryId,EmployeeId,BasicSalaryId,AllowanceSalaryId,BonusSalaryId,OvertimeSalaryId,BonusLevel,NumberOfSession,SalaryDate")] Salary salary)
        {
            if (ModelState.IsValid)
            {
                _context.Add(salary);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AllowanceSalaryId"] = new SelectList(_context.AllowanceSalaries, "AllowanceSalaryId", "AllowanceSalaryName", salary.AllowanceSalaryId);
            ViewData["BasicSalaryId"] = new SelectList(_context.BasicSalaries, "BasicSalaryId", "BasicSalaryName", salary.BasicSalaryId);
            ViewData["BonusSalaryId"] = new SelectList(_context.BonusSalaries, "BonusSalaryId", "BonusSalaryName", salary.BonusSalaryId);
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "EmployeeId", "EmployeeName", salary.EmployeeId);
            ViewData["OvertimeSalaryId"] = new SelectList(_context.OvertimeSalaries, "OvertimeSalaryId", "OvertimeSalaryName", salary.OvertimeSalaryId);
            return View(salary);
        }

        // GET: SalaryManagement/Salary/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var salary = await _context.Salaries.FindAsync(id);
            if (salary == null)
            {
                return NotFound();
            }
            ViewData["AllowanceSalaryId"] = new SelectList(_context.AllowanceSalaries, "AllowanceSalaryId", "AllowanceSalaryName", salary.AllowanceSalaryId);
            ViewData["BasicSalaryId"] = new SelectList(_context.BasicSalaries, "BasicSalaryId", "BasicSalaryName", salary.BasicSalaryId);
            ViewData["BonusSalaryId"] = new SelectList(_context.BonusSalaries, "BonusSalaryId", "BonusSalaryName", salary.BonusSalaryId);
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "EmployeeId", "EmployeeName", salary.EmployeeId);
            ViewData["OvertimeSalaryId"] = new SelectList(_context.OvertimeSalaries, "OvertimeSalaryId", "OvertimeSalaryName", salary.OvertimeSalaryId);
            return View(salary);
        }

        // POST: SalaryManagement/Salary/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("SalaryId,EmployeeId,BasicSalaryId,AllowanceSalaryId,BonusSalaryId,OvertimeSalaryId,BonusLevel,NumberOfSession,SalaryDate")] Salary salary)
        {
            if (id != salary.SalaryId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(salary);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SalaryExists(salary.SalaryId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["AllowanceSalaryId"] = new SelectList(_context.AllowanceSalaries, "AllowanceSalaryId", "AllowanceSalaryName", salary.AllowanceSalaryId);
            ViewData["BasicSalaryId"] = new SelectList(_context.BasicSalaries, "BasicSalaryId", "BasicSalaryName", salary.BasicSalaryId);
            ViewData["BonusSalaryId"] = new SelectList(_context.BonusSalaries, "BonusSalaryId", "BonusSalaryName", salary.BonusSalaryId);
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "EmployeeId", "EmployeeName", salary.EmployeeId);
            ViewData["OvertimeSalaryId"] = new SelectList(_context.OvertimeSalaries, "OvertimeSalaryId", "OvertimeSalaryName", salary.OvertimeSalaryId);
            return View(salary);
        }

        // GET: SalaryManagement/Salary/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var salary = await _context.Salaries
                .Include(s => s.AllowanceSalary)
                .Include(s => s.BasicSalary)
                .Include(s => s.BonusSalary)
                .Include(s => s.Employee)
                .Include(s => s.OvertimeSalary)
                .FirstOrDefaultAsync(m => m.SalaryId == id);
            if (salary == null)
            {
                return NotFound();
            }

            return View(salary);
        }

        // POST: SalaryManagement/Salary/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var salary = await _context.Salaries.FindAsync(id);
            _context.Salaries.Remove(salary);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SalaryExists(int id)
        {
            return _context.Salaries.Any(e => e.SalaryId == id);
        }
    }
}
