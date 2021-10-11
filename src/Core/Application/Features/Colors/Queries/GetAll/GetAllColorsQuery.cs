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

namespace Famtela.Application.Features.Colors.Queries.GetAll
{
    public class GetAllColorsQuery : IRequest<Result<List<GetAllColorsResponse>>>
    {
        public GetAllColorsQuery()
        {
        }
    }

    internal class GetAllColorsCachedQueryHandler : IRequestHandler<GetAllColorsQuery, Result<List<GetAllColorsResponse>>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IAppCache _cache;

        public GetAllColorsCachedQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, IAppCache cache)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _cache = cache;
        }

        public async Task<Result<List<GetAllColorsResponse>>> Handle(GetAllColorsQuery request, CancellationToken cancellationToken)
        {
            Func<Task<List<Color>>> getAllColors = () => _unitOfWork.Repository<Color>().GetAllAsync();
            var colorList = await _cache.GetOrAddAsync(ApplicationConstants.Cache.GetAllColorsCacheKey, getAllColors);
            var mappedColors = _mapper.Map<List<GetAllColorsResponse>>(colorList);
            return await Result<List<GetAllColorsResponse>>.SuccessAsync(mappedColors);
        }
    }
}