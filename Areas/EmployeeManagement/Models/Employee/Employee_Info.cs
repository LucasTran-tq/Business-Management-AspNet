using System.Collections.Generic;

namespace App.Areas.EmployeeManagement.Models{
    public class Employee_Info{
        public Employee employee{set;get;}
        public List<Employee_Skill> employee_skills{get;set;}

        public List<Employee_Position> employee_positions{get;set;}

    }
}