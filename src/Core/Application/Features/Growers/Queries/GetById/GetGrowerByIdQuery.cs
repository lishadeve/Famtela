using AutoMapper;
using Famtela.Application.Interfaces.Repositories;
using Famtela.Domain.Entities.Chicken;
using Famtela.Shared.Wrapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Famtela.Application.Features.Growers.Queries.GetById
{
    public class GetGrowerByIdQuery : IRequest<Result<GetGrowerByIdResponse>>
    {
        public int Id { get; set; }
    }

    internal class GetGrowerByIdQueryHandler : IRequestHandler<GetGrowerByIdQuery, Result<GetGrowerByIdResponse>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;

        public GetGrowerByIdQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<GetGrowerByIdResponse>> Handle(GetGrowerByIdQuery query, CancellationToken cancellationToken)
        {
            var grower = await _unitOfWork.Repository<Grower>().GetByIdAsync(query.Id);
            var mappedGrower = _mapper.Map<GetGrowerByIdResponse>(grower);
            return await Result<GetGrowerByIdResponse>.SuccessAsync(mappedGrower);
        }
    }
}