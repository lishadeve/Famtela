namespace Famtela.Application.Features.WeightEstimates.Queries.GetById
{
    public class GetWeightEstimateByIdResponse
    {
        public int Id { get; set; }
        public decimal CM { get; set; }
        public decimal KG { get; set; }
        public string Remarks { get; set; }
    }
}