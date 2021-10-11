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

namespace Famtela.Application.Features.Statuses.Queries.GetAll
{
    public class GetAllStatusesQuery : IRequest<Result<List<GetAllStatusesResponse>>>
    {
        public GetAllStatusesQuery()
        {
        }
    }

    internal class GetAllStatusesCachedQueryHandler : IRequestHandler<GetAllStatusesQuery, Result<List<GetAllStatusesResponse>>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IAppCache _cache;

        public GetAllStatusesCachedQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, IAppCache cache)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _cache = cache;
        }

        public async Task<Result<List<GetAllStatusesResponse>>> Handle(GetAllStatusesQuery request, CancellationToken cancellationToken)
        {
            Func<Task<List<Status>>> getAllStatuses = () => _unitOfWork.Repository<Status>().GetAllAsync();
            var statusList = await _cache.GetOrAddAsync(ApplicationConstants.Cache.GetAllStatusesCacheKey, getAllStatuses);
            var mappedStatuses = _mapper.Map<List<GetAllStatusesResponse>>(statusList);
            return await Result<List<GetAllStatusesResponse>>.SuccessAsync(mappedStatuses);
        }
    }
}