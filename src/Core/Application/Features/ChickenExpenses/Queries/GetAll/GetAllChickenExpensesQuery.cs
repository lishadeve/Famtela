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

namespace Famtela.Application.Features.ChickenExpenses.Queries.GetAll
{
    public class GetAllChickenExpensesQuery : IRequest<Result<List<GetAllChickenExpensesResponse>>>
    {
        public GetAllChickenExpensesQuery()
        {
        }
    }

    internal class GetAllChickenExpensesCachedQueryHandler : IRequestHandler<GetAllChickenExpensesQuery, Result<List<GetAllChickenExpensesResponse>>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IAppCache _cache;

        public GetAllChickenExpensesCachedQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, IAppCache cache)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _cache = cache;
        }

        public async Task<Result<List<GetAllChickenExpensesResponse>>> Handle(GetAllChickenExpensesQuery request, CancellationToken cancellationToken)
        {
            Func<Task<List<ChickenExpense>>> getAllChickenExpenses = () => _unitOfWork.Repository<ChickenExpense>().GetAllAsync();
            var chickenexpenseList = await _cache.GetOrAddAsync(ApplicationConstants.Cache.GetAllChickenExpensesCacheKey, getAllChickenExpenses);
            var mappedChickenExpenses = _mapper.Map<List<GetAllChickenExpensesResponse>>(chickenexpenseList);
            return await Result<List<GetAllChickenExpensesResponse>>.SuccessAsync(mappedChickenExpenses);
        }
    }
}