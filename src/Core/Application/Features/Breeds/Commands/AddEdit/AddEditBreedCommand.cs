using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Famtela.Application.Interfaces.Repositories;
using Famtela.Shared.Wrapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Localization;
using Famtela.Shared.Constants.Application;
using Famtela.Domain.Entities.Dairy;

namespace Famtela.Application.Features.Breeds.Commands.AddEdit
{
    public partial class AddEditBreedCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
    }

    internal class AddEditBreedCommandHandler : IRequestHandler<AddEditBreedCommand, Result<int>>
    {
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<AddEditBreedCommandHandler> _localizer;
        private readonly IUnitOfWork<int> _unitOfWork;

        public AddEditBreedCommandHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, IStringLocalizer<AddEditBreedCommandHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizer = localizer;
        }

        public async Task<Result<int>> Handle(AddEditBreedCommand command, CancellationToken cancellationToken)
        {
            if (command.Id == 0)
            {
                var breed = _mapper.Map<Breed>(command);
                await _unitOfWork.Repository<Breed>().AddAsync(breed);
                await _unitOfWork.CommitAndRemoveCache(cancellationToken, ApplicationConstants.Cache.GetAllBreedsCacheKey);
                return await Result<int>.SuccessAsync(breed.Id, _localizer["Breed Saved"]);
            }
            else
            {
                var breed = await _unitOfWork.Repository<Breed>().GetByIdAsync(command.Id);
                if (breed != null)
                {
                    breed.Name = command.Name ?? breed.Name;
                    await _unitOfWork.Repository<Breed>().UpdateAsync(breed);
                    await _unitOfWork.CommitAndRemoveCache(cancellationToken, ApplicationConstants.Cache.GetAllBreedsCacheKey);
                    return await Result<int>.SuccessAsync(breed.Id, _localizer["Breed Updated"]);
                }
                else
                {
                    return await Result<int>.FailAsync(_localizer["Breed Not Found!"]);
                }
            }
        }
    }
}