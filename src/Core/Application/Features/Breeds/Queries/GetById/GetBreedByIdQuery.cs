using AutoMapper;
using Famtela.Application.Interfaces.Repositories;
using Famtela.Domain.Entities.Dairy;
using Famtela.Shared.Wrapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Famtela.Application.Features.Breeds.Queries.GetById
{
    public class GetBreedByIdQuery : IRequest<Result<GetBreedByIdResponse>>
    {
        public int Id { get; set; }
    }

    internal class GetBreedByIdQueryHandler : IRequestHandler<GetBreedByIdQuery, Result<GetBreedByIdResponse>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;

        public GetBreedByIdQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<GetBreedByIdResponse>> Handle(GetBreedByIdQuery query, CancellationToken cancellationToken)
        {
            var breed = await _unitOfWork.Repository<Breed>().GetByIdAsync(query.Id);
            var mappedBreed = _mapper.Map<GetBreedByIdResponse>(breed);
            return await Result<GetBreedByIdResponse>.SuccessAsync(mappedBreed);
        }
    }
}