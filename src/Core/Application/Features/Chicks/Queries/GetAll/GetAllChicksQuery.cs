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

namespace Famtela.Application.Features.Chicks.Queries.GetAll
{
    public class GetAllChicksQuery : IRequest<Result<List<GetAllChicksResponse>>>
    {
        public GetAllChicksQuery()
        {
        }
    }

    internal class GetAllChicksCachedQueryHandler : IRequestHandler<GetAllChicksQuery, Result<List<GetAllChicksResponse>>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IAppCache _cache;

        public GetAllChicksCachedQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, IAppCache cache)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _cache = cache;
        }

        public async Task<Result<List<GetAllChicksResponse>>> Handle(GetAllChicksQuery request, CancellationToken cancellationToken)
        {
            Func<Task<List<Chick>>> getAllChicks = () => _unitOfWork.Repository<Chick>().GetAllAsync();
            var chickList = await _cache.GetOrAddAsync(ApplicationConstants.Cache.GetAllChicksCacheKey, getAllChicks);
            var mappedChicks = _mapper.Map<List<GetAllChicksResponse>>(chickList);
            return await Result<List<GetAllChicksResponse>>.SuccessAsync(mappedChicks);
        }
    }
}