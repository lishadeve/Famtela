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
using Famtela.Application.Features.Statuses.Commands.AddEdit;
using Famtela.Domain.Entities.Dairy;

namespace Famtela.Application.Features.Statuses.Commands.Import
{
    public partial class ImportStatusesCommand : IRequest<Result<int>>
    {
        public UploadRequest UploadRequest { get; set; }
    }

    internal class ImportStatusesCommandHandler : IRequestHandler<ImportStatusesCommand, Result<int>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IExcelService _excelService;
        private readonly IMapper _mapper;
        private readonly IValidator<AddEditStatusCommand> _addStatusValidator;
        private readonly IStringLocalizer<ImportStatusesCommandHandler> _localizer;

        public ImportStatusesCommandHandler(
            IUnitOfWork<int> unitOfWork,
            IExcelService excelService,
            IMapper mapper,
            IValidator<AddEditStatusCommand> addStatusValidator,
            IStringLocalizer<ImportStatusesCommandHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _excelService = excelService;
            _mapper = mapper;
            _addStatusValidator = addStatusValidator;
            _localizer = localizer;
        }

        public async Task<Result<int>> Handle(ImportStatusesCommand request, CancellationToken cancellationToken)
        {
            var stream = new MemoryStream(request.UploadRequest.Data);
            var result = (await _excelService.ImportAsync(stream, mappers: new Dictionary<string, Func<DataRow, Status, object>>
            {
                { _localizer["Name"], (row,item) => item.Name = row[_localizer["Name"]].ToString() }
            }, _localizer["Statuses"]));

            if (result.Succeeded)
            {
                var importedStatuses = result.Data;
                var errors = new List<string>();
                var errorsOccurred = false;
                foreach (var status in importedStatuses)
                {
                    var validationResult = await _addStatusValidator.ValidateAsync(_mapper.Map<AddEditStatusCommand>(status), cancellationToken);
                    if (validationResult.IsValid)
                    {
                        await _unitOfWork.Repository<Status>().AddAsync(status);
                    }
                    else
                    {
                        errorsOccurred = true;
                        errors.AddRange(validationResult.Errors.Select(e => $"{(!string.IsNullOrWhiteSpace(status.Name) ? $"{status.Name} - " : string.Empty)}{e.ErrorMessage}"));
                    }
                }

                if (errorsOccurred)
                {
                    return await Result<int>.FailAsync(errors);
                }

                await _unitOfWork.CommitAndRemoveCache(cancellationToken, ApplicationConstants.Cache.GetAllStatusesCacheKey);
                return await Result<int>.SuccessAsync(result.Data.Count(), result.Messages[0]);
            }
            else
            {
                return await Result<int>.FailAsync(result.Messages);
            }
        }
    }
}