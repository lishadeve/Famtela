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

namespace Famtela.Application.Features.FarmProfiles.Queries.Export
{
    public class ExportFarmProfilesQuery : IRequest<Result<string>>
    {
        public string SearchString { get; set; }

        public ExportFarmProfilesQuery(string searchString = "")
        {
            SearchString = searchString;
        }
    }

    internal class ExportFarmProfilesQueryHandler : IRequestHandler<ExportFarmProfilesQuery, Result<string>>
    {
        private readonly IExcelService _excelService;
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IStringLocalizer<ExportFarmProfilesQueryHandler> _localizer;

        public ExportFarmProfilesQueryHandler(IExcelService excelService
            , IUnitOfWork<int> unitOfWork
            , IStringLocalizer<ExportFarmProfilesQueryHandler> localizer)
        {
            _excelService = excelService;
            _unitOfWork = unitOfWork;
            _localizer = localizer;
        }

        public async Task<Result<string>> Handle(ExportFarmProfilesQuery request, CancellationToken cancellationToken)
        {
            var farmprofileFilterSpec = new FarmProfileFilterSpecification(request.SearchString);
            var farmprofiles = await _unitOfWork.Repository<FarmProfile>().Entities
                .Specify(farmprofileFilterSpec)
                .ToListAsync( cancellationToken);
            var data = await _excelService.ExportAsync(farmprofiles, mappers: new Dictionary<string, Func<FarmProfile, object>>
            {
                { _localizer["Id"], item => item.Id },
                { _localizer["Farm Name"], item => item.FarmName },
            }, sheetName: _localizer["Farm Profiles"]);

            return await Result<string>.SuccessAsync(data: data);
        }
    }
}