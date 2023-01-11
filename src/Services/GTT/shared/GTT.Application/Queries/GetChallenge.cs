using FluentValidation;
using GTT.Application.Extensions;
using GTT.Application.Repositories;
using GTT.Domain.Entities;
using MediatR;

namespace GTT.Application.Queries
{
    public class GetChallenge
    {
        public record Query(
            int pageSize,
            int pageIndex
            ) : IRequest<GTTPageResults<ChallengeResponse>>;

        internal class Validator : AbstractValidator<Query>
        {
            public Validator()
            {
                RuleFor(x => x.pageIndex)
                    .GreaterThan(0).WithMessage("Page index is greater than 0")
                    .NotEmpty().WithMessage("Page index must not empty")
                    .NotNull().WithMessage("Page index is required");
                RuleFor(x => x.pageSize)
                    .GreaterThan(0).WithMessage("Page size is greater than 0")
                    .NotEmpty().WithMessage("Page size must not empty")
                    .NotNull().WithMessage("Page size is required");
            }
        }

        internal class Handler : IRequestHandler<Query, GTTPageResults<ChallengeResponse>>
        {
            private readonly IChallengeRepository _challengeRepository;

            public Handler(IChallengeRepository challengeRepository)
            {
                _challengeRepository = challengeRepository;
            }

            public async Task<GTTPageResults<ChallengeResponse>> Handle(Query request, CancellationToken cancellationToken)
            {
                var result = await _challengeRepository.GetAllAsync(request.pageSize, request.pageIndex);

                return GTTPagingUtility.CreatePagedResultsQuery(
                    result.Challenges,
                    request.pageIndex,
                    request.pageSize,
                    result.TotalRow);
            }
        }
    }
}
