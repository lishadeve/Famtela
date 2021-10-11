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
using Famtela.Application.Features.Layers.Commands.AddEdit;

namespace Famtela.Application.Features.Layers.Commands.Import
{
    public partial class ImportLayersCommand : IRequest<Result<int>>
    {
        public UploadRequest UploadRequest { get; set; }
    }

    internal class ImportLayersCommandHandler : IRequestHandler<ImportLayersCommand, Result<int>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IExcelService _excelService;
        private readonly IMapper _mapper;
        private readonly IValidator<AddEditLayerCommand> _addLayerValidator;
        private readonly IStringLocalizer<ImportLayersCommandHandler> _localizer;

        public ImportLayersCommandHandler(
            IUnitOfWork<int> unitOfWork,
            IExcelService excelService,
            IMapper mapper,
            IValidator<AddEditLayerCommand> addLayerValidator,
            IStringLocalizer<ImportLayersCommandHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _excelService = excelService;
            _mapper = mapper;
            _addLayerValidator = addLayerValidator;
            _localizer = localizer;
        }

        public async Task<Result<int>> Handle(ImportLayersCommand request, CancellationToken cancellationToken)
        {
            var stream = new MemoryStream(request.UploadRequest.Data);
            var result = (await _excelService.ImportAsync(stream, mappers: new Dictionary<string, Func<DataRow, Layer, object>>
            {
                { _localizer["Type of Feed"], (row,item) => item.TypeofFeed = row[_localizer["Type of Feed"]].ToString() },
                { _localizer["Disease"], (row,item) => item.Disease = row[_localizer["Disease"]].ToString() },
                { _localizer["Medication"], (row,item) => item.Medication = row[_localizer["Medication"]].ToString() },
                { _localizer["Vaccination"], (row,item) => item.Vaccination = row[_localizer["Vaccination"]].ToString() },
                { _localizer["Remarks"], (row,item) => item.Remarks = row[_localizer["Remarks"]].ToString() },
                { _localizer["Number of Birds"], (row,item) => item.NumberofBirds = int.TryParse(row[_localizer["Number of Birds"]].ToString(), out var numberofbirds) ? numberofbirds : 0 },
                { _localizer["Mortality"], (row,item) => item.Mortality = int.TryParse(row[_localizer["Mortality"]].ToString(), out var mortality) ? mortality : 0 },
                { _localizer["Eggs"], (row,item) => item.Eggs = int.TryParse(row[_localizer["Eggs"]].ToString(), out var eggs) ? eggs : 0 },
                { _localizer["Feed"], (row,item) => item.Feed = decimal.TryParse(row[_localizer["Feed"]].ToString(), out var feed) ? feed : 0 }
            }, _localizer["Layers"]));

            if (result.Succeeded)
            {
                var importedLayers = result.Data;
                var errors = new List<string>();
                var errorsOccurred = false;
                foreach (var layer in importedLayers)
                {
                    var validationResult = await _addLayerValidator.ValidateAsync(_mapper.Map<AddEditLayerCommand>(layer), cancellationToken);
                    if (validationResult.IsValid)
                    {
                        await _unitOfWork.Repository<Layer>().AddAsync(layer);
                    }
                    else
                    {
                        errorsOccurred = true;
                        errors.AddRange(validationResult.Errors.Select(e => $"{(!string.IsNullOrWhiteSpace(layer.TypeofFeed) ? $"{layer.TypeofFeed} - " : string.Empty)}{e.ErrorMessage}"));
                    }
                }

                if (errorsOccurred)
                {
                    return await Result<int>.FailAsync(errors);
                }

                await _unitOfWork.CommitAndRemoveCache(cancellationToken, ApplicationConstants.Cache.GetAllLayersCacheKey);
                return await Result<int>.SuccessAsync(result.Data.Count(), result.Messages[0]);
            }
            else
            {
                return await Result<int>.FailAsync(result.Messages);
            }
        }
    }
}