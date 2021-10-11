using AutoMapper;
using Famtela.Application.Interfaces.Repositories;
using Famtela.Domain.Entities.Chicken;
using Famtela.Shared.Wrapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Famtela.Application.Features.Eggs.Queries.GetById
{
    public class GetEggByIdQuery : IRequest<Result<GetEggByIdResponse>>
    {
        public int Id { get; set; }
    }

    internal class GetEggByIdQueryHandler : IRequestHandler<GetEggByIdQuery, Result<GetEggByIdResponse>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;

        public GetEggByIdQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<GetEggByIdResponse>> Handle(GetEggByIdQuery query, CancellationToken cancellationToken)
        {
            var egg = await _unitOfWork.Repository<Egg>().GetByIdAsync(query.Id);
            var mappedEgg = _mapper.Map<GetEggByIdResponse>(egg);
            return await Result<GetEggByIdResponse>.SuccessAsync(mappedEgg);
        }
    }
}