using Famtela.Application.Interfaces.Repositories;
using Famtela.Application.Interfaces.Services;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Famtela.Application.Extensions;
using Famtela.Shared.Wrapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Famtela.Application.Specifications.Dairy;
using Famtela.Domain.Entities.Dairy;

namespace Famtela.Application.Features.Cows.Queries.Export
{
    public class ExportCowsQuery : IRequest<Result<string>>
    {
        public string SearchString { get; set; }

        public ExportCowsQuery(string searchString = "")
        {
            SearchString = searchString;
        }
    }

    internal class ExportCowsQueryHandler : IRequestHandler<ExportCowsQuery, Result<string>>
    {
        private readonly IExcelService _excelService;
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IStringLocalizer<ExportCowsQueryHandler> _localizer;

        public ExportCowsQueryHandler(IExcelService excelService
            , IUnitOfWork<int> unitOfWork
            , IStringLocalizer<ExportCowsQueryHandler> localizer)
        {
            _excelService = excelService;
            _unitOfWork = unitOfWork;
            _localizer = localizer;
        }

        public async Task<Result<string>> Handle(ExportCowsQuery request, CancellationToken cancellationToken)
        {
            var cowFilterSpec = new CowFilterSpecification(request.SearchString);
            var cows = await _unitOfWork.Repository<Cow>().Entities
                .Specify(cowFilterSpec)
                .ToListAsync( cancellationToken);
            var data = await _excelService.ExportAsync(cows, mappers: new Dictionary<string, Func<Cow, object>>
            {
                { _localizer["Id"], item => item.Id },
                { _localizer["Ear Tag Number"], item => item.EarTagNumber },
                { _localizer["Dam"], item => item.Dam },
                { _localizer["Sire"], item => item.Sire },
                { _localizer["Date of Birth"], item => item.DateofBirth },
                { _localizer["Birth Weight"], item => item.BirthWeight },
            }, sheetName: _localizer["Cows"]);

            return await Result<string>.SuccessAsync(data: data);
        }
    }
}