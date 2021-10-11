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

namespace Famtela.Application.Features.Eggs.Queries.GetAll
{
    public class GetAllEggsQuery : IRequest<Result<List<GetAllEggsResponse>>>
    {
        public GetAllEggsQuery()
        {
        }
    }

    internal class GetAllEggsCachedQueryHandler : IRequestHandler<GetAllEggsQuery, Result<List<GetAllEggsResponse>>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IAppCache _cache;

        public GetAllEggsCachedQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, IAppCache cache)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _cache = cache;
        }

        public async Task<Result<List<GetAllEggsResponse>>> Handle(GetAllEggsQuery request, CancellationToken cancellationToken)
        {
            Func<Task<List<Egg>>> getAllEggs = () => _unitOfWork.Repository<Egg>().GetAllAsync();
            var eggList = await _cache.GetOrAddAsync(ApplicationConstants.Cache.GetAllEggsCacheKey, getAllEggs);
            var mappedEggs = _mapper.Map<List<GetAllEggsResponse>>(eggList);
            return await Result<List<GetAllEggsResponse>>.SuccessAsync(mappedEggs);
        }
    }
}