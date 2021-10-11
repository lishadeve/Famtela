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

namespace Famtela.Application.Features.Growers.Queries.GetAll
{
    public class GetAllGrowersQuery : IRequest<Result<List<GetAllGrowersResponse>>>
    {
        public GetAllGrowersQuery()
        {
        }
    }

    internal class GetAllGrowersCachedQueryHandler : IRequestHandler<GetAllGrowersQuery, Result<List<GetAllGrowersResponse>>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IAppCache _cache;

        public GetAllGrowersCachedQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, IAppCache cache)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _cache = cache;
        }

        public async Task<Result<List<GetAllGrowersResponse>>> Handle(GetAllGrowersQuery request, CancellationToken cancellationToken)
        {
            Func<Task<List<Grower>>> getAllGrowers = () => _unitOfWork.Repository<Grower>().GetAllAsync();
            var growerList = await _cache.GetOrAddAsync(ApplicationConstants.Cache.GetAllGrowersCacheKey, getAllGrowers);
            var mappedGrowers = _mapper.Map<List<GetAllGrowersResponse>>(growerList);
            return await Result<List<GetAllGrowersResponse>>.SuccessAsync(mappedGrowers);
        }
    }
}