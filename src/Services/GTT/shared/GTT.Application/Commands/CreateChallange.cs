using FluentValidation;
using GTT.Application.Repositories;
using GTT.Application.Requests;
using GTT.Application.Response;
using MediatR;

namespace GTT.Application.Commands
{
    public class CreateChallange
    {
        public record Command(
        CreateChallengeData createChallengeData
        ) : IRequest<BaseResponseModel>;

        internal class Validator : AbstractValidator<Command>
        {
            public Validator()
            {
                RuleFor(x => x.createChallengeData.Calories)
                     .NotNull().WithMessage("Challege Calories is required")
                     .NotEmpty().WithMessage("Challege Calories is not empty");

                RuleFor(x => x.createChallengeData.SplatPoints)
                     .NotNull().WithMessage("Challege SplatPoints is required")
                     .NotEmpty().WithMessage("Challege SplatPoints is not empty");

                RuleFor(x => x.createChallengeData.AvgHr)
                     .NotNull().WithMessage("Challege Avg Hr is required")
                     .NotEmpty().WithMessage("Challege Avg Hr is not empty");

                RuleFor(x => x.createChallengeData.MaxHr)
                     .NotNull().WithMessage("Challege Max Hr is required")
                     .NotEmpty().WithMessage("Challege Max Hr is not empty");

                RuleFor(x => x.createChallengeData.Miles)
                     .NotNull().WithMessage("Challege Miles is required")
                     .NotEmpty().WithMessage("Challege Miles is not empty");

                RuleFor(x => x.createChallengeData.Steps)
                     .NotNull().WithMessage("Challege Steps is required")
                     .NotEmpty().WithMessage("Challege Steps is not empty");

                RuleFor(x => x.createChallengeData.ClassID)
                    .NotNull().WithMessage("Class ID is required")
                    .NotEmpty().WithMessage("Class ID is not empty");
            }
        }

        internal class Handler : IRequestHandler<Command, BaseResponseModel>
        {
            private readonly IChallengeRepository _challengeRepo;

            public Handler(IChallengeRepository challengeRepository)
            {
                _challengeRepo = challengeRepository;
            }
            public async Task<BaseResponseModel> Handle(Command command, CancellationToken cancellationToken)
            {
                var challenge = await _challengeRepo.AddAsync(command.createChallengeData);

                return challenge;
            }
        }
    }
}
