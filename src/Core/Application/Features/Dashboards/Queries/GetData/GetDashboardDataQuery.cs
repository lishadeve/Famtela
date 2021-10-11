using Famtela.Application.Interfaces.Repositories;
using Famtela.Application.Interfaces.Services.Identity;
using Famtela.Domain.Entities.Catalog;
using Famtela.Shared.Wrapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Famtela.Domain.Entities.ExtendedAttributes;
using Famtela.Domain.Entities.Misc;
using Microsoft.Extensions.Localization;
using Famtela.Domain.Entities.Chicken;
using Famtela.Domain.Entities.Dairy;

namespace Famtela.Application.Features.Dashboards.Queries.GetData
{
    public class GetDashboardDataQuery : IRequest<Result<DashboardDataResponse>>
    {

    }

    internal class GetDashboardDataQueryHandler : IRequestHandler<GetDashboardDataQuery, Result<DashboardDataResponse>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IUserService _userService;
        private readonly IRoleService _roleService;
        private readonly IStringLocalizer<GetDashboardDataQueryHandler> _localizer;

        public GetDashboardDataQueryHandler(IUnitOfWork<int> unitOfWork, IUserService userService, IRoleService roleService, IStringLocalizer<GetDashboardDataQueryHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _userService = userService;
            _roleService = roleService;
            _localizer = localizer;
        }

