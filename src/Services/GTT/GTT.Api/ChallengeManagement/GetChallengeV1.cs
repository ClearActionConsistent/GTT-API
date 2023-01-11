using FluentValidation;
using GTT.Api.Configuration;
using GTT.Application.Extensions;
using GTT.Application.Queries;
using GTT.Application.Response;
using GTT.Domain.Entities;
using MediatR;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System.Net;

namespace GTT_API.ChallengeManagement
{
    public class GetChallengeV1
    {
        #region Private Members
        private readonly ILogger _logger;
        private readonly IMediator _mediator;
        #endregion

        #region Constructors
        public GetChallengeV1(ILoggerFactory loggerFactory, IMediator mediator)
        {
            _logger = loggerFactory.CreateLogger<GetChallengeV1>();
            _mediator = mediator;
        }
        #endregion

        [Function("GetAllChallenge")]
        [OpenApiOperation(nameof(GetChallengeV1), "Challenge")]
        [OpenApiParameter("PageSize", In = ParameterLocation.Query, Required = false, Type = typeof(int))]
        [OpenApiParameter("PageIndex", In = ParameterLocation.Query, Required = false, Type = typeof(int))]
        [OpenApiResponseWithBody(HttpStatusCode.OK, "application/json", bodyType: typeof(GTTPageResults<ChallengeResponse>))]
        [OpenApiResponseWithBody(HttpStatusCode.NotFound, "application/json", bodyType: typeof(BaseResponseModel))]
        [OpenApiResponseWithoutBody(HttpStatusCode.InternalServerError, Description = "Internal Server Error.")]
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Function, "get", Route = Routes.GetAllChallenge)] HttpRequestData req,
            int pageSize, int pageIndex)
        {
            try
            {
                _logger.LogInformation("C# HTTP trigger function GetChallenge a request.");

                var response = req.CreateResponse();
                var result = await _mediator.Send(new GetChallenge.Query(pageSize, pageIndex));
                await response.WriteAsJsonAsync(result);

                return response;
            }
            catch (ValidationException ex)
            {
                var error = $"[AzureFunction] GetChallenge - {Helpers.BuildErrorMessage(ex)}";
                _logger.LogError(error);
                var response = req.CreateResponse();
                await response.WriteAsJsonAsync(error, HttpStatusCode.BadRequest);

                return response;
            }
            catch (Exception ex)
            {
                var error = $"[AzureFunction] GetChallenge - {Helpers.BuildErrorMessage(ex)}";
                _logger.LogError(error);
                var response = req.CreateResponse();
                await response.WriteAsJsonAsync(error, HttpStatusCode.InternalServerError);

                return response;
            }
        }
    }
}
