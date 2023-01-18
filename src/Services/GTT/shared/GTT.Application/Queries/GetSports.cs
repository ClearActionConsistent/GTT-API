using FluentValidation;
using GTT.Application.Extensions;
using GTT.Application.Interfaces.Repositories;
using GTT.Application.Response;
using MediatR;

namespace GTT.Application.Queries
{
    public class GetSports
    {
        public record Query(
            int pageIndex,
            int pageSize
            ) : IRequest<GTTPageResults<SportReponse>>;

        internal class Validator : AbstractValidator<Query>
        {
            public Validator()
            {
                RuleFor(x => x.pageIndex)
                    .GreaterThan(0).WithMessage("Page index is greater than 0")
                    .NotEmpty().WithMessage("Page index is required");
                RuleFor(x => x.pageSize)
                    .GreaterThan(0).WithMessage("Page size is greater than 0")
                    .NotEmpty().WithMessage("Page size is required");
            }
        }

        internal class Handler : IRequestHandler<Query, GTTPageResults<SportReponse>>
        {
            private readonly ISportsRepository _sportsRepository;

            public Handler(ISportsRepository sportsRepository)
            {
                _sportsRepository = sportsRepository;
            }

            public async Task<GTTPageResults<SportReponse>> Handle(Query request, CancellationToken cancellationToken)
            {
                var result = await _sportsRepository.GetSports(request.pageIndex, request.pageSize);

                return GTTPagingUtility.CreatePagedResultsQuery(
                    result.Sports,
                    request.pageIndex,
                    request.pageSize,
                    result.TotalRow);
            }
        }
    }
}
