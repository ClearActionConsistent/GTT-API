using FluentValidation;
using FluentValidation.Results;
using GTT.Application.Commands;
using MediatR;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using System.Net;

namespace GTT_API.FunctionHandler
{
    public class GetAllPagingChallengeV1
    {
        private readonly ILogger _logger;
        private readonly IMediator _mediator;

        public GetAllPagingChallengeV1(ILoggerFactory loggerFactory, IMediator mediator)
        {
            _logger = loggerFactory.CreateLogger<GetAllPagingChallengeV1>();
            _mediator = mediator;
        }

        [Function("GetAllPagingChallenge")]
        [OpenApiSecurity("bearer_auth", SecuritySchemeType.Http, Scheme = OpenApiSecuritySchemeType.Bearer, BearerFormat = "JWT")]
        [OpenApiOperation(nameof(GetAllPagingChallengeV1), "Test Challenge", Summary = "Search list of Challenge by Pagination", Description = "Search list of Challenge by Pagination",
            Visibility = OpenApiVisibilityType.Advanced)]

        [OpenApiParameter("PageIndex", In = ParameterLocation.Query, Required = true, Type = typeof(object))]
        [OpenApiParameter("PageSize", In = ParameterLocation.Query, Required = true, Type = typeof(object))]
        [OpenApiResponseWithBody(HttpStatusCode.Created, "application/json", typeof(List<string>))]
        [OpenApiResponseWithBody(HttpStatusCode.BadRequest, "application/json", typeof(IEnumerable<ValidationFailure>))]
        [OpenApiResponseWithoutBody(HttpStatusCode.InternalServerError, Description = "Internal Server Error.")]
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "v1/getAllPagingChallenge")] HttpRequestData req)
        {
            try
            {
                // Get request body data.
                var query = System.Web.HttpUtility.ParseQueryString(req.Url.Query);
                string pageIndex = query?.Get("PageIndex");
                string pageSize = query?.Get("PageSize");

                // Convert data from UI
                var pageindex = JsonConvert.DeserializeObject<int>(pageIndex);
                var pagesize = JsonConvert.DeserializeObject<int>(pageSize);

                var command = new GetAllPagingChallenge.Command(pageindex, pagesize);
                var challenge = await _mediator.Send(command);
                var response = req.CreateResponse(HttpStatusCode.OK);
                await response.WriteAsJsonAsync(challenge, HttpStatusCode.OK);

                return response;

            }
            catch (ValidationException ex)
            {
                var responseUnauthorized = req.CreateResponse(HttpStatusCode.BadRequest);
                await responseUnauthorized.WriteAsJsonAsync(ex.Errors, HttpStatusCode.BadRequest);
                return responseUnauthorized;
            }
            catch (Exception ex)
            {
                var response = req.CreateResponse(HttpStatusCode.InternalServerError);
                await response.WriteStringAsync("Unhandle exception has occured");
                return response;
            }

        }
    }
}
