using AutoMapper;
using Famtela.Application.Interfaces.Repositories;
using Famtela.Domain.Entities.Dairy;
using Famtela.Shared.Wrapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Famtela.Application.Features.Tags.Queries.GetById
{
    public class GetTagByIdQuery : IRequest<Result<GetTagByIdResponse>>
    {
        public int Id { get; set; }
    }

    internal class GetTagByIdQueryHandler : IRequestHandler<GetTagByIdQuery, Result<GetTagByIdResponse>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;

        public GetTagByIdQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<GetTagByIdResponse>> Handle(GetTagByIdQuery query, CancellationToken cancellationToken)
        {
            var tag = await _unitOfWork.Repository<Tag>().GetByIdAsync(query.Id);
            var mappedTag = _mapper.Map<GetTagByIdResponse>(tag);
            return await Result<GetTagByIdResponse>.SuccessAsync(mappedTag);
        }
    }
}