using AutoMapper;
using Famtela.Application.Interfaces.Repositories;
using Famtela.Domain.Entities.Catalog;
using Famtela.Shared.Constants.Application;
using Famtela.Shared.Wrapper;
using LazyCache;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Famtela.Application.Features.TypesofFarming.Queries.GetAll
{
    public class GetAllTypesofFarmingQuery : IRequest<Result<List<GetAllTypesofFarmingResponse>>>
    {
        public GetAllTypesofFarmingQuery()
        {
        }
    }

    internal class GetAllTypesofFarmingCachedQueryHandler : IRequestHandler<GetAllTypesofFarmingQuery, Result<List<GetAllTypesofFarmingResponse>>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IAppCache _cache;

        public GetAllTypesofFarmingCachedQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, IAppCache cache)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _cache = cache;
        }

        public async Task<Result<List<GetAllTypesofFarmingResponse>>> Handle(GetAllTypesofFarmingQuery request, CancellationToken cancellationToken)
        {
            Func<Task<List<TypeofFarming>>> getAllTypesofFarming = () => _unitOfWork.Repository<TypeofFarming>().GetAllAsync();
            var typeoffarmingList = await _cache.GetOrAddAsync(ApplicationConstants.Cache.GetAllTypesofFarmingCacheKey, getAllTypesofFarming);
            var mappedTypesofFarming = _mapper.Map<List<GetAllTypesofFarmingResponse>>(typeoffarmingList);
            return await Result<List<GetAllTypesofFarmingResponse>>.SuccessAsync(mappedTypesofFarming);
        }
    }
}