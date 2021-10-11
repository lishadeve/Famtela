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

namespace Famtela.Application.Features.DairyExpenses.Queries.GetAll
{
    public class GetAllDairyExpensesQuery : IRequest<Result<List<GetAllDairyExpensesResponse>>>
    {
        public GetAllDairyExpensesQuery()
        {
        }
    }

    internal class GetAllDairyExpensesCachedQueryHandler : IRequestHandler<GetAllDairyExpensesQuery, Result<List<GetAllDairyExpensesResponse>>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IAppCache _cache;

        public GetAllDairyExpensesCachedQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, IAppCache cache)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _cache = cache;
        }

        public async Task<Result<List<GetAllDairyExpensesResponse>>> Handle(GetAllDairyExpensesQuery request, CancellationToken cancellationToken)
        {
            Func<Task<List<DairyExpense>>> getAllDairyExpenses = () => _unitOfWork.Repository<DairyExpense>().GetAllAsync();
            var dairyexpenseList = await _cache.GetOrAddAsync(ApplicationConstants.Cache.GetAllDairyExpensesCacheKey, getAllDairyExpenses);
            var mappedDairyExpenses = _mapper.Map<List<GetAllDairyExpensesResponse>>(dairyexpenseList);
            return await Result<List<GetAllDairyExpensesResponse>>.SuccessAsync(mappedDairyExpenses);
        }
    }
}