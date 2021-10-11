using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Famtela.Application.Extensions;
using Famtela.Application.Interfaces.Repositories;
using Famtela.Application.Interfaces.Services;
using Famtela.Application.Specifications.Catalog;
using Famtela.Domain.Entities.Catalog;
using Famtela.Shared.Wrapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace Famtela.Application.Features.TypesofFarming.Queries.Export
{
    public class ExportTypesofFarmingQuery : IRequest<Result<string>>
    {
        public string SearchString { get; set; }

        public ExportTypesofFarmingQuery(string searchString = "")
        {
            SearchString = searchString;
        }
    }

    internal class ExportTypesofFarmingQueryHandler : IRequestHandler<ExportTypesofFarmingQuery, Result<string>>
    {
        private readonly IExcelService _excelService;
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IStringLocalizer<ExportTypesofFarmingQueryHandler> _localizer;

        public ExportTypesofFarmingQueryHandler(IExcelService excelService
            , IUnitOfWork<int> unitOfWork
            , IStringLocalizer<ExportTypesofFarmingQueryHandler> localizer)
        {
            _excelService = excelService;
            _unitOfWork = unitOfWork;
            _localizer = localizer;
        }

        public async Task<Result<string>> Handle(ExportTypesofFarmingQuery request, CancellationToken cancellationToken)
        {
            var typeoffarmingFilterSpec = new TypeofFarmingFilterSpecification(request.SearchString);
            var typesoffarming = await _unitOfWork.Repository<TypeofFarming>().Entities
                .Specify(typeoffarmingFilterSpec)
                .ToListAsync(cancellationToken);
            var data = await _excelService.ExportAsync(typesoffarming, mappers: new Dictionary<string, Func<TypeofFarming, object>>
            {
                { _localizer["Id"], item => item.Id },
                { _localizer["Name"], item => item.Name }
            }, sheetName: _localizer["Types of Farming"]);

            return await Result<string>.SuccessAsync(data: data);
        }
    }
}
