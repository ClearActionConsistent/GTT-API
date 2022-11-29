using FluentValidation;
using MediatR;
using GTT.Application.Services;
using GTT.Application.ViewModels;

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
                    .NotNull().WithMessage("Class name is required");
            }
        }

        internal class Handler : IRequestHandler<Query, List<ClassVM>>
        {
            private readonly IClassService _service;
            public Handler(IClassService service)
            {
                _service = service;
            }
            public async Task<List<ClassVM>> Handle(Query request, CancellationToken cancellationToken)
            {
                return await _service.GetClassesByNameAsync(request.name);
            }
        }
     
    }
}
