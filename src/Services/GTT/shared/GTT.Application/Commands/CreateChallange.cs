using FluentValidation;
using GTT.Application.Repositories;
using GTT.Application.ViewModels;
using GTT.Domain.Entities;
using MediatR;

namespace GTT.Application.Commands
{
    public class CreateChallange
    {
        public record Command(
        CreateChallengeData createChallengeData
        ) : IRequest<ChallengeVM>;

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
            }
        }

        internal class Handler : IRequestHandler<Command, ChallengeVM>

        {
            //public IUnitOfWork _uow;
            private readonly IChallengeRepository _challengeRepo;

            public Handler(IChallengeRepository repo)
            {
                _challengeRepo = repo;
            }
            public async Task<ChallengeVM> Handle(Command command, CancellationToken cancellationToken)
            {
                Challenge challengeVM = new Challenge{ 
                        Calories = command.createChallengeData.Calories,
                        SplatPoints = command.createChallengeData.SplatPoints,
                        AvgHr = command.createChallengeData.AvgHr,
                        MaxHr = command.createChallengeData.MaxHr,
                        Miles = command.createChallengeData.Miles,
                        Steps = command.createChallengeData.Steps,
                        memberID = command.createChallengeData.memberID,
                        CreatedDate = command.createChallengeData.CreatedDate,
                        UpdatedDate = DateTime.Now,
                        
                };

                var challenge = _challengeRepo.AddAsync(challengeVM);

                ////map data to entity
                //var result = await _uow.Challenges.AddAsync(new Challenge { });
                //_uow.Complete();
                //return new ChallengeVM();

                return new ChallengeVM();
            }
        }
    }
}
