using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MVC_ASP.NET_Core_Learn.Models
{
    public class Address
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Street { get; set; }   // Вулиця
        public string City { get; set; }     // Місто
        public string State { get; set; }    // Держава
    }
}