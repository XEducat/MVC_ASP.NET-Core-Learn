using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MVC_ASP.NET_Core_Learn.Models
{
	public class User
	{
		//[Key]
		[Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }
		public string Mail { get; set; }
		public DateTime Birdthday { get; set; }
	}
}
