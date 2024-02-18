using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace MVC_ASP.NET_Core_Learn.Models
{
	public class AppUser : IdentityUser
	{
		public DateTime Birdthday { get; set; }

		[ForeignKey("Address")]
		public int AddressId { get; set; }
		public Address? Address { get; set; }
        public IEnumerable<Deposit>? Deposits { get; set; }
    }
}
