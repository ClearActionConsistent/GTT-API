using FluentValidation;
using GTT.Application.Interfaces.Repositories;
using GTT.Application.Response;
using MediatR;

namespace Thrive.Customers.Application.Queries
{
    public class GetCommunity
    {
        public record Query() : IRequest<BaseResponseModel>;

        internal class Handler : IRequestHandler<Query, BaseResponseModel>
        {
            private readonly ICommunityRepository _communityRepository;
            public Handler(ICommunityRepository communityRepository)
            {
                _communityRepository = communityRepository;
            }
            public async Task<BaseResponseModel> Handle(Query request, CancellationToken cancellationToken)
            {
                var result = await _communityRepository.GetAllCommunity();
                return result;
            }
        }
    }
}
