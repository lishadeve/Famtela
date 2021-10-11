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
using Famtela.Application.Features.TypesofFarming.Commands.AddEdit;

namespace Famtela.Application.Features.TypesofFarming.Commands.Import
{
    public partial class ImportTypesofFarmingCommand : IRequest<Result<int>>
    {
        public UploadRequest UploadRequest { get; set; }
    }

    internal class ImportCountiesCommandHandler : IRequestHandler<ImportTypesofFarmingCommand, Result<int>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IExcelService _excelService;
        private readonly IMapper _mapper;
        private readonly IValidator<AddEditTypeofFarmingCommand> _addCountyValidator;
        private readonly IStringLocalizer<ImportCountiesCommandHandler> _localizer;

        public ImportCountiesCommandHandler(
            IUnitOfWork<int> unitOfWork,
            IExcelService excelService,
            IMapper mapper,
            IValidator<AddEditTypeofFarmingCommand> addCountyValidator,
            IStringLocalizer<ImportCountiesCommandHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _excelService = excelService;
            _mapper = mapper;
            _addCountyValidator = addCountyValidator;
            _localizer = localizer;
        }

        public async Task<Result<int>> Handle(ImportTypesofFarmingCommand request, CancellationToken cancellationToken)
        {
            var stream = new MemoryStream(request.UploadRequest.Data);
            var result = (await _excelService.ImportAsync(stream, mappers: new Dictionary<string, Func<DataRow, County, object>>
            {
                { _localizer["Name"], (row,item) => item.Name = row[_localizer["Name"]].ToString() }
            }, _localizer["Counties"]));

            if (result.Succeeded)
            {
                var importedCounties = result.Data;
                var errors = new List<string>();
                var errorsOccurred = false;
                foreach (var typeoffarming in importedCounties)
                {
                    var validationResult = await _addCountyValidator.ValidateAsync(_mapper.Map<AddEditTypeofFarmingCommand>(typeoffarming), cancellationToken);
                    if (validationResult.IsValid)
                    {
                        await _unitOfWork.Repository<County>().AddAsync(typeoffarming);
                    }
                    else
                    {
                        errorsOccurred = true;
                        errors.AddRange(validationResult.Errors.Select(e => $"{(!string.IsNullOrWhiteSpace(typeoffarming.Name) ? $"{typeoffarming.Name} - " : string.Empty)}{e.ErrorMessage}"));
                    }
                }

                if (errorsOccurred)
                {
                    return await Result<int>.FailAsync(errors);
                }

                await _unitOfWork.CommitAndRemoveCache(cancellationToken, ApplicationConstants.Cache.GetAllCountiesCacheKey);
                return await Result<int>.SuccessAsync(result.Data.Count(), result.Messages[0]);
            }
            else
            {
                return await Result<int>.FailAsync(result.Messages);
            }
        }
    }
}