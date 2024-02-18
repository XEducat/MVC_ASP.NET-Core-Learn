﻿using Microsoft.EntityFrameworkCore;
using MVC_ASP.NET_Core_Learn.Models;

namespace MVC_ASP.NET_Core_Learn.Data
{
	public class AppDbContext : DbContext
	{
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

		public DbSet<AppUser> Users { get; set; }

        public DbSet<Deposit> Deposits { get; set; }
        public DbSet<Address> Addresses { get; set; }
    }
}
