using AutoMapper;
using Famtela.Application.Interfaces.Repositories;
using Famtela.Domain.Entities.Chicken;
using Famtela.Shared.Wrapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Famtela.Application.Features.Chicks.Queries.GetById
{
    public class GetChickByIdQuery : IRequest<Result<GetChickByIdResponse>>
    {
        public int Id { get; set; }
    }

    internal class GetChickByIdQueryHandler : IRequestHandler<GetChickByIdQuery, Result<GetChickByIdResponse>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;

        public GetChickByIdQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<GetChickByIdResponse>> Handle(GetChickByIdQuery query, CancellationToken cancellationToken)
        {
            var chick = await _unitOfWork.Repository<Chick>().GetByIdAsync(query.Id);
            var mappedChick = _mapper.Map<GetChickByIdResponse>(chick);
            return await Result<GetChickByIdResponse>.SuccessAsync(mappedChick);
        }
    }
}