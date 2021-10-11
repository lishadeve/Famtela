using AutoMapper;
using Famtela.Application.Features.ChickenExpenses.Commands.AddEdit;
using Famtela.Application.Features.ChickenExpenses.Queries.GetAll;
using Famtela.Application.Features.ChickenExpenses.Queries.GetById;
using Famtela.Domain.Entities.Chicken;

namespace Famtela.Application.Mappings
{
    public class ChickenExpenseProfile : Profile
    {
        public ChickenExpenseProfile()
        {
            CreateMap<AddEditChickenExpenseCommand, ChickenExpense>().ReverseMap();
            CreateMap<GetChickenExpenseByIdResponse, ChickenExpense>().ReverseMap();
            CreateMap<GetAllChickenExpensesResponse, ChickenExpense>().ReverseMap();
        }
    }
}