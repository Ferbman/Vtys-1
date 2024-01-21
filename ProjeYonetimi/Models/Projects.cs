using System.ComponentModel.DataAnnotations;

namespace ProjeYonetimi.Models
{
    public class Projects
    {
        [Key]
        public int project_id { get; set; }
        public string project_name { get; set; }
        public string project_description { get; set; }
        public string start_date { get; set; }
        public string end_date { get; set; }
        public string project_status { get; set; }
        public int delay_time {  get; set; }
        public int manager_id { get; set; }


    }
}