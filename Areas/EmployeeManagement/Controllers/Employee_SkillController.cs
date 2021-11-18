using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using App.Areas.EmployeeDepartment.Models;
using App.Models;

namespace AppMvc.Areas.EmployeeManagement.Controllers
{
    [Area("EmployeeManagement")]
    [Route("admin/employee-management/employee_skill/[action]/{id?}")]
    public class Employee_SkillController : Controller
    {
        private readonly AppDbContext _context;

        public Employee_SkillController(AppDbContext context)
        {
            _context = context;
        }

        // GET: EmployeeManagement/Employee_Skill
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.Employee_Skills
                    .Include(e => e.Employee)
                    .Include(e => e.Skill)
                    .OrderByDescending(p => p.EvaluationDate);

            return View(await appDbContext.ToListAsync());
        }

        // GET: EmployeeManagement/Employee_Skill/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee_Skill = await _context.Employee_Skills
                .Include(e => e.Employee)
                .Include(e => e.Skill)
                .FirstOrDefaultAsync(m => m.id == id);
            if (employee_Skill == null)
            {
                return NotFound();
            }

            return View(employee_Skill);
        }

        // GET: EmployeeManagement/Employee_Skill/Create
        public IActionResult Create()
        {
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "EmployeeId", "EmployeeName");
            ViewData["SkillId"] = new SelectList(_context.Skills, "SkillId", "SkillName");
            return View();
        }

        // POST: EmployeeManagement/Employee_Skill/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,EmployeeId,SkillId,Level,EvaluationDate")] Employee_Skill employee_Skill)
        {
            if (ModelState.IsValid)
            {
                _context.Add(employee_Skill);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "EmployeeId", "EmployeeName", employee_Skill.EmployeeId);
            ViewData["SkillId"] = new SelectList(_context.Skills, "SkillId", "SkillName", employee_Skill.SkillId);
            return View(employee_Skill);
        }

        // GET: EmployeeManagement/Employee_Skill/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee_Skill = await _context.Employee_Skills.FindAsync(id);
            if (employee_Skill == null)
            {
                return NotFound();
            }
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "EmployeeId", "EmployeeName", employee_Skill.EmployeeId);
            ViewData["SkillId"] = new SelectList(_context.Skills, "SkillId", "SkillName", employee_Skill.SkillId);
            return View(employee_Skill);
        }

        // POST: EmployeeManagement/Employee_Skill/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,EmployeeId,SkillId,Level,EvaluationDate")] Employee_Skill employee_Skill)
        {
            if (id != employee_Skill.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(employee_Skill);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!Employee_SkillExists(employee_Skill.id))
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
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "EmployeeId", "EmployeeName", employee_Skill.EmployeeId);
            ViewData["SkillId"] = new SelectList(_context.Skills, "SkillId", "SkillName", employee_Skill.SkillId);
            return View(employee_Skill);
        }

        // GET: EmployeeManagement/Employee_Skill/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee_Skill = await _context.Employee_Skills
                .Include(e => e.Employee)
                .Include(e => e.Skill)
                .FirstOrDefaultAsync(m => m.id == id);
            if (employee_Skill == null)
            {
                return NotFound();
            }

            return View(employee_Skill);
        }

        // POST: EmployeeManagement/Employee_Skill/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var employee_Skill = await _context.Employee_Skills.FindAsync(id);
            _context.Employee_Skills.Remove(employee_Skill);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool Employee_SkillExists(int id)
        {
            return _context.Employee_Skills.Any(e => e.id == id);
        }
    }
}