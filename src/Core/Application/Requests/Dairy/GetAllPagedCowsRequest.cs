namespace Famtela.Application.Requests.Dairy
{
    public class GetAllPagedCowsRequest : PagedRequest
    {
        public string SearchString { get; set; }
    }
}