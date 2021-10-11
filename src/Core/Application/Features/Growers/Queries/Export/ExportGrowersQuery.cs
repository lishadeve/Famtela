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

namespace Famtela.Application.Features.Growers.Queries.Export
{
    public class ExportGrowersQuery : IRequest<Result<string>>
    {
        public string SearchString { get; set; }

        public ExportGrowersQuery(string searchString = "")
        {
            SearchString = searchString;
        }
    }

    internal class ExportGrowersQueryHandler : IRequestHandler<ExportGrowersQuery, Result<string>>
    {
        private readonly IExcelService _excelService;
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IStringLocalizer<ExportGrowersQueryHandler> _localizer;

        public ExportGrowersQueryHandler(IExcelService excelService
            , IUnitOfWork<int> unitOfWork
            , IStringLocalizer<ExportGrowersQueryHandler> localizer)
        {
            _excelService = excelService;
            _unitOfWork = unitOfWork;
            _localizer = localizer;
        }

        public async Task<Result<string>> Handle(ExportGrowersQuery request, CancellationToken cancellationToken)
        {
            var growerFilterSpec = new GrowersFilterSpecification(request.SearchString);
            var growers = await _unitOfWork.Repository<Grower>().Entities
                .Specify(growerFilterSpec)
                .ToListAsync(cancellationToken);
            var data = await _excelService.ExportAsync(growers, mappers: new Dictionary<string, Func<Grower, object>>
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
            }, sheetName: _localizer["Growers"]);

            return await Result<string>.SuccessAsync(data: data);
        }
    }
}
