using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Famtela.Application.Interfaces.Repositories;
using Famtela.Shared.Wrapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Localization;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Famtela.Domain.Entities.Dairy;
using System;

namespace Famtela.Application.Features.Cows.Commands.AddEdit
{
    public partial class AddEditCowCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
        [Required]
        public string EarTagNumber { get; set; }
        public DateTime? DateofBirth { get; set; }
        public decimal BirthWeight { get; set; }
        public string Sire { get; set; }
        public string Dam { get; set; }
        [Required]
        public int BreedId { get; set; }
        [Required]
        public int ColorId { get; set; }
        [Required]
        public int StatusId { get; set; }
        [Required]
        public int TagId { get; set; }
    }

    internal class AddEditCowCommandHandler : IRequestHandler<AddEditCowCommand, Result<int>>
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IStringLocalizer<AddEditCowCommandHandler> _localizer;

        public AddEditCowCommandHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, IStringLocalizer<AddEditCowCommandHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizer = localizer;
        }

        public async Task<Result<int>> Handle(AddEditCowCommand command, CancellationToken cancellationToken)
        {
            if (await _unitOfWork.Repository<Cow>().Entities.Where(p => p.Id != command.Id)
                .AnyAsync(p => p.EarTagNumber == command.EarTagNumber, cancellationToken))
            {
                return await Result<int>.FailAsync(_localizer["Ear Tag Number already exists."]);
            }

            if (command.Id == 0)
            {
                var cow = _mapper.Map<Cow>(command);
                await _unitOfWork.Repository<Cow>().AddAsync(cow);
                await _unitOfWork.Commit(cancellationToken);
                return await Result<int>.SuccessAsync(cow.Id, _localizer["Cow Record Saved"]);
            }
            else
            {
                var cow = await _unitOfWork.Repository<Cow>().GetByIdAsync(command.Id);
                if (cow != null)
                {
                    cow.EarTagNumber = command.EarTagNumber ?? cow.EarTagNumber;
                    cow.DateofBirth = command.DateofBirth ?? cow.DateofBirth;
                    cow.Dam = command.Dam ?? cow.Dam;
                    cow.Sire = command.Sire ?? cow.Sire;
                    cow.BirthWeight = (command.BirthWeight == 0) ? cow.BirthWeight : command.BirthWeight;
                    cow.BreedId = (command.BreedId == 0) ? cow.BreedId : command.BreedId;
                    cow.ColorId = (command.ColorId == 0) ? cow.ColorId : command.ColorId;
                    cow.StatusId = (command.StatusId == 0) ? cow.StatusId : command.StatusId;
                    cow.TagId = (command.TagId == 0) ? cow.TagId : command.TagId;
                    await _unitOfWork.Repository<Cow>().UpdateAsync(cow);
                    await _unitOfWork.Commit(cancellationToken);
                    return await Result<int>.SuccessAsync(cow.Id, _localizer["Cow Record Updated"]);
                }
                else
                {
                    return await Result<int>.FailAsync(_localizer["Cow Record Not Found!"]);
                }
            }
        }
    }
}