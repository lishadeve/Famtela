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

namespace Famtela.Application.Features.Ages.Queries.GetAll
{
    public class GetAllAgesQuery : IRequest<Result<List<GetAllAgesResponse>>>
    {
        public GetAllAgesQuery()
        {
        }
    }

    internal class GetAllAgesCachedQueryHandler : IRequestHandler<GetAllAgesQuery, Result<List<GetAllAgesResponse>>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IAppCache _cache;

        public GetAllAgesCachedQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, IAppCache cache)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _cache = cache;
        }

        public async Task<Result<List<GetAllAgesResponse>>> Handle(GetAllAgesQuery request, CancellationToken cancellationToken)
        {
            Func<Task<List<Age>>> getAllAges = () => _unitOfWork.Repository<Age>().GetAllAsync();
            var ageList = await _cache.GetOrAddAsync(ApplicationConstants.Cache.GetAllAgesCacheKey, getAllAges);
            var mappedAges = _mapper.Map<List<GetAllAgesResponse>>(ageList);
            return await Result<List<GetAllAgesResponse>>.SuccessAsync(mappedAges);
        }
    }
}