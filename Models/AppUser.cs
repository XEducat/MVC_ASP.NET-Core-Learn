using Microsoft.AspNetCore.Identity;

namespace MVC_ASP.NET_Core_Learn.Models
{
	public class AppUser : IdentityUser
	{
		public string? Mail { get; set; }
		public DateTime Birdthday { get; set; }
		public int AddressId { get; set; }
		public Address? Address { get; set; }
        public IEnumerable<Deposit>? Deposits { get; set; }
    }
}
