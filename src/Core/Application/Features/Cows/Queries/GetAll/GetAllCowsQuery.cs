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

namespace Famtela.Application.Features.Cows.Queries.GetAll
{
    public class GetAllCowsQuery : IRequest<Result<List<GetAllCowsResponse>>>
    {
        public GetAllCowsQuery()
        {
        }
    }

    internal class GetAllCowsCachedQueryHandler : IRequestHandler<GetAllCowsQuery, Result<List<GetAllCowsResponse>>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IAppCache _cache;

        public GetAllCowsCachedQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, IAppCache cache)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _cache = cache;
        }

        public async Task<Result<List<GetAllCowsResponse>>> Handle(GetAllCowsQuery request, CancellationToken cancellationToken)
        {
            Func<Task<List<Cow>>> getAllCows = () => _unitOfWork.Repository<Cow>().GetAllAsync();
            var cowList = await _cache.GetOrAddAsync(ApplicationConstants.Cache.GetAllCowsCacheKey, getAllCows);
            var mappedCows = _mapper.Map<List<GetAllCowsResponse>>(cowList);
            return await Result<List<GetAllCowsResponse>>.SuccessAsync(mappedCows);
        }
    }
}