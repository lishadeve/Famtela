using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Famtela.Application.Interfaces.Repositories;
using Famtela.Application.Interfaces.Services;
using Famtela.Domain.Entities.Chicken;
using Famtela.Shared.Wrapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace Famtela.Application.Features.Eggs.Queries.Export
{
    public class ExportEggsQuery : IRequest<Result<string>>
    {
        public string SearchString { get; set; }

        public ExportEggsQuery(string searchString = "")
        {
            SearchString = searchString;
        }
    }

    internal class ExportEggsQueryHandler : IRequestHandler<ExportEggsQuery, Result<string>>
    {
        private readonly IExcelService _excelService;
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IStringLocalizer<ExportEggsQueryHandler> _localizer;

        public ExportEggsQueryHandler(IExcelService excelService
            , IUnitOfWork<int> unitOfWork
            , IStringLocalizer<ExportEggsQueryHandler> localizer)
        {
            _excelService = excelService;
            _unitOfWork = unitOfWork;
            _localizer = localizer;
        }

        public async Task<Result<string>> Handle(ExportEggsQuery request, CancellationToken cancellationToken)
        {
            var eggs = await _unitOfWork.Repository<Egg>().Entities.ToListAsync(cancellationToken);
            var data = await _excelService.ExportAsync(eggs, mappers: new Dictionary<string, Func<Egg, object>>
            {
                { _localizer["Id"], item => item.Id },
                { _localizer["Sold"], item => item.Sold },
                { _localizer["Unit Price"], item => item.UnitPrice },
                { _localizer["Retained"], item => item.Retained },
                { _localizer["Rejected"], item => item.Rejected },
                { _localizer["Home"], item => item.Home },
                { _localizer["Transport"], item => item.Transport },
                { _localizer["Remarks"], item => item.Remarks }
            }, sheetName: _localizer["Eggs"]);

            return await Result<string>.SuccessAsync(data: data);
        }
    }
}
