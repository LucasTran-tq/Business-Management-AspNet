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
    [Route("admin/salary-management/contract/[action]/{id?}")]
    [Authorize(Roles = RoleName.Administrator +  "," + RoleName.Accountant)]
    public class ContractController : Controller
    {
        private readonly AppDbContext _context;

        public ContractController(AppDbContext context)
        {
            _context = context;
        }

        [TempData]
        public string StatusMessage { get; set; }
        [TempData]
        public string StatusDeleteMessage { get; set; }
        [TempData]
        public string StatusEditMessage { get; set; }

        // GET: SalaryManagement/Contract
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.Contracts.Include(c => c.ContractType).Include(c => c.Employee);
            return View(await appDbContext.ToListAsync());
        }

        // GET: SalaryManagement/Contract/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contract = await _context.Contracts
                .Include(c => c.ContractType)
                .Include(c => c.Employee)
                .FirstOrDefaultAsync(m => m.id == id);
            if (contract == null)
            {
                return NotFound();
            }

            return View(contract);
        }

        // GET: SalaryManagement/Contract/Create
        public IActionResult Create(int? empId)
        {
            var empQuery = from emp in _context.Employees
                            where emp.EmployeeId.Equals(empId)
                            select emp;

            ViewData["EmployeeId"] = new SelectList(empQuery, "EmployeeId", "EmployeeName");
            ViewData["ContractTypeId"] = new SelectList(_context.ContractTypes, "ContractTypeId", "ContractTypeName");
            return View();
        }

        // POST: SalaryManagement/Contract/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,EmployeeId,ContractTypeId,StartTime,EndTime")] Contract contract)
        {
            if (ModelState.IsValid)
            {
                _context.Add(contract);
                await _context.SaveChangesAsync();
                StatusMessage = "You have created successfully!!!";
                return RedirectToAction(nameof(Index));
            }

            var empQuery = from emp in _context.Employees
                            where emp.EmployeeId.Equals(contract.EmployeeId)
                            select emp;
                            
            ViewData["ContractTypeId"] = new SelectList(_context.ContractTypes, "ContractTypeId", "ContractTypeName", contract.ContractTypeId);
            ViewData["EmployeeId"] = new SelectList(empQuery, "EmployeeId", "EmployeeName", contract.EmployeeId);
            return View(contract);
        }

        // GET: SalaryManagement/Contract/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contract = await _context.Contracts.FindAsync(id);
            if (contract == null)
            {
                return NotFound();
            }

            var empQuery = from emp in _context.Employees
                            where emp.EmployeeId.Equals(contract.EmployeeId)
                            select emp;

            ViewData["ContractTypeId"] = new SelectList(_context.ContractTypes, "ContractTypeId", "ContractTypeName", contract.ContractTypeId);
            ViewData["EmployeeId"] = new SelectList(empQuery, "EmployeeId", "EmployeeName", contract.EmployeeId);
            return View(contract);
        }

        // POST: SalaryManagement/Contract/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,EmployeeId,ContractTypeId,StartTime,EndTime")] Contract contract)
        {
            if (id != contract.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(contract);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ContractExists(contract.id))
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

            var empQuery = from emp in _context.Employees
                            where emp.EmployeeId.Equals(contract.EmployeeId)
                            select emp;

            ViewData["ContractTypeId"] = new SelectList(_context.ContractTypes, "ContractTypeId", "ContractTypeName", contract.ContractTypeId);
            ViewData["EmployeeId"] = new SelectList(empQuery, "EmployeeId", "EmployeeName", contract.EmployeeId);
            return View(contract);
        }

        // GET: SalaryManagement/Contract/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contract = await _context.Contracts
                .Include(c => c.ContractType)
                .Include(c => c.Employee)
                .FirstOrDefaultAsync(m => m.id == id);
            if (contract == null)
            {
                return NotFound();
            }

            return View(contract);
        }

        // POST: SalaryManagement/Contract/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var contract = await _context.Contracts.FindAsync(id);
            _context.Contracts.Remove(contract);
            await _context.SaveChangesAsync();
            StatusDeleteMessage = "You have deleted successfully!!!";
            return RedirectToAction(nameof(Index));
        }

        private bool ContractExists(int id)
        {
            return _context.Contracts.Any(e => e.id == id);
        }
    }
}