        public async Task<Result<DashboardDataResponse>> Handle(GetDashboardDataQuery query, CancellationToken cancellationToken)
        {
            var response = new DashboardDataResponse
            {
                FarmProfileCount = await _unitOfWork.Repository<FarmProfile>().Entities.CountAsync(cancellationToken),
                CountyCount = await _unitOfWork.Repository<County>().Entities.CountAsync(cancellationToken),
                TypeofFarmingCount = await _unitOfWork.Repository<TypeofFarming>().Entities.CountAsync(cancellationToken),
                AgeCount = await _unitOfWork.Repository<Age>().Entities.CountAsync(cancellationToken),
                ChickenExpenseCount = await _unitOfWork.Repository<ChickenExpense>().Entities.CountAsync(cancellationToken),
                ChicksCount = await _unitOfWork.Repository<Chick>().Entities.CountAsync(cancellationToken),
                ConsumptionCount = await _unitOfWork.Repository<Consumption>().Entities.CountAsync(cancellationToken),
                DiseaseCount = await _unitOfWork.Repository<Disease>().Entities.CountAsync(cancellationToken),
                EggCount = await _unitOfWork.Repository<Egg>().Entities.CountAsync(cancellationToken),
                GrowersCount = await _unitOfWork.Repository<Grower>().Entities.CountAsync(cancellationToken),
                LayersCount = await _unitOfWork.Repository<Layer>().Entities.CountAsync(cancellationToken),
                TypeofFeedCount = await _unitOfWork.Repository<TypeofFeed>().Entities.CountAsync(cancellationToken),
                VaccinationCount = await _unitOfWork.Repository<Vaccination>().Entities.CountAsync(cancellationToken),
                BreedCount = await _unitOfWork.Repository<Breed>().Entities.CountAsync(cancellationToken),
                ColorCount = await _unitOfWork.Repository<Color>().Entities.CountAsync(cancellationToken),
                CowCount = await _unitOfWork.Repository<Cow>().Entities.CountAsync(cancellationToken),
                DairyExpenseCount = await _unitOfWork.Repository<DairyExpense>().Entities.CountAsync(cancellationToken),
                MilkCount = await _unitOfWork.Repository<Milk>().Entities.CountAsync(cancellationToken),
                StatusCount = await _unitOfWork.Repository<Status>().Entities.CountAsync(cancellationToken),
                TagCount = await _unitOfWork.Repository<Tag>().Entities.CountAsync(cancellationToken),
                WeightEstimateCount = await _unitOfWork.Repository<WeightEstimate>().Entities.CountAsync(cancellationToken),
                DocumentCount = await _unitOfWork.Repository<Document>().Entities.CountAsync(cancellationToken),
                DocumentTypeCount = await _unitOfWork.Repository<DocumentType>().Entities.CountAsync(cancellationToken),
                DocumentExtendedAttributeCount = await _unitOfWork.Repository<DocumentExtendedAttribute>().Entities.CountAsync(cancellationToken),
                UserCount = await _userService.GetCountAsync(),
                RoleCount = await _roleService.GetCountAsync()
            };

            var selectedYear = DateTime.Now.Year;
            double[] farmprofilesFigure = new double[13];
            double[] countiesFigure = new double[13];
            double[] typesofarmingFigure = new double[13];
            double[] agesFigure = new double[13];
            double[] chickenexpensesFigure = new double[13];
            double[] chicksFigure = new double[13];
            double[] consumptionsFigure = new double[13];
            double[] diseasesFigure = new double[13];
            double[] eggsFigure = new double[13];
            double[] growersFigure = new double[13];
            double[] layersFigure = new double[13];
            double[] typeoffeedsFigure = new double[13];
            double[] vaccinationsFigure = new double[13];
            double[] breedsFigure = new double[13];
            double[] colorsFigure = new double[13];
            double[] cowsFigure = new double[13];
            double[] dairyexpensesFigure = new double[13];
            double[] milkFigure = new double[13];
            double[] statusesFigure = new double[13];
            double[] tagsFigure = new double[13];
            double[] weightestimatesFigure = new double[13];
            double[] documentsFigure = new double[13];
            double[] documentTypesFigure = new double[13];
            double[] documentExtendedAttributesFigure = new double[13];
            for (int i = 1; i <= 12; i++)
            {
                var month = i;
                var filterStartDate = new DateTime(selectedYear, month, 01);
                var filterEndDate = new DateTime(selectedYear, month, DateTime.DaysInMonth(selectedYear, month), 23, 59, 59); // Monthly Based

                farmprofilesFigure[i - 1] = await _unitOfWork.Repository<FarmProfile>().Entities.Where(x => x.CreatedOn >= filterStartDate && x.CreatedOn <= filterEndDate).CountAsync(cancellationToken);
                countiesFigure[i - 1] = await _unitOfWork.Repository<County>().Entities.Where(x => x.CreatedOn >= filterStartDate && x.CreatedOn <= filterEndDate).CountAsync(cancellationToken);
                typesofarmingFigure[i - 1] = await _unitOfWork.Repository<TypeofFarming>().Entities.Where(x => x.CreatedOn >= filterStartDate && x.CreatedOn <= filterEndDate).CountAsync(cancellationToken);
                agesFigure[i - 1] = await _unitOfWork.Repository<Age>().Entities.Where(x => x.CreatedOn >= filterStartDate && x.CreatedOn <= filterEndDate).CountAsync(cancellationToken);
                chickenexpensesFigure[i - 1] = await _unitOfWork.Repository<ChickenExpense>().Entities.Where(x => x.CreatedOn >= filterStartDate && x.CreatedOn <= filterEndDate).CountAsync(cancellationToken);
                chicksFigure[i - 1] = await _unitOfWork.Repository<Chick>().Entities.Where(x => x.CreatedOn >= filterStartDate && x.CreatedOn <= filterEndDate).CountAsync(cancellationToken);
                consumptionsFigure[i - 1] = await _unitOfWork.Repository<Consumption>().Entities.Where(x => x.CreatedOn >= filterStartDate && x.CreatedOn <= filterEndDate).CountAsync(cancellationToken);
                diseasesFigure[i - 1] = await _unitOfWork.Repository<Disease>().Entities.Where(x => x.CreatedOn >= filterStartDate && x.CreatedOn <= filterEndDate).CountAsync(cancellationToken);
                eggsFigure[i - 1] = await _unitOfWork.Repository<Egg>().Entities.Where(x => x.CreatedOn >= filterStartDate && x.CreatedOn <= filterEndDate).CountAsync(cancellationToken);
                growersFigure[i - 1] = await _unitOfWork.Repository<Grower>().Entities.Where(x => x.CreatedOn >= filterStartDate && x.CreatedOn <= filterEndDate).CountAsync(cancellationToken);
                layersFigure[i - 1] = await _unitOfWork.Repository<Layer>().Entities.Where(x => x.CreatedOn >= filterStartDate && x.CreatedOn <= filterEndDate).CountAsync(cancellationToken);
                typeoffeedsFigure[i - 1] = await _unitOfWork.Repository<TypeofFeed>().Entities.Where(x => x.CreatedOn >= filterStartDate && x.CreatedOn <= filterEndDate).CountAsync(cancellationToken);
                vaccinationsFigure[i - 1] = await _unitOfWork.Repository<Vaccination>().Entities.Where(x => x.CreatedOn >= filterStartDate && x.CreatedOn <= filterEndDate).CountAsync(cancellationToken);
                breedsFigure[i - 1] = await _unitOfWork.Repository<Breed>().Entities.Where(x => x.CreatedOn >= filterStartDate && x.CreatedOn <= filterEndDate).CountAsync(cancellationToken);
                colorsFigure[i - 1] = await _unitOfWork.Repository<Color>().Entities.Where(x => x.CreatedOn >= filterStartDate && x.CreatedOn <= filterEndDate).CountAsync(cancellationToken);
                cowsFigure[i - 1] = await _unitOfWork.Repository<Cow>().Entities.Where(x => x.CreatedOn >= filterStartDate && x.CreatedOn <= filterEndDate).CountAsync(cancellationToken);
                dairyexpensesFigure[i - 1] = await _unitOfWork.Repository<DairyExpense>().Entities.Where(x => x.CreatedOn >= filterStartDate && x.CreatedOn <= filterEndDate).CountAsync(cancellationToken);
                milkFigure[i - 1] = await _unitOfWork.Repository<Milk>().Entities.Where(x => x.CreatedOn >= filterStartDate && x.CreatedOn <= filterEndDate).CountAsync(cancellationToken);
                statusesFigure[i - 1] = await _unitOfWork.Repository<Status>().Entities.Where(x => x.CreatedOn >= filterStartDate && x.CreatedOn <= filterEndDate).CountAsync(cancellationToken);
                tagsFigure[i - 1] = await _unitOfWork.Repository<Tag>().Entities.Where(x => x.CreatedOn >= filterStartDate && x.CreatedOn <= filterEndDate).CountAsync(cancellationToken);
                weightestimatesFigure[i - 1] = await _unitOfWork.Repository<WeightEstimate>().Entities.Where(x => x.CreatedOn >= filterStartDate && x.CreatedOn <= filterEndDate).CountAsync(cancellationToken);
                documentsFigure[i - 1] = await _unitOfWork.Repository<Document>().Entities.Where(x => x.CreatedOn >= filterStartDate && x.CreatedOn <= filterEndDate).CountAsync(cancellationToken);
                documentTypesFigure[i - 1] = await _unitOfWork.Repository<DocumentType>().Entities.Where(x => x.CreatedOn >= filterStartDate && x.CreatedOn <= filterEndDate).CountAsync(cancellationToken);
                documentExtendedAttributesFigure[i - 1] = await _unitOfWork.Repository<DocumentExtendedAttribute>().Entities.Where(x => x.CreatedOn >= filterStartDate && x.CreatedOn <= filterEndDate).CountAsync(cancellationToken);
            }

            response.DataEnterBarChart.Add(new ChartSeries { Name = _localizer["Farm Profiles"], Data = farmprofilesFigure });
            //response.DataEnterBarChart.Add(new ChartSeries { Name = _localizer["Counties"], Data = countiesFigure });
            //response.DataEnterBarChart.Add(new ChartSeries { Name = _localizer["Types of Farming"], Data = typesofarmingFigure });
            //response.DataEnterBarChart.Add(new ChartSeries { Name = _localizer["Ages"], Data = agesFigure });
            response.DataEnterBarChart.Add(new ChartSeries { Name = _localizer["Chicken Expenses Records"], Data = chickenexpensesFigure });
            response.DataEnterBarChart.Add(new ChartSeries { Name = _localizer["Chicks Records"], Data = chicksFigure });
            //response.DataEnterBarChart.Add(new ChartSeries { Name = _localizer["Consumption Records"], Data = consumptionsFigure });
            //response.DataEnterBarChart.Add(new ChartSeries { Name = _localizer["Disease Records"], Data = diseasesFigure });
            response.DataEnterBarChart.Add(new ChartSeries { Name = _localizer["Eggs Records"], Data = eggsFigure });
            response.DataEnterBarChart.Add(new ChartSeries { Name = _localizer["Growers Records"], Data = growersFigure });
            response.DataEnterBarChart.Add(new ChartSeries { Name = _localizer["Layers Records"], Data = layersFigure });
            //response.DataEnterBarChart.Add(new ChartSeries { Name = _localizer["Type of Feed"], Data = typeoffeedsFigure });
            //response.DataEnterBarChart.Add(new ChartSeries { Name = _localizer["Vaccination Records"], Data = vaccinationsFigure });
            //response.DataEnterBarChart.Add(new ChartSeries { Name = _localizer["Breeds"], Data = breedsFigure });
            //response.DataEnterBarChart.Add(new ChartSeries { Name = _localizer["Colors"], Data = colorsFigure });
            response.DataEnterBarChart.Add(new ChartSeries { Name = _localizer["Cows Records"], Data = cowsFigure });
            response.DataEnterBarChart.Add(new ChartSeries { Name = _localizer["Dairy Expenses Records"], Data = dairyexpensesFigure });
            response.DataEnterBarChart.Add(new ChartSeries { Name = _localizer["Milk Records"], Data = milkFigure });
            //response.DataEnterBarChart.Add(new ChartSeries { Name = _localizer["Statuses"], Data = statusesFigure });
            //response.DataEnterBarChart.Add(new ChartSeries { Name = _localizer["Tags"], Data = tagsFigure });
            //response.DataEnterBarChart.Add(new ChartSeries { Name = _localizer["Weight Estimate Records"], Data = weightestimatesFigure });
            //response.DataEnterBarChart.Add(new ChartSeries { Name = _localizer["Documents"], Data = documentsFigure });
            //response.DataEnterBarChart.Add(new ChartSeries { Name = _localizer["Document Types"], Data = documentTypesFigure });
            //response.DataEnterBarChart.Add(new ChartSeries { Name = _localizer["Document Extended Attributes"], Data = documentExtendedAttributesFigure });

            return await Result<DashboardDataResponse>.SuccessAsync(response);
        }
    }
}