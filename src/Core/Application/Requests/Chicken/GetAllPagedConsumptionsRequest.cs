namespace Famtela.Application.Requests.Chicken
{
    public class GetAllPagedConsumptionsRequest : PagedRequest
    {
        public string SearchString { get; set; }
    }
}