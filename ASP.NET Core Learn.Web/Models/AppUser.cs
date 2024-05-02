using Microsoft.AspNetCore.Identity;
using MVC_ASP.NET_Core_Learn.ViewModels;
using System.ComponentModel.DataAnnotations.Schema;

namespace MVC_ASP.NET_Core_Learn.Models
{
	public class AppUser : IdentityUser
	{
		public DateTime Birdthday { get; set; }

		[ForeignKey("Address")]
		public int AddressId { get; set; }
		public decimal Balance { get; set; }
        public List<UserDeposit> Deposits { get; set; }
        public Address? Address { get; set; }
    }
}
