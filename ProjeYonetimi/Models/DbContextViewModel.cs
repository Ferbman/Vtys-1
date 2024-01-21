using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using static System.Net.Mime.MediaTypeNames;



namespace ProjeYonetimi.Models
{
    public class DbContextViewModel : DbContext

    {
        public DbContextViewModel() : base("name=ConnectionStringName")
        {

        }
        public DbSet<Employees> Employees { get; set; }
        public DbSet<Managers> Managers { get; set; }
        public DbSet<Projects> Projects { get; set; } 
        public DbSet<Duties> Duties { get; set; }

    }
}