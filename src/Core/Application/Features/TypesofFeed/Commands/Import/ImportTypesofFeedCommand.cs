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
using Famtela.Application.Features.TypesofFeed.Commands.AddEdit;
using Famtela.Domain.Entities.Dairy;
using Famtela.Domain.Entities.Chicken;

namespace Famtela.Application.Features.TypesofFeed.Commands.Import
{
    public partial class ImportTypesofFeedCommand : IRequest<Result<int>>
    {
        public UploadRequest UploadRequest { get; set; }
    }

    internal class ImportTypesofFeedCommandHandler : IRequestHandler<ImportTypesofFeedCommand, Result<int>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IExcelService _excelService;
        private readonly IMapper _mapper;
        private readonly IValidator<AddEditTypeofFeedCommand> _addTypeofFeedValidator;
        private readonly IStringLocalizer<ImportTypesofFeedCommandHandler> _localizer;

        public ImportTypesofFeedCommandHandler(
            IUnitOfWork<int> unitOfWork,
            IExcelService excelService,
            IMapper mapper,
            IValidator<AddEditTypeofFeedCommand> addTypeofFeedValidator,
            IStringLocalizer<ImportTypesofFeedCommandHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _excelService = excelService;
            _mapper = mapper;
            _addTypeofFeedValidator = addTypeofFeedValidator;
            _localizer = localizer;
        }

        public async Task<Result<int>> Handle(ImportTypesofFeedCommand request, CancellationToken cancellationToken)
        {
            var stream = new MemoryStream(request.UploadRequest.Data);
            var result = (await _excelService.ImportAsync(stream, mappers: new Dictionary<string, Func<DataRow, TypeofFeed, object>>
            {
                { _localizer["Name"], (row,item) => item.Name = row[_localizer["Name"]].ToString() }
            }, _localizer["Types of Feed"]));

            if (result.Succeeded)
            {
                var importedTypesofFeed = result.Data;
                var errors = new List<string>();
                var errorsOccurred = false;
                foreach (var typeoffeed in importedTypesofFeed)
                {
                    var validationResult = await _addTypeofFeedValidator.ValidateAsync(_mapper.Map<AddEditTypeofFeedCommand>(typeoffeed), cancellationToken);
                    if (validationResult.IsValid)
                    {
                        await _unitOfWork.Repository<TypeofFeed>().AddAsync(typeoffeed);
                    }
                    else
                    {
                        errorsOccurred = true;
                        errors.AddRange(validationResult.Errors.Select(e => $"{(!string.IsNullOrWhiteSpace(typeoffeed.Name) ? $"{typeoffeed.Name} - " : string.Empty)}{e.ErrorMessage}"));
                    }
                }

                if (errorsOccurred)
                {
                    return await Result<int>.FailAsync(errors);
                }

                await _unitOfWork.CommitAndRemoveCache(cancellationToken, ApplicationConstants.Cache.GetAllTypesofFeedCacheKey);
                return await Result<int>.SuccessAsync(result.Data.Count(), result.Messages[0]);
            }
            else
            {
                return await Result<int>.FailAsync(result.Messages);
            }
        }
    }
}