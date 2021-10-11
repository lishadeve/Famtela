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

namespace Famtela.Application.Features.Breeds.Queries.GetAll
{
    public class GetAllBreedsQuery : IRequest<Result<List<GetAllBreedsResponse>>>
    {
        public GetAllBreedsQuery()
        {
        }
    }

    internal class GetAllBreedsCachedQueryHandler : IRequestHandler<GetAllBreedsQuery, Result<List<GetAllBreedsResponse>>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IAppCache _cache;

        public GetAllBreedsCachedQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, IAppCache cache)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _cache = cache;
        }

        public async Task<Result<List<GetAllBreedsResponse>>> Handle(GetAllBreedsQuery request, CancellationToken cancellationToken)
        {
            Func<Task<List<Breed>>> getAllBreeds = () => _unitOfWork.Repository<Breed>().GetAllAsync();
            var breedList = await _cache.GetOrAddAsync(ApplicationConstants.Cache.GetAllBreedsCacheKey, getAllBreeds);
            var mappedBreeds = _mapper.Map<List<GetAllBreedsResponse>>(breedList);
            return await Result<List<GetAllBreedsResponse>>.SuccessAsync(mappedBreeds);
        }
    }
}