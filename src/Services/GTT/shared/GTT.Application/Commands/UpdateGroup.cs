using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using GTT.Application.Interfaces.Repositories;
using GTT.Application.Requests;
using GTT.Application.Response;
using MediatR;

namespace GTT.Application.Commands
{
    public class UpdateGroup
    {
        public record Command(
        UpdateGroupRequestModel data
        ) : IRequest<BaseResponseModel>;

        internal class Validator : AbstractValidator<Command>
        {
            public Validator()
            {
                RuleFor(x => x.data.GroupId)
                     .NotEmpty()
                     .WithMessage("Group id is required")
                     .GreaterThan(0).WithMessage("Group id must greater than 0");
            }
        }

        internal class Handler : IRequestHandler<Command, BaseResponseModel>
        {
            private IGroupRepository _groupRepository;

            public Handler(IGroupRepository groupRepository)
            {
                _groupRepository = groupRepository;
            }

            public async Task<BaseResponseModel> Handle(Command command, CancellationToken cancellationToken)
            {
                //handle request command to update group 
                var result = await _groupRepository.UpdateGroup(command.data);
                if (result != null)
                {
                    return new BaseResponseModel(result);
                }

                return new BaseResponseModel(HttpStatusCode.BadRequest, "Failed to update data into Group table");
            }
        }
    }
}