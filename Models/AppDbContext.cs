using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASP.NET_CORE_MVC_EMP_MGMT.Models
{
    //use the derived class to interact with underlying DB
    public class AppDbContext : IdentityDbContext
    {
        //DbContextOptions to carry db connection string and specify the db provider
        //i.e use DbContextOptions class instance to pass config info to DbContext class
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            
        }
        //Dbset Property for each type of model class we have
        //Employee is model class and Employees is property name we gave
        public DbSet<Employee> Employees { get; set; }//so 'Employees' is going to be table name
        public DbSet<Project> Projects { get; set;  }
        public DbSet<Risk> Risks { get; set; }
        public DbSet<Issue> Issues { get; set; }
        public DbSet<Actiono> Actionos { get; set; }
        public DbSet<Register> Registers { get; set; }
        public DbSet<EditUpdateRisk>EditUpdateRisks { get; set; }
        public DbSet<EditUpdateIssue> EditUpdateIssues { get; set; }
        public DbSet<Schedule> Schedules { get; set; }
        //public DbSet<IdentityUser> AspNetUsers { get; set; }



        //seed initial data to Employees table
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); 
            //call Seed method in out extensioin class ModelBuilderExtensions.cs
            modelBuilder.Seed();
        }//end OnModelCreating method
    }//end AppDbContext class extending DbContext class
}
