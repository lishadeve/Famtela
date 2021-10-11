using AutoMapper;
using Famtela.Application.Interfaces.Repositories;
using Famtela.Domain.Entities.Chicken;
using Famtela.Shared.Wrapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Famtela.Application.Features.TypesofFeed.Queries.GetById
{
    public class GetTypeofFeedByIdQuery : IRequest<Result<GetTypeofFeedByIdResponse>>
    {
        public int Id { get; set; }
    }

    internal class GetTypeofFeedByIdQueryHandler : IRequestHandler<GetTypeofFeedByIdQuery, Result<GetTypeofFeedByIdResponse>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;

        public GetTypeofFeedByIdQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<GetTypeofFeedByIdResponse>> Handle(GetTypeofFeedByIdQuery query, CancellationToken cancellationToken)
        {
            var typeoffeed = await _unitOfWork.Repository<TypeofFeed>().GetByIdAsync(query.Id);
            var mappedTypeofFeed = _mapper.Map<GetTypeofFeedByIdResponse>(typeoffeed);
            return await Result<GetTypeofFeedByIdResponse>.SuccessAsync(mappedTypeofFeed);
        }
    }
}