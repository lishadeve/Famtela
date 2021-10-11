namespace Famtela.Application.Features.Milks.Queries.GetAllPaged
{
    public class GetAllPagedMilksResponse
    {
        public int Id { get; set; }
        public decimal Morning { get; set; }
        public decimal Evening { get; set; }
        public string Remarks { get; set; }
        public int CowId { get; set; }
        public string Cow { get; set; }

        public decimal DailyProduction
        {
            get
            {
                return Morning + Evening;
            }
        }
    }
}