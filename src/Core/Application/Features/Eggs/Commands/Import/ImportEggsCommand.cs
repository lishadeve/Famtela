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
using Famtela.Application.Features.Eggs.Commands.AddEdit;

namespace Famtela.Application.Features.Eggs.Commands.Import
{
    public partial class ImportEggsCommand : IRequest<Result<int>>
    {
        public UploadRequest UploadRequest { get; set; }
    }

    internal class ImportEggsCommandHandler : IRequestHandler<ImportEggsCommand, Result<int>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IExcelService _excelService;
        private readonly IMapper _mapper;
        private readonly IValidator<AddEditEggCommand> _addEggValidator;
        private readonly IStringLocalizer<ImportEggsCommandHandler> _localizer;

        public ImportEggsCommandHandler(
            IUnitOfWork<int> unitOfWork,
            IExcelService excelService,
            IMapper mapper,
            IValidator<AddEditEggCommand> addEggValidator,
            IStringLocalizer<ImportEggsCommandHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _excelService = excelService;
            _mapper = mapper;
            _addEggValidator = addEggValidator;
            _localizer = localizer;
        }

        public async Task<Result<int>> Handle(ImportEggsCommand request, CancellationToken cancellationToken)
        {
            var stream = new MemoryStream(request.UploadRequest.Data);
            var result = (await _excelService.ImportAsync(stream, mappers: new Dictionary<string, Func<DataRow, Egg, object>>
            {
                { _localizer["Sold"], (row,item) => item.Sold = int.TryParse(row[_localizer["Sold"]].ToString(), out var sold) ? sold : 0 },
                { _localizer["Unit Price"], (row,item) => item.UnitPrice = decimal.TryParse(row[_localizer["Unit Price"]].ToString(), out var unitprice) ? unitprice : 0 },
                { _localizer["Rejected"], (row,item) => item.Rejected = int.TryParse(row[_localizer["Rejected"]].ToString(), out var rejected) ? rejected : 0 },
                { _localizer["Retained"], (row,item) => item.Retained = int.TryParse(row[_localizer["Retained"]].ToString(), out var retained) ? retained : 0 },
                { _localizer["Home"], (row,item) => item.Home = int.TryParse(row[_localizer["Home"]].ToString(), out var home) ? home : 0 },
                { _localizer["Transport"], (row,item) => item.Transport = decimal.TryParse(row[_localizer["Transport"]].ToString(), out var transport) ? transport : 0 },
                { _localizer["Remarks"], (row,item) => item.Remarks = row[_localizer["Remarks"]].ToString() }
            }, _localizer["Chicken Expenses"]));

            if (result.Succeeded)
            {
                var importedEggs = result.Data;
                var errors = new List<string>();
                var errorsOccurred = false;
                foreach (var egg in importedEggs)
                {
                    var validationResult = await _addEggValidator.ValidateAsync(_mapper.Map<AddEditEggCommand>(egg), cancellationToken);
                    if (validationResult.IsValid)
                    {
                        await _unitOfWork.Repository<Egg>().AddAsync(egg);
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

                await _unitOfWork.CommitAndRemoveCache(cancellationToken, ApplicationConstants.Cache.GetAllEggsCacheKey);
                return await Result<int>.SuccessAsync(result.Data.Count(), result.Messages[0]);
            }
            else
            {
                return await Result<int>.FailAsync(result.Messages);
            }
        }
    }
}