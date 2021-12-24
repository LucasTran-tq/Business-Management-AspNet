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
using Microsoft.AspNetCore.Http;
using System.IO;
using Aspose.Pdf;
using System.Data;
using Aspose.Pdf.Text;

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
        [TempData]
        public string StatusEditMessage { get; set; }


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

         public IActionResult ExportPDF(){
            var document = new Document{ 
                PageInfo = new PageInfo{Margin= new MarginInfo(28,28,28,40)}
            };
            var pdfpage = document.Pages.Add();
            var textStamp = new Aspose.Pdf.TextStamp("AIT COMPANY")
            {
            Background = false,
            Opacity = 0.5,
            HorizontalAlignment = Aspose.Pdf.HorizontalAlignment.Center,
            VerticalAlignment = Aspose.Pdf.VerticalAlignment.Center,
            TextAlignment = Aspose.Pdf.HorizontalAlignment.Center
            };
            textStamp.TextState.Font = FontRepository.FindFont("Arial");
            textStamp.TextState.FontSize = 38.0f;
            textStamp.TextState.FontStyle = FontStyles.Bold;
            textStamp.TextState.FontStyle = FontStyles.Italic;
            textStamp.TextState.ForegroundColor = Aspose.Pdf.Color.FromRgb(System.Drawing.Color.FromArgb(0, 137, 237));
            Table table = new Table{
                ColumnWidths = "20% 25% 15% 15% 25%",
                DefaultCellPadding = new MarginInfo(10, 5, 10, 5),
                Border = new BorderInfo(BorderSide.All, .5f, Color.Black),
                DefaultCellBorder = new BorderInfo(BorderSide.All, .2f, Color.Black),
            };
            DateTime localdate = DateTime.Now;
            var empQuery = from emp in _context.Employees
                            .Include(s => s.Department)
                            .Include(s => s.Level)
                            select emp;

            DataTable table1 = new DataTable();
            table1.Columns.Add("Employee name", typeof(string));
            table1.Columns.Add("Department", typeof(string));
            table1.Columns.Add("Sex", typeof(string));
            table1.Columns.Add("Level", typeof(string));
            table1.Columns.Add("Day of birth", typeof(string));

            foreach(var emp in empQuery) {
                var row = table1.NewRow();
                row["Employee name"] = emp.EmployeeName;
                row["Department"] = emp.Department.DepartmentName;
                row["Sex"] = emp.Sex;
                row["Level"] = emp.Level.LevelName;
                row["Day of birth"] = emp.DOB.ToString("dd/MM/yyyy");
                table1.Rows.Add(row);
            }
            table.ImportDataTable(table1,true,0,0);
            Aspose.Pdf.Text.TextFragment text = new Aspose.Pdf.Text.TextFragment("List employees on " + localdate.ToString("dd/MM/yyyy")){
                HorizontalAlignment = Aspose.Pdf.HorizontalAlignment.Center,
                VerticalAlignment = Aspose.Pdf.VerticalAlignment.Center
            };
            text.TextState.FontSize = 16.0f;
            text.TextState.FontStyle = FontStyles.Bold;
            text.TextState.FontStyle = FontStyles.Italic;
            text.TextState.ForegroundColor = Aspose.Pdf.Color.FromRgb(System.Drawing.Color.FromArgb(121, 177, 0));
            text.Margin.Bottom = 20;
            document.Pages[1].AddStamp(textStamp);
            document.Pages[1].Paragraphs.Add(text);
            document.Pages[1].Paragraphs.Add(table);

            using (var streamout = new MemoryStream()){
                document.Save(streamout);
                return new FileContentResult(streamout.ToArray(), "application/pdf"){
                    FileDownloadName = "Employee.pdf"
                };
            }

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
        public async Task<IActionResult> Create([Bind("EmployeeId,EmployeeName,DepartmentId,LevelId,DOB,Sex,PlaceOfBirth,Address")] Employee employee, List<IFormFile> ImageByte)
        {
            DateTime localDate = DateTime.Now;


            // upload image and save in database by byte type
            if (ImageByte.Count == 1)
            {
                foreach (var item in ImageByte)
                {
                    if (item.Length > 0)
                    {
                        using (var stream = new MemoryStream())
                        {
                            await item.CopyToAsync(stream);
                            employee.ImageByte = stream.ToArray();
                        }
                    }
                }
            }

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
                    PositionId = 1,
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
        public async Task<IActionResult> Edit(int id, [Bind("EmployeeId,EmployeeName,DepartmentId,LevelId,DOB,Sex,PlaceOfBirth,Address")] Employee employee, List<IFormFile> ImageByte)
        {
            if (id != employee.EmployeeId)
            {
                return NotFound();
            }

            // set old image when user dont edit image
            if (ImageByte.Count == 0)
            {
                var oldImage = (from emp in _context.Employees
                                where emp.EmployeeId.Equals(id)
                                select emp.ImageByte).ToList();

                // database dont have image
                if (oldImage.Count == 0)
                {
                    employee.ImageByte = null;
                }

                // database have image
                else
                {
                    foreach (var item in oldImage)
                    {
                        employee.ImageByte = item;
                    }
                }
            }

            // upload image and save in database by byte type
            else if (ImageByte.Count == 1)
            {
                foreach (var item in ImageByte)
                {
                    if (item.Length > 0)
                    {
                        using (var stream = new MemoryStream())
                        {
                            await item.CopyToAsync(stream);
                            employee.ImageByte = stream.ToArray();
                        }
                    }
                }
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
                StatusEditMessage = "You have edited successfully!!!";
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
