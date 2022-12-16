using System.Net;
using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using GTT.Api.Configuration;
using GTT.Application.Queries;
using MediatR;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;



namespace GTT_API.FunctionHandler
{
    public class GetClassV1
    {
        private readonly ILogger _logger;
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public GetClassV1(ILoggerFactory loggerFactory, IMediator mediator, IMapper mapper)
        {
            _logger = loggerFactory.CreateLogger<GetClassV1>();
            _mediator = mediator;
            _mapper = mapper;
        }

        [Function("GetClassV1")]
        [OpenApiSecurity("bearer_auth", SecuritySchemeType.Http, Scheme = OpenApiSecuritySchemeType.Bearer, BearerFormat = "JWT")]

        [OpenApiOperation(nameof(GetClassV1), "Class", Summary = "Search list of tags by search terms", Description = "Search list Class",
            Visibility = OpenApiVisibilityType.Important)]
        [OpenApiParameter("actId", In = ParameterLocation.Query, Required = true, Type = typeof(object))]
        [OpenApiResponseWithBody(HttpStatusCode.OK, "application/json", typeof(List<object>), Description = "Return a List of object")]
        [OpenApiResponseWithBody(HttpStatusCode.BadRequest, "application/json", typeof(IEnumerable<ValidationFailure>))]
        [OpenApiResponseWithoutBody(HttpStatusCode.InternalServerError, Description = "Internal Server Error.")]
        public async Task<HttpResponseData> GetClass([HttpTrigger(AuthorizationLevel.Function, "get", Route = Routes.GetClassV1)] HttpRequestData req)
        {
            try
            {
                _logger.LogInformation("C# HTTP trigger function processed a request.");

                var query = System.Web.HttpUtility.ParseQueryString(req.Url.Query);
                string userid = query?.Get("actId");

                var id = JsonConvert.DeserializeObject<int>(userid);
                var classes = await _mediator.Send(new GetListClass.Query(id));
              
                var response = req.CreateResponse(HttpStatusCode.OK);
                await response.WriteAsJsonAsync(classes, classes.HttpStatus == 200 ? HttpStatusCode.OK : HttpStatusCode.BadRequest);

                return response;
            }
            catch (ValidationException ex)
            {
                var responseValidation = req.CreateResponse(HttpStatusCode.BadRequest);
                await responseValidation.WriteAsJsonAsync(ex.Errors, HttpStatusCode.BadRequest);
                return responseValidation;
            }
            catch (Exception ex)
            {
                var response = req.CreateResponse(HttpStatusCode.InternalServerError);
                await response.WriteStringAsync($"{ex.Message}");
                return response;
            }
        }

    }
}
