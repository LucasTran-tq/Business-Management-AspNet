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
    [Route("admin/sale-management/bill/[action]/{id?}")]
    [Authorize(Roles = RoleName.Administrator + "," + RoleName.Sale)]
    public class BillController : Controller
    {
        private readonly AppDbContext _context;

        public BillController(AppDbContext context)
        {
            _context = context;
        }

        [TempData]
        public string StatusMessage { get; set; }
        [TempData]
        public string StatusDeleteMessage { get; set; }
        [TempData]
        public string StatusEditMessage { get; set; }

        // GET: SaleManagement/Bill
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.Bills.Include(b => b.Customer).Include(b => b.Employee);
            return View(await appDbContext.ToListAsync());
        }

        // GET: SaleManagement/Bill/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bill = await _context.Bills
                .Include(b => b.Customer)
                .Include(b => b.Employee)
                .FirstOrDefaultAsync(m => m.BillId == id);
            if (bill == null)
            {
                return NotFound();
            }

            IQueryable<DetailBill> detailBills = from bill_detail in _context.DetailBills
                            .Include(d => d.Bill)
                            .Include(d => d.Product)
                            .OrderByDescending(emp_skill => emp_skill.DetailBillId)
                                                 select bill_detail;
            detailBills = detailBills.Where(emp => emp.BillId == id);
            Bill_Info bill_info = new Bill_Info();
            bill_info.bill = bill;
            bill_info.detailBills = detailBills.ToList();
            return View(bill_info);
        }

        // GET: SaleManagement/Bill/Create
        public IActionResult Create()
        {
            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "CustomerName");
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "EmployeeId", "EmployeeName");
            return View();
        }

        // POST: SaleManagement/Bill/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BillId,EmployeeId,CustomerId,MakeBillTime,ExportBillTime")] Bill bill)
        {
            if (ModelState.IsValid)
            {   
                bill.TotalBill = 0.0;

                _context.Add(bill);
                await _context.SaveChangesAsync();
                return RedirectToAction("Edit", new { id = bill.BillId });
            }
            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "CustomerName", bill.CustomerId);
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "EmployeeId", "EmployeeName", bill.EmployeeId);
            return View(bill);
        }

        // GET: SaleManagement/Bill/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bill = await _context.Bills.FindAsync(id);
            if (bill == null)
            {
                return NotFound();
            }
            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "CustomerName", bill.CustomerId);
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "EmployeeId", "EmployeeName", bill.EmployeeId);

            IQueryable<DetailBill> detailBills = from bill_detail in _context.DetailBills
                            .Include(d => d.Bill)
                            .Include(d => d.Product)
                            .OrderByDescending(emp_skill => emp_skill.DetailBillId)
                                                 select bill_detail;
            detailBills = detailBills.Where(emp => emp.BillId == id);

            Bill_Info bill_info = new Bill_Info();
            bill_info.bill = bill;
            bill_info.detailBills = detailBills.ToList();

            var billSel = (from b in _context.Bills
                           where b.BillId.Equals(id)
                           select b).ToList();

            ViewData["BillId"] = new SelectList(billSel, "BillId", "BillId");
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "ProductName");
            return View(bill_info);
        }

        // POST: SaleManagement/Bill/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BillId,EmployeeId,CustomerId,MakeBillTime,ExportBillTime")] Bill bill)
        {
            if (id != bill.BillId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var billUpdation = (from billUp in _context.Bills
                                    where billUp.BillId.Equals(bill.BillId)
                                    select billUp.TotalBill).SingleOrDefault();
                

                try
                {
                    bill.TotalBill = billUpdation;

                    _context.Update(bill);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BillExists(bill.BillId))
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
            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "CustomerName", bill.CustomerId);
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "EmployeeId", "EmployeeName", bill.EmployeeId);
            return View(bill);
        }

        [HttpPost]
        public async Task<ActionResult> CreateDetailBill(DetailBill detail)
        {
            DateTime localDate = DateTime.Now;
            if (ModelState.IsValid)
            {   
                // update total bill for bill
                var billUpdation = _context.Bills.Where(b => b.BillId.Equals(detail.BillId)).SingleOrDefault();

                var priceQuery = (from p in _context.Prices
                             where p.ProductId.Equals(detail.ProductId)
                             && DateTime.Compare(p.EndTime, localDate).Equals(1) 
                             && DateTime.Compare(p.StartTime, localDate).Equals(-1)
                             select p.PriceMoney).First();
                
                billUpdation.TotalBill += priceQuery * detail.Amount;

                // create a detail bill
                detail.PriceProduct = priceQuery;

                _context.Add(detail);
                await _context.SaveChangesAsync();
                StatusMessage = "You have created successfully!!!";
                return RedirectToAction("Edit", new { id = detail.BillId });
            }
            return View();
        }

        public async Task<IActionResult> DeleteDetailBill(int? id)
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
        [HttpPost, ActionName("DeleteDetailBill")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteDetailBillConfirmed(int id)
        {
             DateTime localDate = DateTime.Now;
             
            var detailBill = await _context.DetailBills.FindAsync(id);

            // update total bill for bill
                var billUpdation = _context.Bills.Where(b => b.BillId.Equals(detailBill.BillId)).SingleOrDefault();

                var priceQuery = (from p in _context.Prices
                             where p.ProductId.Equals(detailBill.ProductId)
                             && DateTime.Compare(p.EndTime, localDate).Equals(1) 
                             && DateTime.Compare(p.StartTime, localDate).Equals(-1)
                             select p.PriceMoney).First();
                
                billUpdation.TotalBill -= priceQuery * detailBill.Amount;

            _context.DetailBills.Remove(detailBill);
            await _context.SaveChangesAsync();
            StatusDeleteMessage = "You have deleted successfully!!!";
            return  RedirectToAction("Edit", new { id = detailBill.BillId });
        }

        // GET: SaleManagement/Bill/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bill = await _context.Bills
                .Include(b => b.Customer)
                .Include(b => b.Employee)
                .FirstOrDefaultAsync(m => m.BillId == id);
            if (bill == null)
            {
                return NotFound();
            }

            return View(bill);
        }

        // POST: SaleManagement/Bill/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var bill = await _context.Bills.FindAsync(id);
            _context.Bills.Remove(bill);
            await _context.SaveChangesAsync();
            StatusDeleteMessage = "You have deleted successfully!!!";
            return RedirectToAction(nameof(Index));
        }


        private bool BillExists(int id)
        {
            return _context.Bills.Any(e => e.BillId == id);
        }

        public JsonResult GetPriceByProduct(int ProductID)
        {
            DateTime localDate = DateTime.Now;

            var priceQuery = (from p in _context.Prices
                             where p.ProductId.Equals(ProductID)
                             && DateTime.Compare(p.EndTime, localDate).Equals(1) 
                             && DateTime.Compare(p.StartTime, localDate).Equals(-1)
                             select p.PriceMoney).First();
            
            
            return Json(priceQuery);
        }
    }
}
