using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using App.Areas.SalaryManagement.Models;
using App.Models;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Authorization;
using App.Data;

namespace AppMvc.Areas.SalaryManagement.Controllers
{
    [Area("SalaryManagement")]
    [Route("admin/salary-management/salary/[action]/{id?}")]
    [Authorize(Roles = RoleName.Administrator +  "," + RoleName.Accountant)]

    public class SalaryController : Controller
    {
        private const int V = 0;
        private readonly AppDbContext _context;

        public SalaryController(AppDbContext context)
        {
            _context = context;
        }

        [TempData]
        public string StatusMessage { get; set; }
        [TempData]
        public string StatusDeleteMessage { get; set; }
        // GET: SalaryManagement/Salary
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.Salaries
                .Include(s => s.AllowanceSalary)
                .Include(s => s.BasicSalary)
                .Include(s => s.BonusSalary)
                .Include(s => s.Employee)
                .Include(s => s.OvertimeSalary)
                .OrderByDescending(s => s.SalaryDate);
            return View(await appDbContext.ToListAsync());
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        // [Route("admin/salary-management/salary/Index/EmpSearch={EmpSearch}&StartTimeSalary={StartTimeSalary}")]
        public async Task<IActionResult> Index(string EmpSearch, DateTime StartTimeSalary)
        {
            ViewData["GetSalaryHistoryOfEmployee"] = EmpSearch;
            ViewData["GetSalaryTable"] = StartTimeSalary;

            var empQuery = from s in _context.Salaries
                                .Include(s => s.AllowanceSalary)
                                .Include(s => s.BasicSalary)
                                .Include(s => s.BonusSalary)
                                .Include(s => s.Employee)
                                .Include(s => s.OvertimeSalary)
                                .OrderByDescending(s => s.SalaryDate)
                           select s;

            //  EmpName = !null and startTime = null
            if (!String.IsNullOrEmpty(EmpSearch) && StartTimeSalary == DateTime.MinValue)
            {
                empQuery = empQuery.Where(emp => emp.Employee.EmployeeName.Contains(EmpSearch));
            }
            // EmpName = null and startTime = !null
            else if (String.IsNullOrEmpty(EmpSearch) && StartTimeSalary != DateTime.MinValue)
            {
                empQuery = empQuery.Where(emp => emp.SalaryDate.Year.Equals(StartTimeSalary.Year)
                && emp.SalaryDate.Month.Equals(StartTimeSalary.Month));
            }
            // EmpName = !null and startTime = !null
            else if (!String.IsNullOrEmpty(EmpSearch) && StartTimeSalary != DateTime.MinValue)
            {
                empQuery = empQuery.Where(emp => emp.Employee.EmployeeName.Contains(EmpSearch) && emp.SalaryDate.Year.Equals(StartTimeSalary.Year)
                && emp.SalaryDate.Month.Equals(StartTimeSalary.Month));
            }
            // EmpName = null and startTime = null

            return View(await empQuery.AsNoTracking().ToListAsync());
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
            // var basicSalary = (from b in _context.BasicSalaries
            //                        where b.BasicSalaryId == salary.BasicSalaryId
            //                        select b.Money)
            // GetBasicSalaryByEmpId(1);

            // ViewData["AllowanceSalaryId"] = new SelectList(_context.AllowanceSalaries, "AllowanceSalaryId", "AllowanceSalaryName");
            // ViewData["BasicSalaryId"] = new SelectList(_context.BasicSalaries, "BasicSalaryId", "BasicSalaryName");
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
        public async Task<IActionResult> Create([Bind("SalaryId,EmployeeId,BasicSalaryId,AllowanceSalaryId,BonusSalaryId,OvertimeSalaryId,NumberOfSession,SalaryDate")] Salary salary)
        {

            if (ModelState.IsValid)
            {
                var basicSalary = (from b in _context.BasicSalaries
                                   where b.BasicSalaryId == salary.BasicSalaryId
                                   select b.Money).First();

                var allowanceSalary = (from a in _context.AllowanceSalaries
                                       where a.AllowanceSalaryId == salary.AllowanceSalaryId
                                       select a.Allowance).First();

                var bonusSalary = (from bonus in _context.BonusSalaries
                                   where bonus.BonusSalaryId == salary.BonusSalaryId
                                   select bonus.PrizeMoney).First();

                var overtimeSalary = (from overtime in _context.OvertimeSalaries
                                      where overtime.OvertimeSalaryId == salary.OvertimeSalaryId
                                      select overtime.moneyPerSession).First();

                // Console.WriteLine("Salary: {0} + {1} + {2} + {3}",basicSalary.ToString(), 
                // allowanceSalary.ToString(), bonusSalary.ToString(), (overtimeSalary * salary.NumberOfSession).ToString() );

                salary.TotalSalary = basicSalary + allowanceSalary
                    + bonusSalary + overtimeSalary * salary.NumberOfSession;


                _context.Add(salary);
                await _context.SaveChangesAsync();
                StatusMessage = "You have created successfully!!!";
                return RedirectToAction(nameof(Index));
            }
            // ViewData["AllowanceSalaryId"] = new SelectList(_context.AllowanceSalaries, "AllowanceSalaryId", "AllowanceSalaryName", salary.AllowanceSalaryId);
            // ViewData["BasicSalaryId"] = new SelectList(_context.BasicSalaries, "BasicSalaryId", "BasicSalaryName", salary.BasicSalaryId);
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
            // ViewData["AllowanceSalaryId"] = new SelectList(_context.AllowanceSalaries, "AllowanceSalaryId", "AllowanceSalaryName", salary.AllowanceSalaryId);
            // ViewData["BasicSalaryId"] = new SelectList(_context.BasicSalaries, "BasicSalaryId", "BasicSalaryName", salary.BasicSalaryId);
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
        public async Task<IActionResult> Edit(int id, [Bind("SalaryId,EmployeeId,BasicSalaryId,AllowanceSalaryId,BonusSalaryId,OvertimeSalaryId,NumberOfSession,SalaryDate")] Salary salary)
        {
            if (id != salary.SalaryId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var basicSalary = (from b in _context.BasicSalaries
                                       where b.BasicSalaryId == salary.BasicSalaryId
                                       select b.Money).First();

                    var allowanceSalary = (from a in _context.AllowanceSalaries
                                           where a.AllowanceSalaryId == salary.AllowanceSalaryId
                                           select a.Allowance).First();

                    var bonusSalary = (from bonus in _context.BonusSalaries
                                       where bonus.BonusSalaryId == salary.BonusSalaryId
                                       select bonus.PrizeMoney).First();

                    var overtimeSalary = (from overtime in _context.OvertimeSalaries
                                          where overtime.OvertimeSalaryId == salary.OvertimeSalaryId
                                          select overtime.moneyPerSession).First();

                    // Console.WriteLine("Salary: {0} + {1} + {2} + {3}",basicSalary.ToString(), 
                    // allowanceSalary.ToString(), bonusSalary.ToString(), (overtimeSalary * salary.NumberOfSession).ToString() );

                    salary.TotalSalary = basicSalary + allowanceSalary
                        + bonusSalary + overtimeSalary * salary.NumberOfSession;

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
            // ViewData["AllowanceSalaryId"] = new SelectList(_context.AllowanceSalaries, "AllowanceSalaryId", "AllowanceSalaryName", salary.AllowanceSalaryId);
            // ViewData["BasicSalaryId"] = new SelectList(_context.BasicSalaries, "BasicSalaryId", "BasicSalaryName", salary.BasicSalaryId);
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
            StatusDeleteMessage = "You have deleted successfully!!!";
            return RedirectToAction(nameof(Index));
        }


        // GET: SalaryManagement/Salary/ShowChart
        public IActionResult ShowChart()
        {
            return View();
        }

        // GET: SalaryManagement/Salary/ShowChartForEmployee
        public IActionResult ShowChartForEmployee()
        {
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "EmployeeId", "EmployeeName");
            return View();
        }

        // get data for Chart to show total salary of employee
        public JsonResult GetReportSalaryForEmpByYear(string EmpID, int year)
        {

            int empID = Int32.Parse(EmpID);

            var salaryQuery = _context.Salaries
                                    .Where(s => s.EmployeeId.Equals(empID) && s.SalaryDate.Year.Equals(year))
                                    .ToList();

            double month1 = 0.0;
            double month2 = 0.0;
            double month3 = 0.0;
            double month4 = 0.0;
            double month5 = 0.0;
            double month6 = 0.0;
            double month7 = 0.0;
            double month8 = 0.0;
            double month9 = 0.0;
            double month10 = 0.0;
            double month11 = 0.0;
            double month12 = 0.0;

            foreach (var item in salaryQuery)
            {
                switch (item.SalaryDate.Month)
                {
                    case 1:
                        month1 += item.TotalSalary;
                        break;
                    case 2:
                        month2 += item.TotalSalary;
                        break;
                    case 3:
                        month3 += item.TotalSalary;
                        break;
                    case 4:
                        month4 += item.TotalSalary;
                        break;
                    case 5:
                        month5 += item.TotalSalary;
                        break;
                    case 6:
                        month6 += item.TotalSalary;
                        break;
                    case 7:
                        month7 += item.TotalSalary;
                        break;
                    case 8:
                        month8 += item.TotalSalary;
                        break;
                    case 9:
                        month9 += item.TotalSalary;
                        break;
                    case 10:
                        month10 += item.TotalSalary;
                        break;
                    case 11:
                        month11 += item.TotalSalary;
                        break;
                    case 12:
                        month12 += item.TotalSalary;
                        break;
                }
            }

            var listTotalSalaryMonths = new System.Collections.ArrayList();
            listTotalSalaryMonths.Add(month1);
            listTotalSalaryMonths.Add(month2);
            listTotalSalaryMonths.Add(month3);
            listTotalSalaryMonths.Add(month4);
            listTotalSalaryMonths.Add(month5);
            listTotalSalaryMonths.Add(month6);
            listTotalSalaryMonths.Add(month7);
            listTotalSalaryMonths.Add(month8);
            listTotalSalaryMonths.Add(month9);
            listTotalSalaryMonths.Add(month10);
            listTotalSalaryMonths.Add(month11);
            listTotalSalaryMonths.Add(month12);


            return Json(listTotalSalaryMonths);
        }


        private bool SalaryExists(int id)
        {
            return _context.Salaries.Any(e => e.SalaryId == id);
        }


        // get data for Create Salary
        public JsonResult GetBasicSalaryByEmpId(int EmployeeId)
        {

            List<BasicSalary> basicSalary = _context.BasicSalaries.ToList();
            List<ContractType> contractType = _context.ContractTypes.ToList();
            List<Contract> contract = _context.Contracts.ToList();

            var list = (from basic in basicSalary
                        join ctype in contractType on basic.ContractTypeId equals ctype.ContractTypeId
                        join con in contract on ctype.ContractTypeId equals con.ContractTypeId
                        where con.EmployeeId == EmployeeId
                        select new
                        {
                            BasicSalaryId = basic.BasicSalaryId,
                            BasicSalaryName = basic.BasicSalaryName,
                        }).ToList();


            return Json(list);
        }

        // get data for Create Salary
        public JsonResult GetAllowanceSalaryByEmpId(int EmployeeId)
        {

            List<BasicSalary> basicSalary = _context.BasicSalaries.ToList();
            List<ContractType> contractType = _context.ContractTypes.ToList();
            List<Contract> contract = _context.Contracts.ToList();

            var list = (from allowance in _context.AllowanceSalaries
                        join position in _context.Positions on allowance.PositionId equals position.PositionId
                        join emp_pos in _context.Employee_Positions on position.PositionId equals emp_pos.PositionId
                        where emp_pos.EmployeeId == EmployeeId

                        select new
                        {
                            AllowanceSalaryId = allowance.AllowanceSalaryId,
                            AllowanceSalaryName = allowance.AllowanceSalaryName,
                        }).ToList();

            return Json(list);
        }

        // get data for Chart to show total salary of company
        public JsonResult GetReportByYear(int year)
        {
            if (year == 0)
            {
                DateTime localDate = DateTime.Now;
                year = localDate.Year;
            }
            var salaryQuery = from salary in _context.Salaries select salary;
            salaryQuery = salaryQuery.Where(emp => emp.SalaryDate.Year.Equals(year));

            double month1 = 0.0;
            double month2 = 0.0;
            double month3 = 0.0;
            double month4 = 0.0;
            double month5 = 0.0;
            double month6 = 0.0;
            double month7 = 0.0;
            double month8 = 0.0;
            double month9 = 0.0;
            double month10 = 0.0;
            double month11 = 0.0;
            double month12 = 0.0;

            foreach (var item in salaryQuery)
            {
                switch (item.SalaryDate.Month)
                {
                    case 1:
                        month1 += item.TotalSalary;
                        break;
                    case 2:
                        month2 += item.TotalSalary;
                        break;
                    case 3:
                        month3 += item.TotalSalary;
                        break;
                    case 4:
                        month4 += item.TotalSalary;
                        break;
                    case 5:
                        month5 += item.TotalSalary;
                        break;
                    case 6:
                        month6 += item.TotalSalary;
                        break;
                    case 7:
                        month7 += item.TotalSalary;
                        break;
                    case 8:
                        month8 += item.TotalSalary;
                        break;
                    case 9:
                        month9 += item.TotalSalary;
                        break;
                    case 10:
                        month10 += item.TotalSalary;
                        break;
                    case 11:
                        month11 += item.TotalSalary;
                        break;
                    case 12:
                        month12 += item.TotalSalary;
                        break;
                }
            }

            var listTotalSalaryMonths = new System.Collections.ArrayList();
            listTotalSalaryMonths.Add(month1);
            listTotalSalaryMonths.Add(month2);
            listTotalSalaryMonths.Add(month3);
            listTotalSalaryMonths.Add(month4);
            listTotalSalaryMonths.Add(month5);
            listTotalSalaryMonths.Add(month6);
            listTotalSalaryMonths.Add(month7);
            listTotalSalaryMonths.Add(month8);
            listTotalSalaryMonths.Add(month9);
            listTotalSalaryMonths.Add(month10);
            listTotalSalaryMonths.Add(month11);
            listTotalSalaryMonths.Add(month12);



            // Give a json object to FE
            ChartClass chartClass = new ChartClass(year, listTotalSalaryMonths);


            return Json(chartClass);
        }


    }
}


public class ChartClass
{

    public ChartClass(int year, System.Collections.ArrayList listTotalSalaryMonths1)
    {
        this.year = year;
        this.listTotalSalaryMonths = listTotalSalaryMonths1;
    }

    public int year { get; set; }
    public System.Collections.ArrayList listTotalSalaryMonths { get; set; }


}