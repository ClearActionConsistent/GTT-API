using MediatR;
using GTT.Application.Interfaces.Repositories;
using GTT.Application.Extensions;
using GTT.Domain.Entities;
using FluentValidation;

namespace GTT.Application.Queries
{
    public class GetExGroup
    {
        public record Query(
            int pageSize,
            int pageIndex,
            string keyword
            ) : IRequest<GTTPageResults<ExerciseGroupResponse>>;

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

        internal class Handler : IRequestHandler<Query, GTTPageResults<ExerciseGroupResponse>>
        {
            private readonly IExerciseGroupRepository _exGroupRepository;

            public Handler(IExerciseGroupRepository exGroupRepository)
            {
                _exGroupRepository = exGroupRepository;
            }

            public async Task<GTTPageResults<ExerciseGroupResponse>> Handle(Query request, CancellationToken cancellationToken)
            {
                string filter = string.Empty;

                if (!string.IsNullOrEmpty(request.keyword))
                {
                    filter = $"WHERE GroupName LIKE '%{request.keyword}%'";
                }

                var result = await _exGroupRepository.GetAllExGroup(request.pageSize, request.pageIndex, filter);

                return GTTPagingUtility.CreatePagedResultsQuery(
                    result.ExcerciseGroups,
                    request.pageIndex,
                    request.pageSize,
                    result.TotalRow);
            }
        }
    }
}
