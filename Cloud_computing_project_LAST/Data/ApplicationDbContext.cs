using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Cloud_computing_project_LAST.Models;

namespace Cloud_computing_project_LAST.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Cloud_computing_project_LAST.Models.Coffee>? Coffee { get; set; }
        public DbSet<Cloud_computing_project_LAST.Models.Cafe>? Cafe { get; set; }
    }
}