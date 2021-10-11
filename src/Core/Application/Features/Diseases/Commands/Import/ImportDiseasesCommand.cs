using Famtela.Application.Interfaces.Repositories;
using Famtela.Application.Interfaces.Services;
using Famtela.Application.Requests;
using Famtela.Domain.Entities.Catalog;
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
using Famtela.Application.Features.Diseases.Commands.AddEdit;
using Famtela.Domain.Entities.Chicken;

namespace Famtela.Application.Features.Diseases.Commands.Import
{
    public partial class ImportDiseasesCommand : IRequest<Result<int>>
    {
        public UploadRequest UploadRequest { get; set; }
    }

    internal class ImportDiseasesCommandHandler : IRequestHandler<ImportDiseasesCommand, Result<int>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IExcelService _excelService;
        private readonly IMapper _mapper;
        private readonly IValidator<AddEditDiseaseCommand> _addDiseaseValidator;
        private readonly IStringLocalizer<ImportDiseasesCommandHandler> _localizer;

        public ImportDiseasesCommandHandler(
            IUnitOfWork<int> unitOfWork,
            IExcelService excelService,
            IMapper mapper,
            IValidator<AddEditDiseaseCommand> addDiseaseValidator,
            IStringLocalizer<ImportDiseasesCommandHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _excelService = excelService;
            _mapper = mapper;
            _addDiseaseValidator = addDiseaseValidator;
            _localizer = localizer;
        }

        public async Task<Result<int>> Handle(ImportDiseasesCommand request, CancellationToken cancellationToken)
        {
            var stream = new MemoryStream(request.UploadRequest.Data);
            var result = (await _excelService.ImportAsync(stream, mappers: new Dictionary<string, Func<DataRow, Disease, object>>
            {
                { _localizer["Name"], (row,item) => item.Name = row[_localizer["Name"]].ToString() }
            }, _localizer["Diseases"]));

            if (result.Succeeded)
            {
                var importedDiseases = result.Data;
                var errors = new List<string>();
                var errorsOccurred = false;
                foreach (var disease in importedDiseases)
                {
                    var validationResult = await _addDiseaseValidator.ValidateAsync(_mapper.Map<AddEditDiseaseCommand>(disease), cancellationToken);
                    if (validationResult.IsValid)
                    {
                        await _unitOfWork.Repository<Disease>().AddAsync(disease);
                    }
                    else
                    {
                        errorsOccurred = true;
                        errors.AddRange(validationResult.Errors.Select(e => $"{(!string.IsNullOrWhiteSpace(disease.Name) ? $"{disease.Name} - " : string.Empty)}{e.ErrorMessage}"));
                    }
                }

                if (errorsOccurred)
                {
                    return await Result<int>.FailAsync(errors);
                }

                await _unitOfWork.CommitAndRemoveCache(cancellationToken, ApplicationConstants.Cache.GetAllDiseasesCacheKey);
                return await Result<int>.SuccessAsync(result.Data.Count(), result.Messages[0]);
            }
            else
            {
                return await Result<int>.FailAsync(result.Messages);
            }
        }
    }
}