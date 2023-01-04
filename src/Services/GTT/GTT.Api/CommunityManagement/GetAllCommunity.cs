using FluentValidation;
using FluentValidation.Results;
using GTT.Api.Configuration;
using GTT.Application.Extensions;
using MediatR;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System.Net;
using Thrive.Customers.Application.Queries;

namespace GTT_API.CommunityManagement
{
    public class GetAllCommunity
    {

        #region Private Members
        private readonly ILogger _logger;
        private readonly IMediator _mediator;
        #endregion

        #region Constructors
        public GetAllCommunity(ILoggerFactory loggerFactory, IMediator mediator)
        {
            _logger = loggerFactory.CreateLogger<GetAllCommunity>();
            _mediator = mediator;
        }
        #endregion

        [Function("GetAllCommunity")]
        [OpenApiSecurity("bearer_auth", SecuritySchemeType.Http, Scheme = OpenApiSecuritySchemeType.Bearer, BearerFormat = "JWT")]
        [OpenApiOperation(nameof(GetAllCommunity), "Community", Visibility = OpenApiVisibilityType.Advanced)]
        [OpenApiResponseWithBody(HttpStatusCode.Created, "application/json", typeof(List<string>))]
        [OpenApiResponseWithBody(HttpStatusCode.BadRequest, "application/json", typeof(IEnumerable<ValidationFailure>))]
        [OpenApiResponseWithoutBody(HttpStatusCode.InternalServerError, Description = "Internal Server Error.")]
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Function, "get", Route = Routes.GetAllCommunity)] HttpRequestData req)
        {
            try
            {
                _logger.LogInformation("C# HTTP trigger function GetAllCommunity a request.");

                var result = await _mediator.Send(new GetCommunity.Query());

                var response = req.CreateResponse(HttpStatusCode.OK);
                await response.WriteAsJsonAsync(result, result.Status);

                return response;
            }
            catch (ValidationException ex)
            {
                var error = $"[AzureFunction] GetAllCommunity - {Helpers.BuildErrorMessage(ex)}";
                _logger.LogError(error);
                var response = req.CreateResponse();
                await response.WriteAsJsonAsync(error, HttpStatusCode.BadRequest);

                return response;
            }
            catch (Exception ex)
            {
                var error = $"[AzureFunction] GetAllCommunity - {Helpers.BuildErrorMessage(ex)}";
                _logger.LogError(error);
                var response = req.CreateResponse();
                await response.WriteAsJsonAsync(error, HttpStatusCode.InternalServerError);

                return response;
            }
        }
    }
}
