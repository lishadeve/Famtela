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
using Famtela.Application.Features.WeightEstimates.Commands.AddEdit;
using Famtela.Domain.Entities.Dairy;

namespace Famtela.Application.Features.WeightEstimates.Commands.Import
{
    public partial class ImportWeightEstimatesCommand : IRequest<Result<int>>
    {
        public UploadRequest UploadRequest { get; set; }
    }

    internal class ImportWeightEstimatesCommandHandler : IRequestHandler<ImportWeightEstimatesCommand, Result<int>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IExcelService _excelService;
        private readonly IMapper _mapper;
        private readonly IValidator<AddEditWeightEstimateCommand> _addWeightEstimateValidator;
        private readonly IStringLocalizer<ImportWeightEstimatesCommandHandler> _localizer;

        public ImportWeightEstimatesCommandHandler(
            IUnitOfWork<int> unitOfWork,
            IExcelService excelService,
            IMapper mapper,
            IValidator<AddEditWeightEstimateCommand> addWeightEstimateValidator,
            IStringLocalizer<ImportWeightEstimatesCommandHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _excelService = excelService;
            _mapper = mapper;
            _addWeightEstimateValidator = addWeightEstimateValidator;
            _localizer = localizer;
        }

        public async Task<Result<int>> Handle(ImportWeightEstimatesCommand request, CancellationToken cancellationToken)
        {
            var stream = new MemoryStream(request.UploadRequest.Data);
            var result = (await _excelService.ImportAsync(stream, mappers: new Dictionary<string, Func<DataRow, WeightEstimate, object>>
            {
                { _localizer["CM"], (row,item) => item.CM = decimal.TryParse(row[_localizer["CM"]].ToString(), out var cm) ? cm : 0 },
                { _localizer["KG"], (row,item) => item.KG = decimal.TryParse(row[_localizer["KG"]].ToString(), out var kg) ? kg : 0 },
                { _localizer["Remarks"], (row,item) => item.Remarks = row[_localizer["Remarks"]].ToString() }
            }, _localizer["Weight Estimates"]));

            if (result.Succeeded)
            {
                var importedWeightEstimates = result.Data;
                var errors = new List<string>();
                var errorsOccurred = false;
                foreach (var weightestimate in importedWeightEstimates)
                {
                    var validationResult = await _addWeightEstimateValidator.ValidateAsync(_mapper.Map<AddEditWeightEstimateCommand>(weightestimate), cancellationToken);
                    if (validationResult.IsValid)
                    {
                        await _unitOfWork.Repository<WeightEstimate>().AddAsync(weightestimate);
                    }
                    else
                    {
                        errorsOccurred = true;
                    }
                }

                if (errorsOccurred)
                {
                    return await Result<int>.FailAsync(errors);
                }

                await _unitOfWork.CommitAndRemoveCache(cancellationToken, ApplicationConstants.Cache.GetAllWeightEstimatesCacheKey);
                return await Result<int>.SuccessAsync(result.Data.Count(), result.Messages[0]);
            }
            else
            {
                return await Result<int>.FailAsync(result.Messages);
            }
        }
    }
}