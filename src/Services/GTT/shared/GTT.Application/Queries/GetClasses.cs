using FluentValidation;
using GTT.Application;
using GTT.Application.Repositories;
using GTT.Application.ViewModels;
using MediatR;

namespace Thrive.Customers.Application.Queries
{
    public class GetClasses
    {
        public record Query(
        string name
        ) : IRequest<List<ClassVM>>;

        internal class Validator : AbstractValidator<Query>
        {
            public Validator()
            {
                RuleFor(x => x.name)
                    .NotEmpty().WithMessage("Class name is not empty")
                    .NotNull().WithMessage("Class name is required");
            }
        }

        internal class Handler : IRequestHandler<Query, List<ClassVM>>
        {
            private readonly IClassRepository _repo;
            public Handler(IClassRepository repo)
            {
                _repo = repo;
            }
            public async Task<List<ClassVM>> Handle(Query request, CancellationToken cancellationToken)
            {
                
                return new List<ClassVM>();
            }
        }
     
    }
}
