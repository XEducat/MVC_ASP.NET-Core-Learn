using System.ComponentModel;


namespace MVC_ASP.NET_Core_Learn.Data.Enums
{
    public enum InterestRate
    {
		[Description("При поверненні")]
		WhenReturning,

		[Description("Щомісяця")]
		Monthly,

		[Description("Щорічно в кінці терміну, капіталізація")]
		Yearly
    }
}
