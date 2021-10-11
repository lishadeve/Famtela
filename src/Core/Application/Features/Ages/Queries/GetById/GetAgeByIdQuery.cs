using AutoMapper;
using Famtela.Application.Interfaces.Repositories;
using Famtela.Domain.Entities.Chicken;
using Famtela.Shared.Wrapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Famtela.Application.Features.Ages.Queries.GetById
{
    public class GetAgeByIdQuery : IRequest<Result<GetAgeByIdResponse>>
    {
        public int Id { get; set; }
    }

    internal class GetAgeByIdQueryHandler : IRequestHandler<GetAgeByIdQuery, Result<GetAgeByIdResponse>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;

        public GetAgeByIdQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<GetAgeByIdResponse>> Handle(GetAgeByIdQuery query, CancellationToken cancellationToken)
        {
            var age = await _unitOfWork.Repository<Age>().GetByIdAsync(query.Id);
            var mappedAge = _mapper.Map<GetAgeByIdResponse>(age);
            return await Result<GetAgeByIdResponse>.SuccessAsync(mappedAge);
        }
    }
}