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

namespace Famtela.Application.Features.TypesofFeed.Queries.GetAll
{
    public class GetAllTypesofFeedQuery : IRequest<Result<List<GetAllTypesofFeedResponse>>>
    {
        public GetAllTypesofFeedQuery()
        {
        }
    }

    internal class GetAllTypesofFeedCachedQueryHandler : IRequestHandler<GetAllTypesofFeedQuery, Result<List<GetAllTypesofFeedResponse>>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IAppCache _cache;

        public GetAllTypesofFeedCachedQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, IAppCache cache)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _cache = cache;
        }

        public async Task<Result<List<GetAllTypesofFeedResponse>>> Handle(GetAllTypesofFeedQuery request, CancellationToken cancellationToken)
        {
            Func<Task<List<TypeofFeed>>> getAllTypesofFeed = () => _unitOfWork.Repository<TypeofFeed>().GetAllAsync();
            var typeoffeedList = await _cache.GetOrAddAsync(ApplicationConstants.Cache.GetAllTypesofFeedCacheKey, getAllTypesofFeed);
            var mappedTypesofFeed = _mapper.Map<List<GetAllTypesofFeedResponse>>(typeoffeedList);
            return await Result<List<GetAllTypesofFeedResponse>>.SuccessAsync(mappedTypesofFeed);
        }
    }
}