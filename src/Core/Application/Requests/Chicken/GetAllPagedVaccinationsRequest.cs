namespace Famtela.Application.Requests.Chicken
{
    public class GetAllPagedVaccinationsRequest : PagedRequest
    {
        public string SearchString { get; set; }
    }
}