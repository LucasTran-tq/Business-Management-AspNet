using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using App.Areas.EmployeeManagement.Models;
using App.Models;

namespace AppMvc.Areas.EmployeeManagement.Controllers
{
    [Area("EmployeeManagement")]
    [Route("admin/employee-management/Employee_Position/[action]/{id?}")]
    public class Employee_PositionController : Controller
    {
        private readonly AppDbContext _context;

        public Employee_PositionController(AppDbContext context)
        {
            _context = context;
        }

        // GET: EmployeeManagement/Employee_Position
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.Employee_Positions.Include(e => e.Employee).Include(e => e.Position);
            return View(await appDbContext.ToListAsync());
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
        public IActionResult Create()
        {
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "EmployeeId", "EmployeeName");
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
                return RedirectToAction(nameof(Index));
            }
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "EmployeeId", "EmployeeName", employee_Position.EmployeeId);
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
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "EmployeeId", "EmployeeName", employee_Position.EmployeeId);
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
                return RedirectToAction(nameof(Index));
            }
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "EmployeeId", "EmployeeName", employee_Position.EmployeeId);
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
            return RedirectToAction(nameof(Index));
        }

        private bool Employee_PositionExists(int id)
        {
            return _context.Employee_Positions.Any(e => e.id == id);
        }
    }
}