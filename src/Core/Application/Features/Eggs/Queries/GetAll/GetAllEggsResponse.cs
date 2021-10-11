namespace Famtela.Application.Features.Eggs.Queries.GetAll
{
    public class GetAllEggsResponse
    {
        public int Id { get; set; }
        public int Sold { get; set; }
        public decimal UnitPrice { get; set; }
        public int Retained { get; set; }
        public int Rejected { get; set; }
        public int Home { get; set; }
        public decimal Transport { get; set; }
        public string Remarks { get; set; }

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