namespace Famtela.Application.Features.DairyExpenses.Queries.GetById
{
    public class GetDairyExpenseByIdResponse
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
        public decimal UnitCost { get; set; }
        public decimal Transport { get; set; }
    }
}