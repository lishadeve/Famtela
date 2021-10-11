using Famtela.Application.Interfaces.Repositories;
using Famtela.Application.Interfaces.Services;
using Famtela.Domain.Entities.Catalog;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Famtela.Application.Extensions;
using Famtela.Application.Specifications.Catalog;
using Famtela.Shared.Wrapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Famtela.Domain.Entities.Chicken;
using Famtela.Application.Specifications.Chicken;

namespace Famtela.Application.Features.Vaccinations.Queries.Export
{
    public class ExportVaccinationsQuery : IRequest<Result<string>>
    {
        public string SearchString { get; set; }

        public ExportVaccinationsQuery(string searchString = "")
        {
            SearchString = searchString;
        }
    }

    internal class ExportVaccinationsQueryHandler : IRequestHandler<ExportVaccinationsQuery, Result<string>>
    {
        private readonly IExcelService _excelService;
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IStringLocalizer<ExportVaccinationsQueryHandler> _localizer;

        public ExportVaccinationsQueryHandler(IExcelService excelService
            , IUnitOfWork<int> unitOfWork
            , IStringLocalizer<ExportVaccinationsQueryHandler> localizer)
        {
            _excelService = excelService;
            _unitOfWork = unitOfWork;
            _localizer = localizer;
        }

        public async Task<Result<string>> Handle(ExportVaccinationsQuery request, CancellationToken cancellationToken)
        {
            var vaccinationFilterSpec = new VaccinationFilterSpecification(request.SearchString);
            var vaccinations = await _unitOfWork.Repository<Vaccination>().Entities
                .Specify(vaccinationFilterSpec)
                .ToListAsync( cancellationToken);
            var data = await _excelService.ExportAsync(vaccinations, mappers: new Dictionary<string, Func<Vaccination, object>>
            {
                { _localizer["Id"], item => item.Id },
                { _localizer["Administration"], item => item.Administration },
                { _localizer["Remarks"], item => item.Remarks }
            }, sheetName: _localizer["Vaccination Records"]);

            return await Result<string>.SuccessAsync(data: data);
        }
    }
}