using Famtela.Application.Extensions;
using Famtela.Application.Interfaces.Repositories;
using Famtela.Shared.Wrapper;
using MediatR;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using Famtela.Domain.Entities.Dairy;
using Famtela.Application.Specifications.Dairy;

namespace Famtela.Application.Features.Cows.Queries.GetAllPaged
{
    public class GetAllCowsQuery : IRequest<PaginatedResult<GetAllPagedCowsResponse>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string SearchString { get; set; }
        public string[] OrderBy { get; set; } // of the form fieldname [ascending|descending],fieldname [ascending|descending]...

        public GetAllCowsQuery(int pageNumber, int pageSize, string searchString, string orderBy)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
            SearchString = searchString;
            if (!string.IsNullOrWhiteSpace(orderBy))
            {
                OrderBy = orderBy.Split(',');
            }
        }
    }

    internal class GetAllCowsQueryHandler : IRequestHandler<GetAllCowsQuery, PaginatedResult<GetAllPagedCowsResponse>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;

        public GetAllCowsQueryHandler(IUnitOfWork<int> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<PaginatedResult<GetAllPagedCowsResponse>> Handle(GetAllCowsQuery request, CancellationToken cancellationToken)
        {
            Expression<Func<Cow, GetAllPagedCowsResponse>> expression = e => new GetAllPagedCowsResponse
            {
                Id = e.Id,
                EarTagNumber = e.EarTagNumber,
                DateofBirth = e.DateofBirth,
                BirthWeight = e.BirthWeight,
                Dam = e.Dam,
                Sire = e.Sire,
                Breed = e.Breed.Name,
                Color = e.Color.Name,
                Status = e.Status.Name,
                Tag = e.Tag.Name,
                BreedId = e.BreedId,
                ColorId = e.ColorId,
                StatusId = e.StatusId,
                TagId = e.TagId
            };
            var cowFilterSpec = new CowFilterSpecification(request.SearchString);
            if (request.OrderBy?.Any() != true)
            {
                var data = await _unitOfWork.Repository<Cow>().Entities
                   .Specify(cowFilterSpec)
                   .Select(expression)
                   .ToPaginatedListAsync(request.PageNumber, request.PageSize);
                return data;
            }
            else
            {
                var ordering = string.Join(",", request.OrderBy); // of the form fieldname [ascending|descending], ...
                var data = await _unitOfWork.Repository<Cow>().Entities
                   .Specify(cowFilterSpec)
                   .OrderBy(ordering) // require system.linq.dynamic.core
                   .Select(expression)
                   .ToPaginatedListAsync(request.PageNumber, request.PageSize);
                return data;

            }
        }
    }
}