using GTT.Application.ViewModels;
using GTT.Application.Repositories;
using MediatR;
using FluentValidation;
using GTT.Application.Responses;

namespace GTT.Application.Queries
{
    public class GetListClass
    {
        public record Query(
            int activityId
        ) : IRequest<ListResponse<ClassesVM>>;

        public class Validator : AbstractValidator<Query>
        {
            public Validator()
            {
                RuleFor(x => x.activityId)
                    .NotNull().WithMessage("Id can not be null")
                    .GreaterThanOrEqualTo(1).WithMessage("Id must be greater than 0");
            }
        }

        public class Handler : IRequestHandler<Query, ListResponse<ClassesVM>>
        {
            private readonly IClassRepository _repo;
            public Handler(IClassRepository repo)
            {
                _repo = repo;
            }

            public async Task<ListResponse<ClassesVM>> Handle(Query request, CancellationToken cancellationToken)
            {
                var rs = await _repo.GetListClassByActivity(request.activityId);
                return rs;
            }
        }

    }
}
