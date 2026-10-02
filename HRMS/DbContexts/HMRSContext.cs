
using HRMS.Models;
using Microsoft.EntityFrameworkCore;

namespace HRMS.DbContexts
{
    public class HMRSContext : DbContext
    {
        public HMRSContext(DbContextOptions<HMRSContext> options) : base(options)
        {
            // Options تحتوي على 
            // 1) which database? (sql server, oracle, mysql....)
            // 2) Connection String (appsettings.json)
        }

        // Tables => DbSet
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Lookup> Lookups { get; set; }

         
            

    }
}
