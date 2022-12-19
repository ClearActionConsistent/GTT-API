using FluentValidation;
using GTT.Application.Repositories;
using GTT.Application.ViewModels;
using MediatR;

namespace GTT.Application.Commands
{
    public class GetAllPagingChallenge
    {
        public record Command(
        int pageIndex, int pageSize
        ) : IRequest<List<ChallengeVM>>;

        internal class Validator : AbstractValidator<Command>
        {
            public Validator()
            {
                RuleFor(x => x.pageIndex)
                    .GreaterThan(0).WithMessage("Number of page must be greater than 0");

                RuleFor(x => x.pageSize)
                    .GreaterThanOrEqualTo(1).WithMessage("Number of challenges must be greater than 0");
            }
        }

        internal class Handler : IRequestHandler<Command, List<ChallengeVM>>

        {
            //public IUnitOfWork _uow;
            private readonly IChallengeRepository _challengeRepo;

            public Handler(IChallengeRepository repo)
            {
                _challengeRepo = repo;
            }
            public async Task<List<ChallengeVM>> Handle(Command command, CancellationToken cancellationToken)
            {
                var challenge = await _challengeRepo.GetAllPagingAsync(command.pageIndex, command.pageSize);
                return (List<ChallengeVM>)challenge;
            }
        }
    }
}
