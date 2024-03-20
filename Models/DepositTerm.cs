namespace MVC_ASP.NET_Core_Learn.Models
{
    public class DepositTerm
    {
        // TODO: Можливо переробити вз'язок таблиці "Один термін -> Декілька шаблонів"
        public int Id { get; set; }
        public int NumberMonths { get; set; }
    }
}
