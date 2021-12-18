using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using App.Areas.EmployeeManagement.Models;
using App.Models;
using Microsoft.AspNetCore.Authorization;
using App.Data;

namespace AppMvc.Areas.EmployeeManagement.Controllers
{
    [Area("EmployeeManagement")]
    [Route("admin/employee-management/Employee_Position/[action]/{id?}")]
    [Authorize(Roles = RoleName.Administrator +  "," + RoleName.HR)]
    public class Employee_PositionController : Controller
    {
        private readonly AppDbContext _context;

        public Employee_PositionController(AppDbContext context)
        {
            _context = context;
        }

        [TempData]
        public string StatusMessage { get; set; }
        [TempData]
        public string StatusDeleteMessage { get; set; }
        [TempData]
        public string StatusEditMessage { get; set; }

        // GET: EmployeeManagement/Employee_Position
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.Employee_Positions.Include(e => e.Employee).Include(e => e.Position);
            return View(await appDbContext.ToListAsync());
        }

        [HttpGet]
        public async Task<IActionResult> Index(string EmpSearch)
        {
            ViewData["GetEmployeePosition"] = EmpSearch;

            var emp_posQuery = from emp_pos in _context.Employee_Positions
                                        .Include(e => e.Employee)
                                        .Include(e => e.Position)
                                        .OrderByDescending(emp_pos => emp_pos.StartTime)
                               select emp_pos;

            if (!String.IsNullOrEmpty(EmpSearch))
            {
                emp_posQuery = emp_posQuery.Where(emp => emp.Employee.EmployeeName.Contains(EmpSearch));
            }
            return View(await emp_posQuery.AsNoTracking().ToListAsync());
        }

        // GET: EmployeeManagement/Employee_Position/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee_Position = await _context.Employee_Positions
                .Include(e => e.Employee)
                .Include(e => e.Position)
                .FirstOrDefaultAsync(m => m.id == id);
            if (employee_Position == null)
            {
                return NotFound();
            }

            return View(employee_Position);
        }

        // GET: EmployeeManagement/Employee_Position/Create
        public IActionResult Create(int? empId)
        {
            var empQuery = from emp in _context.Employees
                            where emp.EmployeeId.Equals(empId)
                            select emp;

            ViewData["EmployeeId"] = new SelectList(empQuery, "EmployeeId", "EmployeeName");
            ViewData["PositionId"] = new SelectList(_context.Positions, "PositionId", "PositionName");
            return View();
        }

        // POST: EmployeeManagement/Employee_Position/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,EmployeeId,PositionId,StartTime,EndTime")] Employee_Position employee_Position)
        {
            if (ModelState.IsValid)
            {
                _context.Add(employee_Position);
                await _context.SaveChangesAsync();
                StatusMessage = "You have created successfully!!!";
                return RedirectToAction(nameof(Index));
            }

            var empQuery = from emp in _context.Employees
                            where emp.EmployeeId.Equals(employee_Position.EmployeeId)
                            select emp;

            ViewData["EmployeeId"] = new SelectList(empQuery, "EmployeeId", "EmployeeName", employee_Position.EmployeeId);
            ViewData["PositionId"] = new SelectList(_context.Positions, "PositionId", "PositionName", employee_Position.PositionId);
            return View(employee_Position);
        }

        // GET: EmployeeManagement/Employee_Position/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee_Position = await _context.Employee_Positions.FindAsync(id);
            if (employee_Position == null)
            {
                return NotFound();
            }

            var empQuery = from emp in _context.Employees
                            where emp.EmployeeId.Equals(employee_Position.EmployeeId)
                            select emp;

            ViewData["EmployeeId"] = new SelectList(empQuery, "EmployeeId", "EmployeeName", employee_Position.EmployeeId);
            ViewData["PositionId"] = new SelectList(_context.Positions, "PositionId", "PositionName", employee_Position.PositionId);
            return View(employee_Position);
        }

        // POST: EmployeeManagement/Employee_Position/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,EmployeeId,PositionId,StartTime,EndTime")] Employee_Position employee_Position)
        {
            if (id != employee_Position.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(employee_Position);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!Employee_PositionExists(employee_Position.id))
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
                            where emp.EmployeeId.Equals(employee_Position.EmployeeId)
                            select emp;

            ViewData["EmployeeId"] = new SelectList(empQuery, "EmployeeId", "EmployeeName", employee_Position.EmployeeId);
            ViewData["PositionId"] = new SelectList(_context.Positions, "PositionId", "PositionName", employee_Position.PositionId);
            return View(employee_Position);
        }

        // GET: EmployeeManagement/Employee_Position/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee_Position = await _context.Employee_Positions
                .Include(e => e.Employee)
                .Include(e => e.Position)
                .FirstOrDefaultAsync(m => m.id == id);
            if (employee_Position == null)
            {
                return NotFound();
            }

            return View(employee_Position);
        }

        // POST: EmployeeManagement/Employee_Position/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var employee_Position = await _context.Employee_Positions.FindAsync(id);
            _context.Employee_Positions.Remove(employee_Position);
            await _context.SaveChangesAsync();
            StatusDeleteMessage = "You have deleted successfully!!!";
            return RedirectToAction(nameof(Index));
        }

        private bool Employee_PositionExists(int id)
        {
            return _context.Employee_Positions.Any(e => e.id == id);
        }
    }
}
