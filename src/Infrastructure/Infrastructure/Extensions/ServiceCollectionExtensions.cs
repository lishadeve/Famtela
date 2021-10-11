using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Famtela.Application.Interfaces.Repositories;
using Famtela.Application.Interfaces.Services.Storage;
using Famtela.Application.Interfaces.Services.Storage.Provider;
using Famtela.Application.Interfaces.Serialization.Serializers;
using Famtela.Application.Serialization.JsonConverters;
using Famtela.Infrastructure.Repositories;
using Famtela.Infrastructure.Services.Storage;
using Famtela.Application.Serialization.Options;
using Famtela.Infrastructure.Services.Storage.Provider;
using Famtela.Application.Serialization.Serializers;

namespace Famtela.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddInfrastructureMappings(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
        }

        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            return services
                .AddTransient(typeof(IRepositoryAsync<,>), typeof(RepositoryAsync<,>))
                .AddTransient<IAgeRepository, AgeRepository>()
                .AddTransient<IBreedRepository, BreedRepository>()
                .AddTransient<IChickenExpenseRepository, ChickenExpenseRepository>()
                .AddTransient<IChicksRepository, ChicksRepository>()
                .AddTransient<IColorRepository, ColorRepository>()
                .AddTransient<IConsumptionRepository, ConsumptionRepository>()
                .AddTransient<ICountyRepository, CountyRepository>()
                .AddTransient<ICowRepository, CowRepository>()
                .AddTransient<IDairyExpenseRepository, DairyExpenseRepository>()
                .AddTransient<IDiseaseRepository, DiseaseRepository>()
                .AddTransient<IDocumentRepository, DocumentRepository>()
                .AddTransient<IDocumentTypeRepository, DocumentTypeRepository>()
                .AddTransient<IEggRepository, EggRepository>()
                .AddTransient<IFarmProfileRepository, FarmProfileRepository>()
                .AddTransient<IGrowersRepository, GrowersRepository>()
                .AddTransient<ILayersRepository, LayersRepository>()
                .AddTransient<IMilkRepository, MilkRepository>()
                .AddTransient<IStatusRepository, StatusRepository>()
                .AddTransient<ITagRepository, TagRepository>()
                .AddTransient<ITypeofFarmingRepository, TypeofFarmingRepository>()
                .AddTransient<ITypeofFeedRepository, TypeofFeedRepository>()
                .AddTransient(typeof(IUnitOfWork<>), typeof(UnitOfWork<>))
                .AddTransient<IVaccinationRepository, VaccinationRepository>()
                .AddTransient<IWeightEstimateRepository, WeightEstimateRepository>();
        }

        public static IServiceCollection AddExtendedAttributesUnitOfWork(this IServiceCollection services)
        {
            return services
                .AddTransient(typeof(IExtendedAttributeUnitOfWork<,,>), typeof(ExtendedAttributeUnitOfWork<,,>));
        }

        public static IServiceCollection AddServerStorage(this IServiceCollection services)
            => AddServerStorage(services, null);

        public static IServiceCollection AddServerStorage(this IServiceCollection services, Action<SystemTextJsonOptions> configure)
        {
            return services
                .AddScoped<IJsonSerializer, SystemTextJsonSerializer>()
                .AddScoped<IStorageProvider, ServerStorageProvider>()
                .AddScoped<IServerStorageService, ServerStorageService>()
                .AddScoped<ISyncServerStorageService, ServerStorageService>()
                .Configure<SystemTextJsonOptions>(configureOptions =>
                {
                    configure?.Invoke(configureOptions);
                    if (!configureOptions.JsonSerializerOptions.Converters.Any(c => c.GetType() == typeof(TimespanJsonConverter)))
                        configureOptions.JsonSerializerOptions.Converters.Add(new TimespanJsonConverter());
                });
        }
    }
}