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
using Famtela.Application.Features.Colors.Commands.AddEdit;
using Famtela.Domain.Entities.Dairy;

namespace Famtela.Application.Features.Colors.Commands.Import
{
    public partial class ImportColorsCommand : IRequest<Result<int>>
    {
        public UploadRequest UploadRequest { get; set; }
    }

    internal class ImportColorsCommandHandler : IRequestHandler<ImportColorsCommand, Result<int>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IExcelService _excelService;
        private readonly IMapper _mapper;
        private readonly IValidator<AddEditColorCommand> _addColorValidator;
        private readonly IStringLocalizer<ImportColorsCommandHandler> _localizer;

        public ImportColorsCommandHandler(
            IUnitOfWork<int> unitOfWork,
            IExcelService excelService,
            IMapper mapper,
            IValidator<AddEditColorCommand> addColorValidator,
            IStringLocalizer<ImportColorsCommandHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _excelService = excelService;
            _mapper = mapper;
            _addColorValidator = addColorValidator;
            _localizer = localizer;
        }

        public async Task<Result<int>> Handle(ImportColorsCommand request, CancellationToken cancellationToken)
        {
            var stream = new MemoryStream(request.UploadRequest.Data);
            var result = (await _excelService.ImportAsync(stream, mappers: new Dictionary<string, Func<DataRow, Color, object>>
            {
                { _localizer["Name"], (row,item) => item.Name = row[_localizer["Name"]].ToString() }
            }, _localizer["Colors"]));

            if (result.Succeeded)
            {
                var importedColors = result.Data;
                var errors = new List<string>();
                var errorsOccurred = false;
                foreach (var color in importedColors)
                {
                    var validationResult = await _addColorValidator.ValidateAsync(_mapper.Map<AddEditColorCommand>(color), cancellationToken);
                    if (validationResult.IsValid)
                    {
                        await _unitOfWork.Repository<Color>().AddAsync(color);
                    }
                    else
                    {
                        errorsOccurred = true;
                        errors.AddRange(validationResult.Errors.Select(e => $"{(!string.IsNullOrWhiteSpace(color.Name) ? $"{color.Name} - " : string.Empty)}{e.ErrorMessage}"));
                    }
                }

                if (errorsOccurred)
                {
                    return await Result<int>.FailAsync(errors);
                }

                await _unitOfWork.CommitAndRemoveCache(cancellationToken, ApplicationConstants.Cache.GetAllColorsCacheKey);
                return await Result<int>.SuccessAsync(result.Data.Count(), result.Messages[0]);
            }
            else
            {
                return await Result<int>.FailAsync(result.Messages);
            }
        }
    }
}