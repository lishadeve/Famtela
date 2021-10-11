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

namespace Famtela.Application.Features.Counties.Queries.GetAll
{
    public class GetAllCountiesQuery : IRequest<Result<List<GetAllCountiesResponse>>>
    {
        public GetAllCountiesQuery()
        {
        }
    }

    internal class GetAllCountiesCachedQueryHandler : IRequestHandler<GetAllCountiesQuery, Result<List<GetAllCountiesResponse>>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IAppCache _cache;

        public GetAllCountiesCachedQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, IAppCache cache)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _cache = cache;
        }

        public async Task<Result<List<GetAllCountiesResponse>>> Handle(GetAllCountiesQuery request, CancellationToken cancellationToken)
        {
            Func<Task<List<County>>> getAllCounties = () => _unitOfWork.Repository<County>().GetAllAsync();
            var countyList = await _cache.GetOrAddAsync(ApplicationConstants.Cache.GetAllCountiesCacheKey, getAllCounties);
            var mappedCounties = _mapper.Map<List<GetAllCountiesResponse>>(countyList);
            return await Result<List<GetAllCountiesResponse>>.SuccessAsync(mappedCounties);
        }
    }
}