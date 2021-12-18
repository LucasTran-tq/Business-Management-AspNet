using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using App.Areas.SaleManagement.Models;
using App.Models;
using Microsoft.AspNetCore.Authorization;
using App.Data;

namespace AppMvc.Areas.SaleManagement.Controllers
{
    [Area("SaleManagement")]
    [Route("admin/sale-management/detail-bill/[action]/{id?}")]
    [Authorize(Roles = RoleName.Administrator +  "," + RoleName.Sale)]
    public class DetailBillController : Controller
    {
        private readonly AppDbContext _context;

        public DetailBillController(AppDbContext context)
        {
            _context = context;
        }

        [TempData]
        public string StatusMessage { get; set; }
        [TempData]
        public string StatusDeleteMessage { get; set; }
        [TempData]
        public string StatusEditMessage { get; set; }
        
        // GET: SaleManagement/DetailBill
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.DetailBills.Include(d => d.Bill).Include(d => d.Product);
            return View(await appDbContext.ToListAsync());
        }

        // GET: SaleManagement/DetailBill/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var detailBill = await _context.DetailBills
                .Include(d => d.Bill)
                .Include(d => d.Product)
                .FirstOrDefaultAsync(m => m.DetailBillId == id);
            if (detailBill == null)
            {
                return NotFound();
            }

            return View(detailBill);
        }

        // GET: SaleManagement/DetailBill/Create
        public IActionResult Create()
        {
            ViewData["BillId"] = new SelectList(_context.Bills, "BillId", "BillId");
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "ProductName");
            return View();
        }

        // POST: SaleManagement/DetailBill/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DetailBillId,ProductId,BillId,Amount")] DetailBill detailBill)
        {
            if (ModelState.IsValid)
            {
                _context.Add(detailBill);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BillId"] = new SelectList(_context.Bills, "BillId", "BillId", detailBill.BillId);
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "ProductName", detailBill.ProductId);
            return View(detailBill);
        }

        // GET: SaleManagement/DetailBill/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var detailBill = await _context.DetailBills.FindAsync(id);
            if (detailBill == null)
            {
                return NotFound();
            }
            ViewData["BillId"] = new SelectList(_context.Bills, "BillId", "BillId", detailBill.BillId);
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "ProductName", detailBill.ProductId);
            return View(detailBill);
        }

        // POST: SaleManagement/DetailBill/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("DetailBillId,ProductId,BillId,Amount")] DetailBill detailBill)
        {
            if (id != detailBill.DetailBillId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(detailBill);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DetailBillExists(detailBill.DetailBillId))
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
            ViewData["BillId"] = new SelectList(_context.Bills, "BillId", "BillId", detailBill.BillId);
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "ProductName", detailBill.ProductId);
            return View(detailBill);
        }

        // GET: SaleManagement/DetailBill/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var detailBill = await _context.DetailBills
                .Include(d => d.Bill)
                .Include(d => d.Product)
                .FirstOrDefaultAsync(m => m.DetailBillId == id);
            if (detailBill == null)
            {
                return NotFound();
            }

            return View(detailBill);
        }

        // POST: SaleManagement/DetailBill/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var detailBill = await _context.DetailBills.FindAsync(id);
            _context.DetailBills.Remove(detailBill);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DetailBillExists(int id)
        {
            return _context.DetailBills.Any(e => e.DetailBillId == id);
        }
    }
}
