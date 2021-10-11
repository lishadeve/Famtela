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

namespace Famtela.Application.Features.Vaccinations.Queries.GetAllPaged
{
    public class GetAllVaccinationsQuery : IRequest<PaginatedResult<GetAllPagedVaccinationsResponse>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string SearchString { get; set; }
        public string[] OrderBy { get; set; } // of the form fieldname [ascending|descending],fieldname [ascending|descending]...

        public GetAllVaccinationsQuery(int pageNumber, int pageSize, string searchString, string orderBy)
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

    internal class GetAllVaccinationsQueryHandler : IRequestHandler<GetAllVaccinationsQuery, PaginatedResult<GetAllPagedVaccinationsResponse>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;

        public GetAllVaccinationsQueryHandler(IUnitOfWork<int> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<PaginatedResult<GetAllPagedVaccinationsResponse>> Handle(GetAllVaccinationsQuery request, CancellationToken cancellationToken)
        {
            Expression<Func<Vaccination, GetAllPagedVaccinationsResponse>> expression = e => new GetAllPagedVaccinationsResponse
            {
                Id = e.Id,
                Administration = e.Administration,
                Remarks = e.Remarks,
                Age = e.Age.Name,
                Disease = e.Disease.Name,
                AgeId = e.AgeId,
                DiseaseId = e.DiseaseId
            };
            var vaccinationFilterSpec = new VaccinationFilterSpecification(request.SearchString);
            if (request.OrderBy?.Any() != true)
            {
                var data = await _unitOfWork.Repository<Vaccination>().Entities
                   .Specify(vaccinationFilterSpec)
                   .Select(expression)
                   .ToPaginatedListAsync(request.PageNumber, request.PageSize);
                return data;
            }
            else
            {
                var ordering = string.Join(",", request.OrderBy); // of the form fieldname [ascending|descending], ...
                var data = await _unitOfWork.Repository<Vaccination>().Entities
                   .Specify(vaccinationFilterSpec)
                   .OrderBy(ordering) // require system.linq.dynamic.core
                   .Select(expression)
                   .ToPaginatedListAsync(request.PageNumber, request.PageSize);
                return data;

            }
        }
    }
}