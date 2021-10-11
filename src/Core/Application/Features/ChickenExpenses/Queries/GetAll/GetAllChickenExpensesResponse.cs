namespace Famtela.Application.Features.ChickenExpenses.Queries.GetAll
{
    public class GetAllChickenExpensesResponse
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
        public decimal UnitCost { get; set; }
        public decimal Transport { get; set; }

        public decimal TotalCost
        {
            get
            {
                return Quantity * UnitCost;
            }
        }
        public decimal TotalExpense
        {
            get
            {
                return TotalCost + Transport;
            }
        }
    }
}