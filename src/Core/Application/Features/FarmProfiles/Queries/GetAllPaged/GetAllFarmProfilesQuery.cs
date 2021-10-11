using Famtela.Application.Extensions;
using Famtela.Application.Interfaces.Repositories;
using Famtela.Application.Specifications.Catalog;
using Famtela.Domain.Entities.Catalog;
using Famtela.Shared.Wrapper;
using MediatR;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;

namespace Famtela.Application.Features.FarmProfiles.Queries.GetAllPaged
{
    public class GetAllFarmProfilesQuery : IRequest<PaginatedResult<GetAllPagedFarmProfilesResponse>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string SearchString { get; set; }
        public string[] OrderBy { get; set; } // of the form fieldname [ascending|descending],fieldname [ascending|descending]...

        public GetAllFarmProfilesQuery(int pageNumber, int pageSize, string searchString, string orderBy)
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

    internal class GetAllFarmProfilesQueryHandler : IRequestHandler<GetAllFarmProfilesQuery, PaginatedResult<GetAllPagedFarmProfilesResponse>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;

        public GetAllFarmProfilesQueryHandler(IUnitOfWork<int> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<PaginatedResult<GetAllPagedFarmProfilesResponse>> Handle(GetAllFarmProfilesQuery request, CancellationToken cancellationToken)
        {
            Expression<Func<FarmProfile, GetAllPagedFarmProfilesResponse>> expression = e => new GetAllPagedFarmProfilesResponse
            {
                Id = e.Id,
                FarmName = e.FarmName,
                County = e.County.Name,
                TypeofFarming = e.TypeofFarming.Name,
                CountyId = e.CountyId,
                TypeofFarmingId = e.TypeofFarmingId
            };
            var farmprofileFilterSpec = new FarmProfileFilterSpecification(request.SearchString);
            if (request.OrderBy?.Any() != true)
            {
                var data = await _unitOfWork.Repository<FarmProfile>().Entities
                   .Specify(farmprofileFilterSpec)
                   .Select(expression)
                   .ToPaginatedListAsync(request.PageNumber, request.PageSize);
                return data;
            }
            else
            {
                var ordering = string.Join(",", request.OrderBy); // of the form fieldname [ascending|descending], ...
                var data = await _unitOfWork.Repository<FarmProfile>().Entities
                   .Specify(farmprofileFilterSpec)
                   .OrderBy(ordering) // require system.linq.dynamic.core
                   .Select(expression)
                   .ToPaginatedListAsync(request.PageNumber, request.PageSize);
                return data;

            }
        }
    }
}