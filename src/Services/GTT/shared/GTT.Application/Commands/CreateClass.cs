using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using GTT.Application.Repositories;
using GTT.Application.Requests;
using GTT.Application.Response;
using MediatR;

namespace GTT.Application.Commands
{
    public class CreateClass
    {
        public record Command(
         CreateClassRequestModel data
        ) : IRequest<BaseResponseModel>;

        internal class Validator : AbstractValidator<Command>
        {
            public Validator()
            {
                RuleFor(x => x.data.Title)
                    .NotNull()
                    .NotEmpty()
                    .WithMessage("Title is required");
                RuleFor(x => x.data.CommunityId)
                    .GreaterThan(0).WithMessage("CommunityId is required")
                    .NotNull()
                    .NotEmpty()
                    .WithMessage("CommunityId is required");
                RuleFor(x => x.data.CoachId)
                    .GreaterThan(0).WithMessage("CoachId is required")
                    .NotNull()
                    .NotEmpty()
                    .WithMessage("CoachId is required");
            }
        }

        internal class Handler : IRequestHandler<Command, BaseResponseModel>
        {
            private IClassRepository _classRepository;

            public Handler(IClassRepository classRepository)
            {
                _classRepository = classRepository;
            }

            public async Task<BaseResponseModel> Handle(Command command, CancellationToken cancellation)
            {
                //handle request command to create class information
                var result = await _classRepository.CreateClass(command.data);

                if (result.Status == HttpStatusCode.OK)
                {
                    return new BaseResponseModel(HttpStatusCode.OK, "Success");
                }

                if (result.Status == HttpStatusCode.NotFound)
                {
                    return new BaseResponseModel(HttpStatusCode.NotFound, result.Message);
                }

                return new BaseResponseModel(HttpStatusCode.BadRequest, "Failed to create Class");
            }
        }
    }
}
