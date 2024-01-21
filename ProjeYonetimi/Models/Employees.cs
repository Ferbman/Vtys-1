using System;
using System.ComponentModel.DataAnnotations;

namespace ProjeYonetimi.Models
{
    public class Employees
    {
        [Key]
        public string tc { get; set; }
        public string name { get; set; }
        public string surname { get; set; }
        public string email { get; set; }
        public string phone_num { get; set; }
        public int duty_id { get; set; }
    }
}



