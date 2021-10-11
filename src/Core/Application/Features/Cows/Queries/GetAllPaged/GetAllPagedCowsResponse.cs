using System;

namespace Famtela.Application.Features.Cows.Queries.GetAllPaged
{
    public class GetAllPagedCowsResponse
    {
        public int Id { get; set; }
        public string EarTagNumber { get; set; }
        public DateTime? DateofBirth { get; set; }
        public decimal BirthWeight { get; set; }
        public string Sire { get; set; }
        public string Dam { get; set; }
        public int BreedId { get; set; }
        public int ColorId { get; set; }
        public int StatusId { get; set; }
        public int TagId { get; set; }
        public string Breed { get; set; }
        public string Color { get; set; }
        public string Status { get; set; }
        public string Tag { get; set; }
    }
}