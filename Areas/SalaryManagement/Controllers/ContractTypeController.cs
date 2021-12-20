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
    [Route("admin/salary-management/contract-type/[action]/{id?}")]
    [Authorize(Roles = RoleName.Administrator +  "," + RoleName.Accountant)]
    public class ContractTypeController : Controller
    {
        private readonly AppDbContext _context;

        public ContractTypeController(AppDbContext context)
        {
            _context = context;
        }

        [TempData]
        public string StatusMessage { get; set; }
        [TempData]
        public string StatusDeleteMessage { get; set; }
        [TempData]
        public string StatusEditMessage { get; set; }

        // GET: SalaryManagement/ContractType
        public async Task<IActionResult> Index()
        {
            return View(await _context.ContractTypes.ToListAsync());
        }

        // GET: SalaryManagement/ContractType/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contractType = await _context.ContractTypes
                .FirstOrDefaultAsync(m => m.ContractTypeId == id);
            if (contractType == null)
            {
                return NotFound();
            }

            return View(contractType);
        }

        // GET: SalaryManagement/ContractType/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: SalaryManagement/ContractType/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ContractTypeId,ContractTypeName")] ContractType contractType)
        {
            if (ModelState.IsValid)
            {
                _context.Add(contractType);
                await _context.SaveChangesAsync();
                StatusMessage = "You have created successfully!!!";
                return RedirectToAction(nameof(Index));
            }
            return View(contractType);
        }

        // GET: SalaryManagement/ContractType/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contractType = await _context.ContractTypes.FindAsync(id);
            if (contractType == null)
            {
                return NotFound();
            }
            return View(contractType);
        }

        // POST: SalaryManagement/ContractType/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ContractTypeId,ContractTypeName")] ContractType contractType)
        {
            if (id != contractType.ContractTypeId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(contractType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ContractTypeExists(contractType.ContractTypeId))
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
            return View(contractType);
        }

        // GET: SalaryManagement/ContractType/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contractType = await _context.ContractTypes
                .FirstOrDefaultAsync(m => m.ContractTypeId == id);
            if (contractType == null)
            {
                return NotFound();
            }

            return View(contractType);
        }

        // POST: SalaryManagement/ContractType/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var contractType = await _context.ContractTypes.FindAsync(id);
            _context.ContractTypes.Remove(contractType);
            await _context.SaveChangesAsync();
            StatusDeleteMessage = "You have deleted successfully!!!";
            return RedirectToAction(nameof(Index));
        }

        private bool ContractTypeExists(int id)
        {
            return _context.ContractTypes.Any(e => e.ContractTypeId == id);
        }
    }
}
