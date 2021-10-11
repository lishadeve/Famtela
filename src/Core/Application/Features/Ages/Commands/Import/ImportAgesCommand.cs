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
using Famtela.Application.Features.Ages.Commands.AddEdit;
using Famtela.Domain.Entities.Chicken;

namespace Famtela.Application.Features.Ages.Commands.Import
{
    public partial class ImportAgesCommand : IRequest<Result<int>>
    {
        public UploadRequest UploadRequest { get; set; }
    }

    internal class ImportAgesCommandHandler : IRequestHandler<ImportAgesCommand, Result<int>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IExcelService _excelService;
        private readonly IMapper _mapper;
        private readonly IValidator<AddEditAgeCommand> _addAgeValidator;
        private readonly IStringLocalizer<ImportAgesCommandHandler> _localizer;

        public ImportAgesCommandHandler(
            IUnitOfWork<int> unitOfWork,
            IExcelService excelService,
            IMapper mapper,
            IValidator<AddEditAgeCommand> addAgeValidator,
            IStringLocalizer<ImportAgesCommandHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _excelService = excelService;
            _mapper = mapper;
            _addAgeValidator = addAgeValidator;
            _localizer = localizer;
        }

        public async Task<Result<int>> Handle(ImportAgesCommand request, CancellationToken cancellationToken)
        {
            var stream = new MemoryStream(request.UploadRequest.Data);
            var result = (await _excelService.ImportAsync(stream, mappers: new Dictionary<string, Func<DataRow, Age, object>>
            {
                { _localizer["Name"], (row,item) => item.Name = row[_localizer["Name"]].ToString() }
            }, _localizer["Ages"]));

            if (result.Succeeded)
            {
                var importedAges = result.Data;
                var errors = new List<string>();
                var errorsOccurred = false;
                foreach (var age in importedAges)
                {
                    var validationResult = await _addAgeValidator.ValidateAsync(_mapper.Map<AddEditAgeCommand>(age), cancellationToken);
                    if (validationResult.IsValid)
                    {
                        await _unitOfWork.Repository<Age>().AddAsync(age);
                    }
                    else
                    {
                        errorsOccurred = true;
                        errors.AddRange(validationResult.Errors.Select(e => $"{(!string.IsNullOrWhiteSpace(age.Name) ? $"{age.Name} - " : string.Empty)}{e.ErrorMessage}"));
                    }
                }

                if (errorsOccurred)
                {
                    return await Result<int>.FailAsync(errors);
                }

                await _unitOfWork.CommitAndRemoveCache(cancellationToken, ApplicationConstants.Cache.GetAllAgesCacheKey);
                return await Result<int>.SuccessAsync(result.Data.Count(), result.Messages[0]);
            }
            else
            {
                return await Result<int>.FailAsync(result.Messages);
            }
        }
    }
}