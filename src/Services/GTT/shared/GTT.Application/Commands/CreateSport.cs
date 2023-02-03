using FluentValidation;
using GTT.Application.Requests;
using GTT.Application.Response;
using MediatR;
using System.Net;
using GTT.Application.Interfaces.Repositories;

namespace GTT.Application.Commands
{
    public class CreateSport
    {
        public record Command(
        SportRequestModel data
        ) : IRequest<BaseResponseModel>;

        internal class Validator : AbstractValidator<Command>
        {
            public Validator()
            {
                RuleFor(x => x.data.SportName)
                     .NotEmpty().WithMessage("Sport name is required");
                RuleFor(x => x.data.SportType)
                     .NotEmpty().WithMessage("Sport type is required");
                RuleFor(x => x.data.IsActive)
                    .NotEmpty().WithMessage("Active is required");
            }
        }

        internal class Handler : IRequestHandler<Command, BaseResponseModel>
        {
            private ISportsRepository _sportsRepository;

            public Handler(ISportsRepository sportsRepository)
            {
                _sportsRepository = sportsRepository;
            }

            public async Task<BaseResponseModel> Handle(Command command, CancellationToken cancellationToken)
            {
                //handle request command to create sport information
                var result = await _sportsRepository.CreateSport(command.data);

                return result;
            }
        }
    }
}
