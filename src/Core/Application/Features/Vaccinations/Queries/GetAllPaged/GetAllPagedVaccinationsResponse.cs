namespace Famtela.Application.Features.Vaccinations.Queries.GetAllPaged
{
    public class GetAllPagedVaccinationsResponse
    {
        public int Id { get; set; }
        public string Administration { get; set; }
        public string Remarks { get; set; }
        public int AgeId { get; set; }
        public int DiseaseId { get; set; }
        public string Age { get; set; }
        public string Disease { get; set; }
    }
}