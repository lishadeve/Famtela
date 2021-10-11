namespace Famtela.Application.Requests.Dairy
{
    public class GetAllPagedMilksRequest : PagedRequest
    {
        public string SearchString { get; set; }
    }
}