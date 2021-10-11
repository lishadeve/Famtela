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

namespace Famtela.Application.Features.Diseases.Queries.GetAll
{
    public class GetAllDiseasesQuery : IRequest<Result<List<GetAllDiseasesResponse>>>
    {
        public GetAllDiseasesQuery()
        {
        }
    }

    internal class GetAllDiseasesCachedQueryHandler : IRequestHandler<GetAllDiseasesQuery, Result<List<GetAllDiseasesResponse>>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IAppCache _cache;

        public GetAllDiseasesCachedQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, IAppCache cache)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _cache = cache;
        }

        public async Task<Result<List<GetAllDiseasesResponse>>> Handle(GetAllDiseasesQuery request, CancellationToken cancellationToken)
        {
            Func<Task<List<Disease>>> getAllDiseases = () => _unitOfWork.Repository<Disease>().GetAllAsync();
            var diseaseList = await _cache.GetOrAddAsync(ApplicationConstants.Cache.GetAllDiseasesCacheKey, getAllDiseases);
            var mappedDiseases = _mapper.Map<List<GetAllDiseasesResponse>>(diseaseList);
            return await Result<List<GetAllDiseasesResponse>>.SuccessAsync(mappedDiseases);
        }
    }
}