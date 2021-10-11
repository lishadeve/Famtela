using AutoMapper;
using Famtela.Application.Interfaces.Repositories;
using Famtela.Domain.Entities.Chicken;
using Famtela.Domain.Entities.Dairy;
using Famtela.Shared.Wrapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Famtela.Application.Features.Statuses.Queries.GetById
{
    public class GetStatusByIdQuery : IRequest<Result<GetStatusByIdResponse>>
    {
        public int Id { get; set; }
    }

    internal class GetStatusByIdQueryHandler : IRequestHandler<GetStatusByIdQuery, Result<GetStatusByIdResponse>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;

        public GetStatusByIdQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<GetStatusByIdResponse>> Handle(GetStatusByIdQuery query, CancellationToken cancellationToken)
        {
            var status = await _unitOfWork.Repository<Status>().GetByIdAsync(query.Id);
            var mappedStatus = _mapper.Map<GetStatusByIdResponse>(status);
            return await Result<GetStatusByIdResponse>.SuccessAsync(mappedStatus);
        }
    }
}