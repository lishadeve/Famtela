using AutoMapper;
using Famtela.Application.Interfaces.Repositories;
using Famtela.Domain.Entities.Dairy;
using Famtela.Shared.Constants.Application;
using Famtela.Shared.Wrapper;
using LazyCache;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Famtela.Application.Features.WeightEstimates.Queries.GetAll
{
    public class GetAllWeightEstimatesQuery : IRequest<Result<List<GetAllWeightEstimatesResponse>>>
    {
        public GetAllWeightEstimatesQuery()
        {
        }
    }

    internal class GetAllWeightEstimatesCachedQueryHandler : IRequestHandler<GetAllWeightEstimatesQuery, Result<List<GetAllWeightEstimatesResponse>>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IAppCache _cache;

        public GetAllWeightEstimatesCachedQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, IAppCache cache)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _cache = cache;
        }

        public async Task<Result<List<GetAllWeightEstimatesResponse>>> Handle(GetAllWeightEstimatesQuery request, CancellationToken cancellationToken)
        {
            Func<Task<List<WeightEstimate>>> getAllWeightEstimates = () => _unitOfWork.Repository<WeightEstimate>().GetAllAsync();
            var weightestimateList = await _cache.GetOrAddAsync(ApplicationConstants.Cache.GetAllWeightEstimatesCacheKey, getAllWeightEstimates);
            var mappedWeightEstimates = _mapper.Map<List<GetAllWeightEstimatesResponse>>(weightestimateList);
            return await Result<List<GetAllWeightEstimatesResponse>>.SuccessAsync(mappedWeightEstimates);
        }
    }
}