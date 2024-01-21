using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using ProjeYonetimi.Models;

namespace ProjeYonetimi.Models
{
[NotMapped]
    public class ProjectViewModel: Projects 
    {
        public IEnumerable<Projects> ProjectsList { get; set; }
        public IEnumerable<Duties>DutiesList { get; set; }

        public int duty_id {  get; set; }

        public string duty_name { get; set; }

    }
}