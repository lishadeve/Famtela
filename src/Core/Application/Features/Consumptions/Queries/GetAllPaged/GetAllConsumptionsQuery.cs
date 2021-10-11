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
using Famtela.Domain.Entities.Chicken;
using Famtela.Application.Specifications.Chicken;

namespace Famtela.Application.Features.Consumptions.Queries.GetAllPaged
{
    public class GetAllConsumptionsQuery : IRequest<PaginatedResult<GetAllPagedConsumptionsResponse>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string SearchString { get; set; }
        public string[] OrderBy { get; set; } // of the form fieldname [ascending|descending],fieldname [ascending|descending]...

        public GetAllConsumptionsQuery(int pageNumber, int pageSize, string searchString, string orderBy)
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

    internal class GetAllConsumptionsQueryHandler : IRequestHandler<GetAllConsumptionsQuery, PaginatedResult<GetAllPagedConsumptionsResponse>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;

        public GetAllConsumptionsQueryHandler(IUnitOfWork<int> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<PaginatedResult<GetAllPagedConsumptionsResponse>> Handle(GetAllConsumptionsQuery request, CancellationToken cancellationToken)
        {
            Expression<Func<Consumption, GetAllPagedConsumptionsResponse>> expression = e => new GetAllPagedConsumptionsResponse
            {
                Id = e.Id,
                Grams = e.Grams,
                Remarks = e.Remarks,
                Age = e.Age.Name,
                TypeofFeed = e.TypeofFeed.Name,
                AgeId = e.AgeId,
                TypeofFeedId = e.TypeofFeedId
            };
            var consumptionFilterSpec = new ConsumptionFilterSpecification(request.SearchString);
            if (request.OrderBy?.Any() != true)
            {
                var data = await _unitOfWork.Repository<Consumption>().Entities
                   .Specify(consumptionFilterSpec)
                   .Select(expression)
                   .ToPaginatedListAsync(request.PageNumber, request.PageSize);
                return data;
            }
            else
            {
                var ordering = string.Join(",", request.OrderBy); // of the form fieldname [ascending|descending], ...
                var data = await _unitOfWork.Repository<Consumption>().Entities
                   .Specify(consumptionFilterSpec)
                   .OrderBy(ordering) // require system.linq.dynamic.core
                   .Select(expression)
                   .ToPaginatedListAsync(request.PageNumber, request.PageSize);
                return data;

            }
        }
    }
}