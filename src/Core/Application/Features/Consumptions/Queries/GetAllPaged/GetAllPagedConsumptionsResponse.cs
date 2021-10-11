namespace Famtela.Application.Features.Consumptions.Queries.GetAllPaged
{
    public class GetAllPagedConsumptionsResponse
    {
        public int Id { get; set; }
        public decimal Grams { get; set; }
        public string Remarks { get; set; }
        public int AgeId { get; set; }
        public int TypeofFeedId { get; set; }
        public string Age { get; set; }
        public string TypeofFeed { get; set; }
    }
}