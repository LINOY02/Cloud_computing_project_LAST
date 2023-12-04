using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Cloud_computing_project_LAST.Models;
using System;

namespace Cloud_computing_project_LAST.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Cafe> Cafe { get; set; }
       
        public DbSet<Cloud_computing_project_LAST.Models.UserInfo> UsersInfo { get; set; }
        public DbSet<Cloud_computing_project_LAST.Models.Product>? Product { get; set; }
        public DbSet<Cloud_computing_project_LAST.Models.CartItem>? CartItem { get; set; }
        public DbSet<Cloud_computing_project_LAST.Models.Cart>? Cart { get; set; }
        public DbSet<Cloud_computing_project_LAST.Models.Orderr>? Orderr { get; set; }
        public DbSet<Cloud_computing_project_LAST.Models.Menu>? Menu { get; set; }
        public DbSet<Cloud_computing_project_LAST.Models.Weather>? Weather { get; set; }
        public DbSet<Cloud_computing_project_LAST.Models.OrderItem>? OrderItem { get; set; }
        public DbSet<Cloud_computing_project_LAST.Models.Hebcal>? Hebcal { get; set; }

    }
}