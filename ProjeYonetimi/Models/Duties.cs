using System.ComponentModel.DataAnnotations;

namespace ProjeYonetimi.Models
{
    public class Duties
    {
        [Key]
        public int duty_id { get; set; }
        public string duty_name { get; set; }
        public int project_id { get; set; }
    }
}