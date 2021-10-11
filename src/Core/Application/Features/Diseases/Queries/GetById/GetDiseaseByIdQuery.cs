using AutoMapper;
using Famtela.Application.Interfaces.Repositories;
using Famtela.Domain.Entities.Chicken;
using Famtela.Shared.Wrapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Famtela.Application.Features.Diseases.Queries.GetById
{
    public class GetDiseaseByIdQuery : IRequest<Result<GetDiseaseByIdResponse>>
    {
        public int Id { get; set; }
    }

    internal class GetDiseaseByIdQueryHandler : IRequestHandler<GetDiseaseByIdQuery, Result<GetDiseaseByIdResponse>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;

        public GetDiseaseByIdQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<GetDiseaseByIdResponse>> Handle(GetDiseaseByIdQuery query, CancellationToken cancellationToken)
        {
            var disease = await _unitOfWork.Repository<Disease>().GetByIdAsync(query.Id);
            var mappedDisease = _mapper.Map<GetDiseaseByIdResponse>(disease);
            return await Result<GetDiseaseByIdResponse>.SuccessAsync(mappedDisease);
        }
    }
}