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

namespace Famtela.Application.Features.Milks.Queries.GetAllPaged
{
    public class GetAllMilksQuery : IRequest<PaginatedResult<GetAllPagedMilksResponse>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string SearchString { get; set; }
        public string[] OrderBy { get; set; } // of the form fieldname [ascending|descending],fieldname [ascending|descending]...

        public GetAllMilksQuery(int pageNumber, int pageSize, string searchString, string orderBy)
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

    internal class GetAllMilksQueryHandler : IRequestHandler<GetAllMilksQuery, PaginatedResult<GetAllPagedMilksResponse>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;

        public GetAllMilksQueryHandler(IUnitOfWork<int> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<PaginatedResult<GetAllPagedMilksResponse>> Handle(GetAllMilksQuery request, CancellationToken cancellationToken)
        {
            Expression<Func<Milk, GetAllPagedMilksResponse>> expression = e => new GetAllPagedMilksResponse
            {
                Id = e.Id,
                Morning = e.Morning,
                Evening = e.Evening,
                Remarks = e.Remarks,
                Cow = e.Cow.EarTagNumber,
                CowId = e.CowId
            };
            var milkFilterSpec = new MilkFilterSpecification(request.SearchString);
            if (request.OrderBy?.Any() != true)
            {
                var data = await _unitOfWork.Repository<Milk>().Entities
                   .Specify(milkFilterSpec)
                   .Select(expression)
                   .ToPaginatedListAsync(request.PageNumber, request.PageSize);
                return data;
            }
            else
            {
                var ordering = string.Join(",", request.OrderBy); // of the form fieldname [ascending|descending], ...
                var data = await _unitOfWork.Repository<Milk>().Entities
                   .Specify(milkFilterSpec)
                   .OrderBy(ordering) // require system.linq.dynamic.core
                   .Select(expression)
                   .ToPaginatedListAsync(request.PageNumber, request.PageSize);
                return data;

            }
        }
    }
}