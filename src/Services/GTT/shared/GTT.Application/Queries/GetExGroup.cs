using System.Net;
using MediatR;
using GTT.Application.Interfaces.Repositories;
using GTT.Application.Response;

namespace GTT.Application.Queries
{
    public class GetExGroup
    {
        public record Query() : IRequest<BaseResponseModel>;

        internal class Handler : IRequestHandler<Query, BaseResponseModel>
        {
            private readonly IExGroupRepository _exGroupRepository;
            public Handler(IExGroupRepository exGroupRepository)
            {
                _exGroupRepository = exGroupRepository;
            }
            public async Task<BaseResponseModel> Handle(Query request, CancellationToken cancellationToken)
            {
                var result = await _exGroupRepository.GetAllExGroup();

                return result != null
                    ? new BaseResponseModel(result)
                    : new BaseResponseModel(HttpStatusCode.NotFound, "List of excercise group not found in database");
            }
        }
    }
}
