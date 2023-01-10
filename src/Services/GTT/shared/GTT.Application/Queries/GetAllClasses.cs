using FluentValidation;
using GTT.Application.Extensions;
using GTT.Application.Repositories;
using GTT.Application.Response;
using MediatR;

namespace GTT.Application.Queries
{
    public class GetAllClasses
    {
        public record Query(
        int pageSize,
        int pageIndex,
        string keyword
        ) : IRequest<GTTPageResults<ClassResponse>>;

        internal class Validator : AbstractValidator<Query>
        {
            public Validator()
            {
                RuleFor(x => x.pageIndex)
                    .NotNull().WithMessage("Page index is required")
                    .NotEmpty().WithMessage("Page index must not empty")
                    .GreaterThan(0).WithMessage("Page index is greater than 0");
                RuleFor(x => x.pageSize)
                    .NotNull().WithMessage("Page size is required")
                    .NotEmpty().WithMessage("Page size must not empty")
                    .GreaterThan(0).WithMessage("Page size is greater than 0");
            }
        }

        internal class Handler : IRequestHandler<Query, GTTPageResults<ClassResponse>>
        {
            private readonly IClassRepository _classRepository;

            public Handler(IClassRepository classRepository)
            {
                _classRepository = classRepository;
            }

            public async Task<GTTPageResults<ClassResponse>> Handle(Query request, CancellationToken cancellationToken)
            {
                string filter = string.Empty;

                if (!string.IsNullOrEmpty(request.keyword))
                {
                    filter = $"WHERE Title LIKE '%{request.keyword}%'";
                }

                var result = await _classRepository.GetAllClass(request.pageSize, request.pageIndex, filter);

                return GTTPagingUtility.CreatePagedResultsQuery(
                    result.Classes,
                    request.pageIndex,
                    request.pageSize,
                    result.TotalRow);
            }
        }
    }
}
