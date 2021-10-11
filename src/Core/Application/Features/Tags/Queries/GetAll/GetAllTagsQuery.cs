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

namespace Famtela.Application.Features.Tags.Queries.GetAll
{
    public class GetAllTagsQuery : IRequest<Result<List<GetAllTagsResponse>>>
    {
        public GetAllTagsQuery()
        {
        }
    }

    internal class GetAllTagsCachedQueryHandler : IRequestHandler<GetAllTagsQuery, Result<List<GetAllTagsResponse>>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IAppCache _cache;

        public GetAllTagsCachedQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, IAppCache cache)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _cache = cache;
        }

        public async Task<Result<List<GetAllTagsResponse>>> Handle(GetAllTagsQuery request, CancellationToken cancellationToken)
        {
            Func<Task<List<Tag>>> getAllTags = () => _unitOfWork.Repository<Tag>().GetAllAsync();
            var tagList = await _cache.GetOrAddAsync(ApplicationConstants.Cache.GetAllTagsCacheKey, getAllTags);
            var mappedTags = _mapper.Map<List<GetAllTagsResponse>>(tagList);
            return await Result<List<GetAllTagsResponse>>.SuccessAsync(mappedTags);
        }
    }
}