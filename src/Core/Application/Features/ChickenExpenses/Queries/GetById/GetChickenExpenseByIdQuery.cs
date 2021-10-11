using AutoMapper;
using Famtela.Application.Interfaces.Repositories;
using Famtela.Domain.Entities.Chicken;
using Famtela.Shared.Wrapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Famtela.Application.Features.ChickenExpenses.Queries.GetById
{
    public class GetChickenExpenseByIdQuery : IRequest<Result<GetChickenExpenseByIdResponse>>
    {
        public int Id { get; set; }
    }

    internal class GetChickenExpenseByIdQueryHandler : IRequestHandler<GetChickenExpenseByIdQuery, Result<GetChickenExpenseByIdResponse>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;

        public GetChickenExpenseByIdQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<GetChickenExpenseByIdResponse>> Handle(GetChickenExpenseByIdQuery query, CancellationToken cancellationToken)
        {
            var chickenexpense = await _unitOfWork.Repository<ChickenExpense>().GetByIdAsync(query.Id);
            var mappedChickenExpense = _mapper.Map<GetChickenExpenseByIdResponse>(chickenexpense);
            return await Result<GetChickenExpenseByIdResponse>.SuccessAsync(mappedChickenExpense);
        }
    }
}