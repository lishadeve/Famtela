using AutoMapper;
using Famtela.Application.Interfaces.Repositories;
using Famtela.Domain.Entities.Catalog;
using Famtela.Shared.Wrapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Famtela.Application.Features.TypesofFarming.Queries.GetById
{
    public class GetTypeofFarmingByIdQuery : IRequest<Result<GetTypeofFarmingByIdResponse>>
    {
        public int Id { get; set; }
    }

    internal class GetTypeofFarmingByIdQueryHandler : IRequestHandler<GetTypeofFarmingByIdQuery, Result<GetTypeofFarmingByIdResponse>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;

        public GetTypeofFarmingByIdQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<GetTypeofFarmingByIdResponse>> Handle(GetTypeofFarmingByIdQuery query, CancellationToken cancellationToken)
        {
            var typeoffarming = await _unitOfWork.Repository<TypeofFarming>().GetByIdAsync(query.Id);
            var mappedTypeofFarming = _mapper.Map<GetTypeofFarmingByIdResponse>(typeoffarming);
            return await Result<GetTypeofFarmingByIdResponse>.SuccessAsync(mappedTypeofFarming);
        }
    }
}