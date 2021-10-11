using AutoMapper;
using Famtela.Application.Interfaces.Repositories;
using Famtela.Domain.Entities.Chicken;
using Famtela.Shared.Constants.Application;
using Famtela.Shared.Wrapper;
using LazyCache;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Famtela.Application.Features.Layers.Queries.GetAll
{
    public class GetAllLayersQuery : IRequest<Result<List<GetAllLayersResponse>>>
    {
        public GetAllLayersQuery()
        {
        }
    }

    internal class GetAllLayersCachedQueryHandler : IRequestHandler<GetAllLayersQuery, Result<List<GetAllLayersResponse>>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IAppCache _cache;

        public GetAllLayersCachedQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, IAppCache cache)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _cache = cache;
        }

        public async Task<Result<List<GetAllLayersResponse>>> Handle(GetAllLayersQuery request, CancellationToken cancellationToken)
        {
            Func<Task<List<Layer>>> getAllLayers = () => _unitOfWork.Repository<Layer>().GetAllAsync();
            var layerList = await _cache.GetOrAddAsync(ApplicationConstants.Cache.GetAllLayersCacheKey, getAllLayers);
            var mappedLayers = _mapper.Map<List<GetAllLayersResponse>>(layerList);
            return await Result<List<GetAllLayersResponse>>.SuccessAsync(mappedLayers);
        }
    }
}