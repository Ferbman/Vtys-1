using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ProjeYonetimi.Models
{
    [NotMapped]
    public class EmployeeViewModel:Employees
    {
        public IEnumerable<EmployeeViewModel> EmployeesViewModelList { get; set; }
        public IEnumerable<Employees> EmployeesList { get; set; }
        public IEnumerable<Duties> DutiesList { get; set; }

        public string project_name { get; set; }
        public string duty_name { get; set;}
    }
}