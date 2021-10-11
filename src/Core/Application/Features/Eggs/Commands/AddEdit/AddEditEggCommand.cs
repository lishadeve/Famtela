using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Famtela.Application.Interfaces.Repositories;
using Famtela.Shared.Wrapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Localization;
using Famtela.Shared.Constants.Application;
using Famtela.Domain.Entities.Chicken;

namespace Famtela.Application.Features.Eggs.Commands.AddEdit
{
    public partial class AddEditEggCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
        [Required]
        public int Sold { get; set; }
        [Required]
        public decimal UnitPrice { get; set; }
        public int Retained { get; set; }
        public int Rejected { get; set; }
        public int Home { get; set; }
        public decimal Transport { get; set; }
        public string Remarks { get; set; }
    }

    internal class AddEditEggCommandHandler : IRequestHandler<AddEditEggCommand, Result<int>>
    {
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<AddEditEggCommandHandler> _localizer;
        private readonly IUnitOfWork<int> _unitOfWork;

        public AddEditEggCommandHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, IStringLocalizer<AddEditEggCommandHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizer = localizer;
        }

        public async Task<Result<int>> Handle(AddEditEggCommand command, CancellationToken cancellationToken)
        {
            if (command.Id == 0)
            {
                var egg = _mapper.Map<Egg>(command);
                await _unitOfWork.Repository<Egg>().AddAsync(egg);
                await _unitOfWork.CommitAndRemoveCache(cancellationToken, ApplicationConstants.Cache.GetAllEggsCacheKey);
                return await Result<int>.SuccessAsync(egg.Id, _localizer["Eggs Record Saved"]);
            }
            else
            {
                var egg = await _unitOfWork.Repository<Egg>().GetByIdAsync(command.Id);
                if (egg != null)
                {
                    egg.Sold = (command.Sold == 0) ? egg.Sold : command.Sold;
                    egg.UnitPrice = (command.UnitPrice == 0) ? egg.UnitPrice : command.UnitPrice;
                    egg.Retained = (command.Retained == 0) ? egg.Retained : command.Retained;
                    egg.Rejected = (command.Rejected == 0) ? egg.Rejected : command.Rejected;
                    egg.Home = (command.Home == 0) ? egg.Home : command.Home;
                    egg.Transport = (command.Transport == 0) ? egg.Transport : command.Transport;
                    egg.Remarks = command.Remarks ?? egg.Remarks;
                    await _unitOfWork.Repository<Egg>().UpdateAsync(egg);
                    await _unitOfWork.CommitAndRemoveCache(cancellationToken, ApplicationConstants.Cache.GetAllEggsCacheKey);
                    return await Result<int>.SuccessAsync(egg.Id, _localizer["Eggs Record Updated"]);
                }
                else
                {
                    return await Result<int>.FailAsync(_localizer["Eggs Record Not Found!"]);
                }
            }
        }
    }
}