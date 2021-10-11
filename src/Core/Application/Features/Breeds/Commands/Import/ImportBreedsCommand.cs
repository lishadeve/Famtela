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
using Famtela.Application.Features.Breeds.Commands.AddEdit;
using Famtela.Domain.Entities.Dairy;

namespace Famtela.Application.Features.Breeds.Commands.Import
{
    public partial class ImportBreedsCommand : IRequest<Result<int>>
    {
        public UploadRequest UploadRequest { get; set; }
    }

    internal class ImportBreedsCommandHandler : IRequestHandler<ImportBreedsCommand, Result<int>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IExcelService _excelService;
        private readonly IMapper _mapper;
        private readonly IValidator<AddEditBreedCommand> _addBreedValidator;
        private readonly IStringLocalizer<ImportBreedsCommandHandler> _localizer;

        public ImportBreedsCommandHandler(
            IUnitOfWork<int> unitOfWork,
            IExcelService excelService,
            IMapper mapper,
            IValidator<AddEditBreedCommand> addBreedValidator,
            IStringLocalizer<ImportBreedsCommandHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _excelService = excelService;
            _mapper = mapper;
            _addBreedValidator = addBreedValidator;
            _localizer = localizer;
        }

        public async Task<Result<int>> Handle(ImportBreedsCommand request, CancellationToken cancellationToken)
        {
            var stream = new MemoryStream(request.UploadRequest.Data);
            var result = (await _excelService.ImportAsync(stream, mappers: new Dictionary<string, Func<DataRow, Breed, object>>
            {
                { _localizer["Name"], (row,item) => item.Name = row[_localizer["Name"]].ToString() }
            }, _localizer["Breeds"]));

            if (result.Succeeded)
            {
                var importedBreeds = result.Data;
                var errors = new List<string>();
                var errorsOccurred = false;
                foreach (var breed in importedBreeds)
                {
                    var validationResult = await _addBreedValidator.ValidateAsync(_mapper.Map<AddEditBreedCommand>(breed), cancellationToken);
                    if (validationResult.IsValid)
                    {
                        await _unitOfWork.Repository<Breed>().AddAsync(breed);
                    }
                    else
                    {
                        errorsOccurred = true;
                        errors.AddRange(validationResult.Errors.Select(e => $"{(!string.IsNullOrWhiteSpace(breed.Name) ? $"{breed.Name} - " : string.Empty)}{e.ErrorMessage}"));
                    }
                }

                if (errorsOccurred)
                {
                    return await Result<int>.FailAsync(errors);
                }

                await _unitOfWork.CommitAndRemoveCache(cancellationToken, ApplicationConstants.Cache.GetAllBreedsCacheKey);
                return await Result<int>.SuccessAsync(result.Data.Count(), result.Messages[0]);
            }
            else
            {
                return await Result<int>.FailAsync(result.Messages);
            }
        }
    }
}