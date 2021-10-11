using AutoMapper;
using Famtela.Application.Interfaces.Repositories;
using Famtela.Domain.Entities.Chicken;
using Famtela.Shared.Wrapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Famtela.Application.Features.Layers.Queries.GetById
{
    public class GetLayerByIdQuery : IRequest<Result<GetLayerByIdResponse>>
    {
        public int Id { get; set; }
    }

    internal class GetLayerByIdQueryHandler : IRequestHandler<GetLayerByIdQuery, Result<GetLayerByIdResponse>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;

        public GetLayerByIdQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<GetLayerByIdResponse>> Handle(GetLayerByIdQuery query, CancellationToken cancellationToken)
        {
            var layer = await _unitOfWork.Repository<Layer>().GetByIdAsync(query.Id);
            var mappedLayer = _mapper.Map<GetLayerByIdResponse>(layer);
            return await Result<GetLayerByIdResponse>.SuccessAsync(mappedLayer);
        }
    }
}