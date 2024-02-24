namespace MVC_ASP.NET_Core_Learn.Models
{
	public class ErrorViewModel
	{
		public string? RequestId { get; set; }

		public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);

		public List<string> Errors { get; set; }
	}
}
