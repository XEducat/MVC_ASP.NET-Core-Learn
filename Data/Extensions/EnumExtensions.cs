using System.ComponentModel;
using System.Reflection;

namespace MVC_ASP.NET_Core_Learn.Data.Extensions
{
	public static class EnumExtensions
	{
		public static string GetDescription(this Enum value)
		{
			// Отримуємо тип перерахування
			Type type = value.GetType();

			// Отримуємо член перерахування за значенням
			MemberInfo[] memInfo = type.GetMember(value.ToString());

			if (memInfo.Length > 0)
			{
				// Отримуємо атрибут Description, якщо він існує
				object[] attrs = memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);

				if (attrs.Length > 0)
				{
					// Повертаємо опис з атрибута Description
					return ((DescriptionAttribute)attrs[0]).Description;
				}
			}

			// Якщо атрибут Description не знайдено, просто повертаємо рядок перерахування
			return value.ToString();
		}
	}
}
