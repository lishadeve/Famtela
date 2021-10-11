namespace Famtela.Application.Features.Eggs.Queries.GetById
{
    public class GetEggByIdResponse
    {
        public int Id { get; set; }
        public int Sold { get; set; }
        public decimal UnitPrice { get; set; }
        public int Retained { get; set; }
        public int Rejected { get; set; }
        public int Home { get; set; }
        public decimal Transport { get; set; }

        public decimal TotalPrice
        {
            get
            {
                return Sold * UnitPrice;
            }
        }
        public decimal TotalValue
        {
            get
            {
                return TotalPrice - Transport;
            }
        }
    }
}