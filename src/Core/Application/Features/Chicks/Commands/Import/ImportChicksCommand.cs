using Famtela.Application.Interfaces.Repositories;
using Famtela.Application.Interfaces.Services;
using Famtela.Application.Requests;
using Famtela.Shared.Constants.Application;
using Famtela.Shared.Wrapper;
using MediatR;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using Famtela.Domain.Entities.Chicken;
using Famtela.Application.Features.Chicks.Commands.AddEdit;

namespace Famtela.Application.Features.Chicks.Commands.Import
{
    public partial class ImportChicksCommand : IRequest<Result<int>>
    {
        public UploadRequest UploadRequest { get; set; }
    }

    internal class ImportChicksCommandHandler : IRequestHandler<ImportChicksCommand, Result<int>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IExcelService _excelService;
        private readonly IMapper _mapper;
        private readonly IValidator<AddEditChickCommand> _addChickValidator;
        private readonly IStringLocalizer<ImportChicksCommandHandler> _localizer;

        public ImportChicksCommandHandler(
            IUnitOfWork<int> unitOfWork,
            IExcelService excelService,
            IMapper mapper,
            IValidator<AddEditChickCommand> addChickValidator,
            IStringLocalizer<ImportChicksCommandHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _excelService = excelService;
            _mapper = mapper;
            _addChickValidator = addChickValidator;
            _localizer = localizer;
        }

        public async Task<Result<int>> Handle(ImportChicksCommand request, CancellationToken cancellationToken)
        {
            var stream = new MemoryStream(request.UploadRequest.Data);
            var result = (await _excelService.ImportAsync(stream, mappers: new Dictionary<string, Func<DataRow, Chick, object>>
            {
                { _localizer["Type of Feed"], (row,item) => item.TypeofFeed = row[_localizer["Type of Feed"]].ToString() },
                { _localizer["Disease"], (row,item) => item.Disease = row[_localizer["Disease"]].ToString() },
                { _localizer["Medication"], (row,item) => item.Medication = row[_localizer["Medication"]].ToString() },
                { _localizer["Vaccination"], (row,item) => item.Vaccination = row[_localizer["Vaccination"]].ToString() },
                { _localizer["Remarks"], (row,item) => item.Remarks = row[_localizer["Remarks"]].ToString() },
                { _localizer["Number of Birds"], (row,item) => item.NumberofBirds = int.TryParse(row[_localizer["Number of Birds"]].ToString(), out var numberofbirds) ? numberofbirds : 0 },
                { _localizer["Mortality"], (row,item) => item.Mortality = int.TryParse(row[_localizer["Mortality"]].ToString(), out var mortality) ? mortality : 0 },
                { _localizer["Feed"], (row,item) => item.Feed = decimal.TryParse(row[_localizer["Feed"]].ToString(), out var feed) ? feed : 0 }
            }, _localizer["Chicks"]));

            if (result.Succeeded)
            {
                var importedChicks = result.Data;
                var errors = new List<string>();
                var errorsOccurred = false;
                foreach (var chick in importedChicks)
                {
                    var validationResult = await _addChickValidator.ValidateAsync(_mapper.Map<AddEditChickCommand>(chick), cancellationToken);
                    if (validationResult.IsValid)
                    {
                        await _unitOfWork.Repository<Chick>().AddAsync(chick);
                    }
                    else
                    {
                        errorsOccurred = true;
                        errors.AddRange(validationResult.Errors.Select(e => $"{(!string.IsNullOrWhiteSpace(chick.TypeofFeed) ? $"{chick.TypeofFeed} - " : string.Empty)}{e.ErrorMessage}"));
                    }
                }

                if (errorsOccurred)
                {
                    return await Result<int>.FailAsync(errors);
                }

                await _unitOfWork.CommitAndRemoveCache(cancellationToken, ApplicationConstants.Cache.GetAllChicksCacheKey);
                return await Result<int>.SuccessAsync(result.Data.Count(), result.Messages[0]);
            }
            else
            {
                return await Result<int>.FailAsync(result.Messages);
            }
        }
    }
}