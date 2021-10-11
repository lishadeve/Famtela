using AutoMapper;
using Famtela.Application.Interfaces.Repositories;
using Famtela.Domain.Entities.Dairy;
using Famtela.Shared.Wrapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Famtela.Application.Features.DairyExpenses.Queries.GetById
{
    public class GetDairyExpenseByIdQuery : IRequest<Result<GetDairyExpenseByIdResponse>>
    {
        public int Id { get; set; }
    }

    internal class GetDairyExpenseByIdQueryHandler : IRequestHandler<GetDairyExpenseByIdQuery, Result<GetDairyExpenseByIdResponse>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;

        public GetDairyExpenseByIdQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<GetDairyExpenseByIdResponse>> Handle(GetDairyExpenseByIdQuery query, CancellationToken cancellationToken)
        {
            var dairyexpense = await _unitOfWork.Repository<DairyExpense>().GetByIdAsync(query.Id);
            var mappedDairyExpense = _mapper.Map<GetDairyExpenseByIdResponse>(dairyexpense);
            return await Result<GetDairyExpenseByIdResponse>.SuccessAsync(mappedDairyExpense);
        }
    }
}