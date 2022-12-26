using FluentValidation;
using GTT.Application.Interfaces.Repositories;
using GTT.Application.Requests.ExerciseLib;
using GTT.Application.Response;
using MediatR;

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
                try
                {
                    var result = await _exerciseLibRepository.CreateExerciseLib(request.data);
                    return result;
                }
                catch (Exception ex)
                {
                    var error = $"[Handle] CreateExGroup - {Helpers.BuildErrorMessage(ex)}";
                    throw new Exception(error);
                }
            }
        }
    }
}
