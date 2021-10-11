using AutoMapper;
using Famtela.Application.Interfaces.Repositories;
using Famtela.Domain.Entities.Chicken;
using Famtela.Domain.Entities.Dairy;
using Famtela.Shared.Wrapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Famtela.Application.Features.WeightEstimates.Queries.GetById
{
    public class GetWeightEstimateByIdQuery : IRequest<Result<GetWeightEstimateByIdResponse>>
    {
        public int Id { get; set; }
    }

    internal class GetWeightEstimateByIdQueryHandler : IRequestHandler<GetWeightEstimateByIdQuery, Result<GetWeightEstimateByIdResponse>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;

        public GetWeightEstimateByIdQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<GetWeightEstimateByIdResponse>> Handle(GetWeightEstimateByIdQuery query, CancellationToken cancellationToken)
        {
            var weightestimate = await _unitOfWork.Repository<WeightEstimate>().GetByIdAsync(query.Id);
            var mappedWeightEstimate = _mapper.Map<GetWeightEstimateByIdResponse>(weightestimate);
            return await Result<GetWeightEstimateByIdResponse>.SuccessAsync(mappedWeightEstimate);
        }
    }
}