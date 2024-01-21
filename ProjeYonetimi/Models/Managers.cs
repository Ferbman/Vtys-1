using System.ComponentModel.DataAnnotations;

namespace ProjeYonetimi.Models
{
    public class Managers
    {
        [Key]
        public int manager_id { get; set; }
        public string username { get; set; }
        public string name { get; set; }
        public string surname { get; set; }

        public string email { get; set; }

        public string password { get; set; }

        public string phone_num { get; set; }
    }
}