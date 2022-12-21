using FluentValidation;
using GTT.Application.Repositories;
using GTT.Application.Requests;
using GTT.Application.Response;
using GTT.Domain.Enums;
using MediatR;
using System.Net;

namespace GTT.Application.Commands
{
    public class CreateExGroup
    {
        public record Command(
        ExGroupRequestModel data
        ) : IRequest<BaseResponseModel>;

        internal class Validator : AbstractValidator<Command>
        {
            public Validator()
            {
                RuleFor(x => x.data.GroupNumber)
                    .GreaterThan(0).WithMessage("Group number is greater than 0")
                     .NotNull().WithMessage("Group number is required")
                     .NotEmpty().WithMessage("Group number is not empty");
                RuleFor(x => x.data.GroupName)
                     .NotNull().WithMessage("Group name is required")
                     .NotEmpty().WithMessage("Group name is not empty");
                RuleFor(x => x.data.Address)
                     .NotNull().WithMessage("Address is required")
                     .NotEmpty().WithMessage("Address is not empty");
                RuleFor(x => x.data.Quotation)
                    .GreaterThan(0).WithMessage("Quotation is greater than 0")
                    .LessThan(101).WithMessage("Quotation is less than 100")
                     .NotNull().WithMessage("Quotation is required")
                     .NotEmpty().WithMessage("Quotation is not empty");
                RuleFor(x => x.data.City)
                     .NotNull().WithMessage("City is required")
                     .NotEmpty().WithMessage("City is not empty");
                RuleFor(x => x.data.Phone)
                     .NotNull().WithMessage("Phone is required")
                     .NotEmpty().WithMessage("Phone is not empty");
                RuleFor(x => x.data.Community)
                     .NotNull().WithMessage("Community is required")
                     .NotEmpty().WithMessage("Community is not empty");
                RuleFor(x => x.data.IsActive)
                     .NotNull().WithMessage("Active is required")
                     .NotEmpty().WithMessage("Active is not empty");
            }
        }

        internal class Handler : IRequestHandler<Command, BaseResponseModel>
        {
            private IUnitOfWork _uow;

            public Handler(IUnitOfWork uow)
            {
                _uow = uow;
            }

            public async Task<BaseResponseModel> Handle(Command command, CancellationToken cancellationToken)
            {
                try
                {
                    //handle request command to create excercise group information
                    var result = await _uow.ExGroup.CreateExGroup(command.data);
                    if (result >= 0)
                    {
                        _uow.Complete();
                        return new BaseResponseModel(HttpStatusCode.OK, "Success");
                    }
                    else
                    {
                        _uow.Rollback();
                        return new BaseResponseModel(HttpStatusCode.BadRequest, "Failed to create excercise group");
                    }
                }
                catch (Exception ex)
                {
                    var error = $"[Handle] CreateExGroup - {Helper.BuildErrorMessage(ex)}";
                    throw new Exception(error);
                }
            }
        }
    }
}
