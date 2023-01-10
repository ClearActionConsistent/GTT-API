using System.Net;
using FluentValidation;
using GTT.Application.Repositories;
using GTT.Application.Requests;
using GTT.Application.Response;
using MediatR;

namespace GTT.Application.Commands
{
    public class UpdateClassInformation
    {
        public record Command(
        UpdateClassData classData
        ) : IRequest<BaseResponseModel>;

        internal class Validator : AbstractValidator<Command>
        {
            public Validator()
            {
                RuleFor(x => x.classData.ClassId)
                    .NotNull()
                    .NotEmpty()
                    .WithMessage("ClassId is required")
                    .GreaterThan(0).WithMessage("ClassId is greater than 0");
            }

            internal class Handler : IRequestHandler<Command, BaseResponseModel>
            {

                private IClassRepository _classRepository;

                public Handler(IClassRepository classRepository)
                {
                    _classRepository = classRepository;
                }

                public async Task<BaseResponseModel> Handle(Command request, CancellationToken cancellationToken)
                {
                    //handle request command to update class information

                    var result = await _classRepository.UpdateClass(request.classData);

                    if (result == -1)
                    {
                        return new BaseResponseModel(HttpStatusCode.NotFound, "ClassId is invalid");
                    }

                    if (result == 0)
                    {
                        return new BaseResponseModel(HttpStatusCode.BadRequest, "Can not update class");
                    }

                    return new BaseResponseModel(HttpStatusCode.OK, "Successfully");
                }
            }
        }
    }
}
