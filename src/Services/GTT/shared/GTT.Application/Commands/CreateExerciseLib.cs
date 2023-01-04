using FluentValidation;
using GTT.Application.Interfaces.Repositories;
using GTT.Application.Requests.ExerciseLib;
using GTT.Application.Response;
using MediatR;
using System.Net;

namespace GTT.Application.Commands.ExerciseLibrary
{
    public class CreateExerciseLib
    {
        public record Command(CreateExerciseLibRequestModel data) : IRequest<BaseResponseModel>;

        public class Valiator : AbstractValidator<Command>
        {
            public Valiator()
            {
                RuleFor(x => x.data.ExerciseName)
                    .NotEmpty().WithMessage("Exercise name is not empty")
                    .NotNull().WithMessage("Exercise name is not null");
                RuleFor(x => x.data.ClassId)
                    .NotEmpty().WithMessage("Class id is not empty")
                    .NotNull().WithMessage("Class id is not null")
                    .GreaterThan(0).WithMessage("Class id is greater than 0");
                RuleFor(x => x.data.FocusArea)
                    .NotEmpty().WithMessage("Focus Area is not empty")
                    .NotNull().WithMessage("Focus Area is not null");
                RuleFor(x => x.data.Equipment)
                    .NotEmpty().WithMessage("Equipment is not empty")
                    .NotNull().WithMessage("Equipment is not null");
                RuleFor(x => x.data.ExerciseImage)
                    .NotEmpty().WithMessage("Image is not empty")
                    .NotNull().WithMessage("Image is not null");
                RuleFor(x => x.data.CommunityId)
                    .NotEmpty().WithMessage("Community id is not empty")
                    .NotNull().WithMessage("Community id is not null")
                    .GreaterThan(0).WithMessage("Community id is greater than 0");
            }
        }

        public class Handler : IRequestHandler<Command, BaseResponseModel>
        {
            private readonly  IExerciseLibRepository _exerciseLibRepository;

            public Handler(IExerciseLibRepository exerciseLibRepository)
            {
                _exerciseLibRepository = exerciseLibRepository;
            }

            public async Task<BaseResponseModel> Handle(Command request, CancellationToken cancellationToken)
            {
                var result = await _exerciseLibRepository.CreateExerciseLib(request.data);

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
