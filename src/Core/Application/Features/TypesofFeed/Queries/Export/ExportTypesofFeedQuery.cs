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

namespace Famtela.Application.Features.TypesofFeed.Queries.Export
{
    public class ExportTypesofFeedQuery : IRequest<Result<string>>
    {
        public string SearchString { get; set; }

        public ExportTypesofFeedQuery(string searchString = "")
        {
            SearchString = searchString;
        }
    }

    internal class ExportTypesofFeedQueryHandler : IRequestHandler<ExportTypesofFeedQuery, Result<string>>
    {
        private readonly IExcelService _excelService;
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IStringLocalizer<ExportTypesofFeedQueryHandler> _localizer;

        public ExportTypesofFeedQueryHandler(IExcelService excelService
            , IUnitOfWork<int> unitOfWork
            , IStringLocalizer<ExportTypesofFeedQueryHandler> localizer)
        {
            _excelService = excelService;
            _unitOfWork = unitOfWork;
            _localizer = localizer;
        }

        public async Task<Result<string>> Handle(ExportTypesofFeedQuery request, CancellationToken cancellationToken)
        {
            var typeoffeedFilterSpec = new TypeofFeedFilterSpecification(request.SearchString);
            var typesoffeed = await _unitOfWork.Repository<TypeofFeed>().Entities
                .Specify(typeoffeedFilterSpec)
                .ToListAsync(cancellationToken);
            var data = await _excelService.ExportAsync(typesoffeed, mappers: new Dictionary<string, Func<TypeofFeed, object>>
            {
                { _localizer["Id"], item => item.Id },
                { _localizer["Name"], item => item.Name }
            }, sheetName: _localizer["Types of Feed"]);

            return await Result<string>.SuccessAsync(data: data);
        }
    }
}
