using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Famtela.Application.Interfaces.Repositories;
using Famtela.Application.Interfaces.Services;
using Famtela.Domain.Entities.Dairy;
using Famtela.Shared.Wrapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace Famtela.Application.Features.WeightEstimates.Queries.Export
{
    public class ExportWeightEstimatesQuery : IRequest<Result<string>>
    {
        public string SearchString { get; set; }

        public ExportWeightEstimatesQuery(string searchString = "")
        {
            SearchString = searchString;
        }
    }

    internal class ExportWeightEstimatesQueryHandler : IRequestHandler<ExportWeightEstimatesQuery, Result<string>>
    {
        private readonly IExcelService _excelService;
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IStringLocalizer<ExportWeightEstimatesQueryHandler> _localizer;

        public ExportWeightEstimatesQueryHandler(IExcelService excelService
            , IUnitOfWork<int> unitOfWork
            , IStringLocalizer<ExportWeightEstimatesQueryHandler> localizer)
        {
            _excelService = excelService;
            _unitOfWork = unitOfWork;
            _localizer = localizer;
        }

        public async Task<Result<string>> Handle(ExportWeightEstimatesQuery request, CancellationToken cancellationToken)
        {
            var weightestimates = await _unitOfWork.Repository<WeightEstimate>().Entities.ToListAsync(cancellationToken);
            var data = await _excelService.ExportAsync(weightestimates, mappers: new Dictionary<string, Func<WeightEstimate, object>>
            {
                { _localizer["Id"], item => item.Id },
                { _localizer["CM"], item => item.CM },
                { _localizer["KG"], item => item.KG },
                { _localizer["Remarks"], item => item.Remarks }
            }, sheetName: _localizer["Weight Estimates"]);

            return await Result<string>.SuccessAsync(data: data);
        }
    }
}
