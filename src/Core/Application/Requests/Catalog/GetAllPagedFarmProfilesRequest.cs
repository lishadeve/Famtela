namespace Famtela.Application.Requests.Catalog
{
    public class GetAllPagedFarmProfilesRequest : PagedRequest
    {
        public string SearchString { get; set; }
    }
}