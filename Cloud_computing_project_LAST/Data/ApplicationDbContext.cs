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
        public DbSet<Order> Order { get; set; }
        public DbSet<UserInfo> UsersInfo { get; set; }
        public DbSet<Cloud_computing_project_LAST.Models.Product>? Product { get; set; }
    }
}