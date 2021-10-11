namespace Famtela.Application.Features.FarmProfiles.Queries.GetAllPaged
{
    public class GetAllPagedFarmProfilesResponse
    {
        public int Id { get; set; }
        public string FarmName { get; set; }
        public string County { get; set; }
        public int CountyId { get; set; }
        public string TypeofFarming { get; set; }
        public int TypeofFarmingId { get; set; }
    }
}