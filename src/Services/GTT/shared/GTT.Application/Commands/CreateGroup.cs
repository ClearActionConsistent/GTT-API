using FluentValidation;
using GTT.Application.Interfaces.Repositories;
using GTT.Application.Requests;
using GTT.Application.Requests.ExerciseLib;
using GTT.Application.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace GTT.Application.Commands.GroupLib
{
    public class CreateGroupLib
    {
        public record Command(CreateGroupRequestModel data) : IRequest<BaseResponseModel>;

        public class Valiator : AbstractValidator<Command>
        {
            public Valiator()
            {
                RuleFor(x => x.data.GroupName)
                    .NotEmpty().WithMessage("Group name is not empty")
                    .NotNull().WithMessage("Group name is not null");
                RuleFor(x => x.data.Location)
                    .NotEmpty().WithMessage("Location is not empty")
                    .NotNull().WithMessage("Location is not null");
                RuleFor(x => x.data.GroupType)
                    .NotEmpty().WithMessage("Group type is not empty")
                    .NotNull().WithMessage("Group type is not null");
                RuleFor(x => x.data.Sports)
                    .NotEmpty().WithMessage("Sport is not empty")
                    .NotNull().WithMessage("Sport is not null");
                RuleFor(x => x.data.IsActive)
                    .NotEmpty().WithMessage("Active is not empty")
                    .NotNull().WithMessage("Active is not null");
            }
        }

        public class Handler : IRequestHandler<Command, BaseResponseModel>
        {
            private readonly IGroupRepository _groupRepository;

            public Handler(IGroupRepository groupRepository)
            {
                _groupRepository = groupRepository;
            }

            public async Task<BaseResponseModel> Handle(Command request, CancellationToken cancellationToken)
            {
                var result = await _groupRepository.CreateGroup(request.data);

                if (result.Status == HttpStatusCode.NotFound)
                {
                    return new BaseResponseModel(HttpStatusCode.NotFound, result.Message);
                }

                if (result.Status == HttpStatusCode.BadRequest)
                {
                    return new BaseResponseModel(HttpStatusCode.BadRequest, result.Message);
                }

                return result;
            }
        }
    }
}
