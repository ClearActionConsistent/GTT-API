using GTT.Application.Services;
using GTT.Application.ViewModels;
using MediatR;

namespace GTT.Application.Commands
{
    public class CreateChallange
    {
        public record Command(
        CreateChallengeData createChallengeData
        ) : IRequest<ChallengeVM>;

        internal class Handler : IRequestHandler<Command, ChallengeVM>
        {
            public IChallengeService _service;

            public Handler(IChallengeService service)
            {
                _service = service;
            }
            public async Task<ChallengeVM> Handle(Command request, CancellationToken cancellationToken)
            {
                return await _service.CreateChallengeAsync(request.createChallengeData);
            }
        }
    }
}
