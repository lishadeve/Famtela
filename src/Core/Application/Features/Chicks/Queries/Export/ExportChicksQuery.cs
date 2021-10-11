using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Famtela.Application.Extensions;
using Famtela.Application.Interfaces.Repositories;
using Famtela.Application.Interfaces.Services;
using Famtela.Application.Specifications.Chicken;
using Famtela.Domain.Entities.Chicken;
using Famtela.Shared.Wrapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace Famtela.Application.Features.Chicks.Queries.Export
{
    public class ExportChicksQuery : IRequest<Result<string>>
    {
        public string SearchString { get; set; }

        public ExportChicksQuery(string searchString = "")
        {
            SearchString = searchString;
        }
    }

    internal class ExportChicksQueryHandler : IRequestHandler<ExportChicksQuery, Result<string>>
    {
        private readonly IExcelService _excelService;
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IStringLocalizer<ExportChicksQueryHandler> _localizer;

        public ExportChicksQueryHandler(IExcelService excelService
            , IUnitOfWork<int> unitOfWork
            , IStringLocalizer<ExportChicksQueryHandler> localizer)
        {
            _excelService = excelService;
            _unitOfWork = unitOfWork;
            _localizer = localizer;
        }

        public async Task<Result<string>> Handle(ExportChicksQuery request, CancellationToken cancellationToken)
        {
            var chickFilterSpec = new ChicksFilterSpecification(request.SearchString);
            var chicks = await _unitOfWork.Repository<Chick>().Entities
                .Specify(chickFilterSpec)
                .ToListAsync(cancellationToken);
            var data = await _excelService.ExportAsync(chicks, mappers: new Dictionary<string, Func<Chick, object>>
            {
                { _localizer["Id"], item => item.Id },
                { _localizer["Number of Birds"], item => item.NumberofBirds },
                { _localizer["Disease"], item => item.Disease },
                { _localizer["Medication"], item => item.Medication },
                { _localizer["Vaccination"], item => item.Vaccination },
                { _localizer["Remarks"], item => item.Remarks },
                { _localizer["Mortality"], item => item.Mortality },
                { _localizer["Feed"], item => item.Feed },
                { _localizer["Type of Feed"], item => item.TypeofFeed }
            }, sheetName: _localizer["Chicks"]);

            return await Result<string>.SuccessAsync(data: data);
        }
    }
}
