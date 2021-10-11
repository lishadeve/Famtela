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

namespace Famtela.Application.Features.Breeds.Queries.Export
{
    public class ExportBreedsQuery : IRequest<Result<string>>
    {
        public string SearchString { get; set; }

        public ExportBreedsQuery(string searchString = "")
        {
            SearchString = searchString;
        }
    }

    internal class ExportBreedsQueryHandler : IRequestHandler<ExportBreedsQuery, Result<string>>
    {
        private readonly IExcelService _excelService;
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IStringLocalizer<ExportBreedsQueryHandler> _localizer;

        public ExportBreedsQueryHandler(IExcelService excelService
            , IUnitOfWork<int> unitOfWork
            , IStringLocalizer<ExportBreedsQueryHandler> localizer)
        {
            _excelService = excelService;
            _unitOfWork = unitOfWork;
            _localizer = localizer;
        }

        public async Task<Result<string>> Handle(ExportBreedsQuery request, CancellationToken cancellationToken)
        {
            var breedFilterSpec = new BreedFilterSpecification(request.SearchString);
            var breeds = await _unitOfWork.Repository<Breed>().Entities
                .Specify(breedFilterSpec)
                .ToListAsync(cancellationToken);
            var data = await _excelService.ExportAsync(breeds, mappers: new Dictionary<string, Func<Breed, object>>
            {
                { _localizer["Id"], item => item.Id },
                { _localizer["Name"], item => item.Name }
            }, sheetName: _localizer["Breeds"]);

            return await Result<string>.SuccessAsync(data: data);
        }
    }
}
