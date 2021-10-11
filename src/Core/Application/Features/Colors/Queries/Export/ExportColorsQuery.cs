using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Famtela.Application.Extensions;
using Famtela.Application.Interfaces.Repositories;
using Famtela.Application.Interfaces.Services;
using Famtela.Application.Specifications.Dairy;
using Famtela.Domain.Entities.Dairy;
using Famtela.Shared.Wrapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace Famtela.Application.Features.Colors.Queries.Export
{
    public class ExportColorsQuery : IRequest<Result<string>>
    {
        public string SearchString { get; set; }

        public ExportColorsQuery(string searchString = "")
        {
            SearchString = searchString;
        }
    }

    internal class ExportColorsQueryHandler : IRequestHandler<ExportColorsQuery, Result<string>>
    {
        private readonly IExcelService _excelService;
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IStringLocalizer<ExportColorsQueryHandler> _localizer;

        public ExportColorsQueryHandler(IExcelService excelService
            , IUnitOfWork<int> unitOfWork
            , IStringLocalizer<ExportColorsQueryHandler> localizer)
        {
            _excelService = excelService;
            _unitOfWork = unitOfWork;
            _localizer = localizer;
        }

        public async Task<Result<string>> Handle(ExportColorsQuery request, CancellationToken cancellationToken)
        {
            var colorFilterSpec = new ColorFilterSpecification(request.SearchString);
            var colors = await _unitOfWork.Repository<Color>().Entities
                .Specify(colorFilterSpec)
                .ToListAsync(cancellationToken);
            var data = await _excelService.ExportAsync(colors, mappers: new Dictionary<string, Func<Color, object>>
            {
                { _localizer["Id"], item => item.Id },
                { _localizer["Name"], item => item.Name }
            }, sheetName: _localizer["Colors"]);

            return await Result<string>.SuccessAsync(data: data);
        }
    }
}
