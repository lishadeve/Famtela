using AutoMapper;
using Famtela.Application.Interfaces.Repositories;
using Famtela.Domain.Entities.Catalog;
using Famtela.Shared.Wrapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Famtela.Application.Features.Counties.Queries.GetById
{
    public class GetCountyByIdQuery : IRequest<Result<GetCountyByIdResponse>>
    {
        public int Id { get; set; }
    }

    internal class GetCountyByIdQueryHandler : IRequestHandler<GetCountyByIdQuery, Result<GetCountyByIdResponse>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;

        public GetCountyByIdQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<GetCountyByIdResponse>> Handle(GetCountyByIdQuery query, CancellationToken cancellationToken)
        {
            var county = await _unitOfWork.Repository<County>().GetByIdAsync(query.Id);
            var mappedCounty = _mapper.Map<GetCountyByIdResponse>(county);
            return await Result<GetCountyByIdResponse>.SuccessAsync(mappedCounty);
        }
    }
}