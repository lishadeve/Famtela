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

namespace Famtela.Application.Features.Layers.Queries.Export
{
    public class ExportLayersQuery : IRequest<Result<string>>
    {
        public string SearchString { get; set; }

        public ExportLayersQuery(string searchString = "")
        {
            SearchString = searchString;
        }
    }

    internal class ExportLayersQueryHandler : IRequestHandler<ExportLayersQuery, Result<string>>
    {
        private readonly IExcelService _excelService;
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IStringLocalizer<ExportLayersQueryHandler> _localizer;

        public ExportLayersQueryHandler(IExcelService excelService
            , IUnitOfWork<int> unitOfWork
            , IStringLocalizer<ExportLayersQueryHandler> localizer)
        {
            _excelService = excelService;
            _unitOfWork = unitOfWork;
            _localizer = localizer;
        }

        public async Task<Result<string>> Handle(ExportLayersQuery request, CancellationToken cancellationToken)
        {
            var layerFilterSpec = new LayersFilterSpecification(request.SearchString);
            var layers = await _unitOfWork.Repository<Layer>().Entities
                .Specify(layerFilterSpec)
                .ToListAsync(cancellationToken);
            var data = await _excelService.ExportAsync(layers, mappers: new Dictionary<string, Func<Layer, object>>
            {
                { _localizer["Id"], item => item.Id },
                { _localizer["Number of Birds"], item => item.NumberofBirds },
                { _localizer["Disease"], item => item.Disease },
                { _localizer["Medication"], item => item.Medication },
                { _localizer["Vaccination"], item => item.Vaccination },
                { _localizer["Remarks"], item => item.Remarks },
                { _localizer["Mortality"], item => item.Mortality },
                { _localizer["Feed"], item => item.Feed },
                { _localizer["Type of Feed"], item => item.TypeofFeed },
                { _localizer["Eggs"], item => item.Eggs }
            }, sheetName: _localizer["Layers"]);

            return await Result<string>.SuccessAsync(data: data);
        }
    }
}
