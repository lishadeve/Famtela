namespace Famtela.Application.Features.WeightEstimates.Queries.GetAll
{
    public class GetAllWeightEstimatesResponse
    {
        public int Id { get; set; }
        public decimal CM { get; set; }
        public decimal KG { get; set; }
        public string Remarks { get; set; }
    }
}