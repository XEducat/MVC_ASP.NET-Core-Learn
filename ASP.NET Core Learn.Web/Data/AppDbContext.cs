using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MVC_ASP.NET_Core_Learn.Models;

namespace MVC_ASP.NET_Core_Learn.Data
{
	public class AppDbContext : IdentityDbContext<AppUser>
	{
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

		public DbSet<AppUser> Users { get; set; }
        public DbSet<UserDeposit> UserDeposits { get; set; }
        public DbSet<DepositTemplate> DepositTemplates { get; set; }
        public DbSet<DepositTerm> DepositTerms { get; set; }
        public DbSet<Address> Addresses { get; set; }
    }
}
