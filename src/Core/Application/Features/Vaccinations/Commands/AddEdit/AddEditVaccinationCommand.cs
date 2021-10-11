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
using Famtela.Domain.Entities.Chicken;

namespace Famtela.Application.Features.Vaccinations.Commands.AddEdit
{
    public partial class AddEditVaccinationCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
        [Required]
        public string Administration { get; set; }
        [Required]
        public string Remarks { get; set; }
        public int AgeId { get; set; }
        public int DiseaseId { get; set; }
    }

    internal class AddEditVaccinationCommandHandler : IRequestHandler<AddEditVaccinationCommand, Result<int>>
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IStringLocalizer<AddEditVaccinationCommandHandler> _localizer;

        public AddEditVaccinationCommandHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, IStringLocalizer<AddEditVaccinationCommandHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizer = localizer;
        }

        public async Task<Result<int>> Handle(AddEditVaccinationCommand command, CancellationToken cancellationToken)
        {
            if (await _unitOfWork.Repository<Vaccination>().Entities.Where(p => p.Id != command.Id)
                .AnyAsync(p => p.Remarks == command.Remarks, cancellationToken))
            {
                return await Result<int>.FailAsync(_localizer["Vaccination remark already exists."]);
            }

            if (command.Id == 0)
            {
                var vaccination = _mapper.Map<Vaccination>(command);
                await _unitOfWork.Repository<Vaccination>().AddAsync(vaccination);
                await _unitOfWork.Commit(cancellationToken);
                return await Result<int>.SuccessAsync(vaccination.Id, _localizer["Vaccination Record Saved"]);
            }
            else
            {
                var vaccination = await _unitOfWork.Repository<Vaccination>().GetByIdAsync(command.Id);
                if (vaccination != null)
                {
                    vaccination.Administration = command.Administration ?? vaccination.Administration;
                    vaccination.Remarks = command.Remarks ?? vaccination.Remarks;
                    vaccination.AgeId = (command.AgeId == 0) ? vaccination.AgeId : command.AgeId;
                    vaccination.DiseaseId = (command.DiseaseId == 0) ? vaccination.DiseaseId : command.DiseaseId;
                    await _unitOfWork.Repository<Vaccination>().UpdateAsync(vaccination);
                    await _unitOfWork.Commit(cancellationToken);
                    return await Result<int>.SuccessAsync(vaccination.Id, _localizer["Vaccination Record Updated"]);
                }
                else
                {
                    return await Result<int>.FailAsync(_localizer["Vaccination Record Not Found!"]);
                }
            }
        }
    }
}