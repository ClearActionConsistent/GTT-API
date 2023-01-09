using FluentValidation;
using GTT.Application.Extensions;
using GTT.Application.Interfaces.Repositories;
using GTT.Application.Response;
using MediatR;

namespace GTT.Application.Queries
{
    public class GetExLibrary
    {
        public record Query(
            int pageSize,
            int pageIndex,
            string keyword
            ) : IRequest<GTTPageResults<ExerciseLibraryResponse>>;

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

        internal class Handler : IRequestHandler<Query, GTTPageResults<ExerciseLibraryResponse>>
        {
            private readonly IExerciseLibRepository _exerciseLibRepository;

            public Handler(IExerciseLibRepository exerciseLibRepository)
            {
                _exerciseLibRepository = exerciseLibRepository;
            }

            public async Task<GTTPageResults<ExerciseLibraryResponse>> Handle(Query request, CancellationToken cancellationToken)
            {
                string filter = string.Empty;

                if (!string.IsNullOrEmpty(request.keyword))
                {
                    filter = $"WHERE Name LIKE '%{request.keyword}%'";
                }

                var result = await _exerciseLibRepository.GetAllExerciseLib(request.pageSize, request.pageIndex, filter);

                return GTTPagingUtility.CreatePagedResultsQuery(
                    result.ExcerciseLibrary,
                    request.pageIndex,
                    request.pageSize,
                    result.TotalRow);
            }
        }
    }
}
