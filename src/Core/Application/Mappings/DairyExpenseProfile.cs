using AutoMapper;
using Famtela.Application.Features.DairyExpenses.Commands.AddEdit;
using Famtela.Application.Features.DairyExpenses.Queries.GetAll;
using Famtela.Application.Features.DairyExpenses.Queries.GetById;
using Famtela.Domain.Entities.Dairy;

namespace Famtela.Application.Mappings
{
    public class DairyExpenseProfile : Profile
    {
        public DairyExpenseProfile()
        {
            CreateMap<AddEditDairyExpenseCommand, DairyExpense>().ReverseMap();
            CreateMap<GetDairyExpenseByIdResponse, DairyExpense>().ReverseMap();
            CreateMap<GetAllDairyExpensesResponse, DairyExpense>().ReverseMap();
        }
    }
}