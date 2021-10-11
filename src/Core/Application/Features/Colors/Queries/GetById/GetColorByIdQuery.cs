using AutoMapper;
using Famtela.Application.Interfaces.Repositories;
using Famtela.Domain.Entities.Dairy;
using Famtela.Shared.Wrapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Famtela.Application.Features.Colors.Queries.GetById
{
    public class GetColorByIdQuery : IRequest<Result<GetColorByIdResponse>>
    {
        public int Id { get; set; }
    }

    internal class GetColorByIdQueryHandler : IRequestHandler<GetColorByIdQuery, Result<GetColorByIdResponse>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;

        public GetColorByIdQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<GetColorByIdResponse>> Handle(GetColorByIdQuery query, CancellationToken cancellationToken)
        {
            var color = await _unitOfWork.Repository<Color>().GetByIdAsync(query.Id);
            var mappedColor = _mapper.Map<GetColorByIdResponse>(color);
            return await Result<GetColorByIdResponse>.SuccessAsync(mappedColor);
        }
    }
}