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
    [Route("admin/sale-management/product/[action]/{id?}")]
    [Authorize(Roles = RoleName.Administrator + "," + RoleName.Sale)]

    public class ProductController : Controller
    {
        private readonly AppDbContext _context;

        public ProductController(AppDbContext context)
        {
            _context = context;
        }

        [TempData]
        public string StatusMessage { get; set; }
        [TempData]
        public string StatusDeleteMessage { get; set; }
        [TempData]
        public string StatusEditMessage { get; set; }

        // GET: SaleManagement/Product
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.Products.Include(p => p.ProductType).Include(p => p.Supplier);
            return View(await appDbContext.ToListAsync());
        }

        // GET: SaleManagement/Product/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.ProductType)
                .Include(p => p.Supplier)
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: SaleManagement/Product/Create
        public IActionResult Create()
        {
            ViewData["ProductTypeId"] = new SelectList(_context.ProductTypes, "ProductTypeId", "ProductTypeName");
            ViewData["SupplierId"] = new SelectList(_context.Suppliers, "SupplierId", "SupplierName");
            return View();
        }

        // POST: SaleManagement/Product/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductId,ProductName,SupplierId,ProductTypeId,Unit")] Product product)
        {
            if (ModelState.IsValid)
            {
                _context.Add(product);
                await _context.SaveChangesAsync();
                StatusMessage = "You have created successfully!!!";
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProductTypeId"] = new SelectList(_context.ProductTypes, "ProductTypeId", "ProductTypeName", product.ProductTypeId);
            ViewData["SupplierId"] = new SelectList(_context.Suppliers, "SupplierId", "SupplierName", product.SupplierId);
            return View(product);
        }

        // GET: SaleManagement/Product/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            ViewData["ProductTypeId"] = new SelectList(_context.ProductTypes, "ProductTypeId", "ProductTypeName", product.ProductTypeId);
            ViewData["SupplierId"] = new SelectList(_context.Suppliers, "SupplierId", "SupplierName", product.SupplierId);
            return View(product);
        }

        // POST: SaleManagement/Product/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProductId,ProductName,SupplierId,ProductTypeId,Unit")] Product product)
        {
            if (id != product.ProductId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.ProductId))
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
            ViewData["ProductTypeId"] = new SelectList(_context.ProductTypes, "ProductTypeId", "ProductTypeName", product.ProductTypeId);
            ViewData["SupplierId"] = new SelectList(_context.Suppliers, "SupplierId", "SupplierName", product.SupplierId);
            return View(product);
        }

        // GET: SaleManagement/Product/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.ProductType)
                .Include(p => p.Supplier)
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: SaleManagement/Product/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Products.FindAsync(id);
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            StatusDeleteMessage = "You have deleted successfully!!!";
            return RedirectToAction(nameof(Index));
        }

        // GET: SaleManagement/ShowTopProduct
        public IActionResult ShowTopProduct()
        {

         var topProductQuery = (from b in _context.Bills
                                    join d in _context.DetailBills on b.BillId equals d.BillId
                                   join p in _context.Products.Include(p => p.Supplier).Include(p => p.ProductType) 
                                   on d.ProductId equals p.ProductId
                                   join pri in _context.Prices on p.ProductId equals pri.ProductId
                                   select new
                                   {
                                       ProductId = d.ProductId,
                                       ProductName = p.ProductName,
                                       Supplier = p.Supplier.SupplierName,
                                       ProductType = p.ProductType.ProductTypeName,
                                       Amount = d.Amount,
                                       ProductPrice = pri.PriceMoney,
                                       BillDateYear = b.MakeBillTime.Year,
                                       BillDateMonth = b.MakeBillTime.Month,
                                   } into s
                                   group s by new
                                   {
                                       s.ProductId,
                                       s.ProductName,
                                       s.Supplier,
                                       s.ProductType,
                                       s.ProductPrice,
                                       s.BillDateYear,
                                       s.BillDateMonth,
                                   } into g
                                   
                                   select new
                                   {
                                       ProductId = g.Key.ProductId,
                                       ProductName = g.Key.ProductName,
                                       Supplier = g.Key.Supplier,
                                       ProductType = g.Key.ProductType,
                                       ProductPrice = g.Key.ProductPrice,
                                       BillDateYear = g.Key.BillDateYear,
                                       BillDateMonth = g.Key.BillDateMonth,
                                       Amount = g.Select(s => s.Amount).Sum(),
                                   }

                                ).OrderByDescending(g => g.Amount)
                                .ToList();


            // foreach (var item in topProductQuery)
            // {
            //     System.Console.WriteLine("ProductName: {0}, {1}", item.ProductName,item.ProductPrice);
            // }
            List<TopProduct> topProducts = new List<TopProduct>();
            foreach (var item in topProductQuery)
            {
               
                topProducts.Add(
                    new TopProduct () {
                        ProductName = item.ProductName,
                        Amount = item.Amount,
                        ProductPrice = item.ProductPrice,
                        SupplierName = item.Supplier,
                        ProductTypeName = item.ProductType,
                        // BillDate = item.BillDate.ToString("MM/dd/yyyy"),
                        BillDate = item.BillDateMonth + "/" + item.BillDateYear,
                    }
                );
            }


            return View(topProducts);
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.ProductId == id);
        }
    }

    
}
