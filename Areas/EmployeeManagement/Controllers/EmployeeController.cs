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
    [Route("admin/employee-management/employee/[action]/{id?}")]
    [Authorize(Roles = RoleName.Administrator + "," + RoleName.HR)]
    public class EmployeeController : Controller
    {
        private readonly AppDbContext _context;

        public EmployeeController(AppDbContext context)
        {
            _context = context;
        }

        [TempData]
        public string StatusMessage { get; set; }
        [TempData]
        public string StatusDeleteMessage { get; set; }


        // GET: EmployeeManagement/Employee
        public async Task<IActionResult> IndexAsync()
        {
            var appDbContext = _context.Employees
                           .Include(s => s.Department)
                           .Include(s => s.Level);

            return View(await appDbContext.ToListAsync());
        }

        [HttpGet]
        public async Task<IActionResult> Index(string EmpSearch)
        {
            ViewData["GetEmployeeDetails"] = EmpSearch;

            var empQuery = from emp in _context.Employees
                        .Include(s => s.Department)
                        .Include(s => s.Level)
                           select emp;
            if (!String.IsNullOrEmpty(EmpSearch))
            {
                empQuery = empQuery.Where(emp => emp.EmployeeName.Contains(EmpSearch));
            }
            return View(await empQuery.AsNoTracking().ToListAsync());
        }

        // GET: EmployeeManagement/Employee/Details/5

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees
                .Include(s => s.Department)
                .Include(s => s.Level)
                .FirstOrDefaultAsync(m => m.EmployeeId == id);
            if (employee == null)
            {
                return NotFound();
            }

            IQueryable<Employee_Skill> skills = from emp_skill in _context.Employee_Skills
                            .Include(e => e.Employee)
                            .Include(e => e.Skill)
                            .OrderByDescending(emp_skill => emp_skill.EvaluationDate)
                                                select emp_skill;
            skills = skills.Where(emp => emp.Employee.EmployeeId == id);

            IQueryable<Employee_Position> positions = from emp_pos in _context.Employee_Positions
                            .Include(e => e.Employee)
                            .Include(e => e.Position)
                            .OrderByDescending(emp_pos => emp_pos.StartTime)
                                                      select emp_pos;
            positions = positions.Where(emp => emp.Employee.EmployeeId == id);

            Employee_Info employee_Info = new Employee_Info();
            employee_Info.employee = employee;
            employee_Info.employee_skills = skills.ToList();
            employee_Info.employee_positions = positions.ToList();

            return View(employee_Info);
        }

        // GET: EmployeeManagement/Employee/Create
        public IActionResult Create()
        {
            ViewData["DepartmentId"] = new SelectList(_context.Departments, "DepartmentId", "DepartmentName");
            ViewData["LevelId"] = new SelectList(_context.Levels, "LevelId", "LevelName");

            return View();
        }

        // POST: EmployeeManagement/Employee/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EmployeeId,EmployeeName,DepartmentId,LevelId,DOB,Sex,PlaceOfBirth,Address")] Employee employee)
        {
            DateTime localDate = DateTime.Now;

            if (ModelState.IsValid)
            {
                _context.Add(employee);
                await _context.SaveChangesAsync();

                // set default skill and position to employee
                Employee_Skill employee_Skill = new Employee_Skill
                {
                    EmployeeId = employee.EmployeeId,
                    SkillId = 1,
                    Level = "1",
                    EvaluationDate = localDate
                };

                Employee_Position employee_Position = new Employee_Position
                {
                    EmployeeId = employee.EmployeeId,
                    PositionId = 2,
                    StartTime = localDate,
                    EndTime = localDate,
                };

                _context.Add(employee_Skill);
                _context.Add(employee_Position);
                await _context.SaveChangesAsync();

                StatusMessage = "You have created successfully!!!";
                return RedirectToAction(nameof(Index));
            }
            ViewData["DepartmentId"] = new SelectList(_context.Departments, "DepartmentId", "DepartmentName");
            ViewData["LevelId"] = new SelectList(_context.Levels, "LevelId", "LevelName");
            return View(employee);
        }

        // GET: EmployeeManagement/Employee/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }
            ViewData["DepartmentId"] = new SelectList(_context.Departments, "DepartmentId", "DepartmentName");
            ViewData["LevelId"] = new SelectList(_context.Levels, "LevelId", "LevelName");
            return View(employee);
        }

        // POST: EmployeeManagement/Employee/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("EmployeeId,EmployeeName,DepartmentId,LevelId,DOB,Sex,PlaceOfBirth,Address")] Employee employee)
        {
            if (id != employee.EmployeeId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(employee);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmployeeExists(employee.EmployeeId))
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
            ViewData["DepartmentId"] = new SelectList(_context.Departments, "DepartmentId", "DepartmentName");
            ViewData["LevelId"] = new SelectList(_context.Levels, "LevelId", "LevelName");
            return View(employee);
        }

        // GET: EmployeeManagement/Employee/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees
                .Include(s => s.Department)
                .Include(s => s.Level)
                .FirstOrDefaultAsync(m => m.EmployeeId == id);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // POST: EmployeeManagement/Employee/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var employee = await _context.Employees.FindAsync(id);
            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();
            StatusDeleteMessage = "You have deleted successfully!!!";
            return RedirectToAction(nameof(Index));
        }

        private bool EmployeeExists(int id)
        {
            return _context.Employees.Any(e => e.EmployeeId == id);
        }
    }
}
