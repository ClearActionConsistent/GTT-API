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
                RuleFor(x => x.createChallengeData.Name)
                     .NotNull().WithMessage("Challege name is required")
                     .NotEmpty().WithMessage("Challege name is not empty");
            }
        }

        internal class Handler : IRequestHandler<Command, ChallengeVM>
        {
            public IChallengeRepository _repo;

            public Handler(IChallengeRepository repo)
            {
                _repo = repo;
            }
            public async Task<ChallengeVM> Handle(Command request, CancellationToken cancellationToken)
            {
                //map data to entity
                var result = await _repo.AddAsync(new Challenge { });
                return new ChallengeVM();
            }
        }
    }
}
