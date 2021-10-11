namespace Famtela.Application.Features.Growers.Queries.GetAll
{
    public class GetAllGrowersResponse
    {
        public int Id { get; set; }
        public int NumberofBirds { get; set; }
        public string Disease { get; set; }
        public int Mortality { get; set; }
        public string Vaccination { get; set; }
        public string Medication { get; set; }
        public decimal Feed { get; set; }
        public string TypeofFeed { get; set; }
        public int Eggs { get; set; }
        public string Remarks { get; set; }
    }
}