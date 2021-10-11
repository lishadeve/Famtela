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

namespace Famtela.Application.Features.Diseases.Queries.Export
{
    public class ExportDiseasesQuery : IRequest<Result<string>>
    {
        public string SearchString { get; set; }

        public ExportDiseasesQuery(string searchString = "")
        {
            SearchString = searchString;
        }
    }

    internal class ExportDiseasesQueryHandler : IRequestHandler<ExportDiseasesQuery, Result<string>>
    {
        private readonly IExcelService _excelService;
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IStringLocalizer<ExportDiseasesQueryHandler> _localizer;

        public ExportDiseasesQueryHandler(IExcelService excelService
            , IUnitOfWork<int> unitOfWork
            , IStringLocalizer<ExportDiseasesQueryHandler> localizer)
        {
            _excelService = excelService;
            _unitOfWork = unitOfWork;
            _localizer = localizer;
        }

        public async Task<Result<string>> Handle(ExportDiseasesQuery request, CancellationToken cancellationToken)
        {
            var diseaseFilterSpec = new DiseaseFilterSpecification(request.SearchString);
            var diseases = await _unitOfWork.Repository<Disease>().Entities
                .Specify(diseaseFilterSpec)
                .ToListAsync(cancellationToken);
            var data = await _excelService.ExportAsync(diseases, mappers: new Dictionary<string, Func<Disease, object>>
            {
                { _localizer["Id"], item => item.Id },
                { _localizer["Name"], item => item.Name }
            }, sheetName: _localizer["Diseases"]);

            return await Result<string>.SuccessAsync(data: data);
        }
    }
}
