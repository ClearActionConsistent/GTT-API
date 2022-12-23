using System.Net;
using GTT.Api.Configuration;
using GTT.Application;
using GTT.Application.Queries;
using GTT.Application.Response;
using GTT.Domain.Entities;
using MediatR;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;

namespace GTT_API.ExcerciseGroupManagement
{
    public class GetExcerciseGroup
    {
        #region Private Members
        private readonly ILogger _logger;
        private readonly IMediator _mediator;
        #endregion

        #region Constructors
        public GetExcerciseGroup(ILoggerFactory loggerFactory, IMediator mediator)
        {
            _logger = loggerFactory.CreateLogger<GetExcerciseGroup>();
            _mediator = mediator;
        }
        #endregion

        [Function("GetExcerciseGroup")]
        [OpenApiOperation(nameof(GetExcerciseGroup), "Excercise")]
        [OpenApiResponseWithBody(HttpStatusCode.OK, "application/json", bodyType: typeof(ExcerciseGroup))]
        [OpenApiResponseWithBody(HttpStatusCode.NotFound, "application/json", bodyType: typeof(BaseResponseModel))]
        [OpenApiResponseWithoutBody(HttpStatusCode.InternalServerError, Description = "Internal Server Error.")]
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = Routes.GetAndCreateExGroup)] HttpRequestData req)
        {
            try
            {
                _logger.LogInformation("C# HTTP trigger function GetExcerciseGroup a request.");

                var response = req.CreateResponse();
                var result = await _mediator.Send(new GetExGroup.Query());
                await response.WriteAsJsonAsync(result);

                return response;
            }
            catch (Exception ex)
            {
                var error = $"[AzureFunction] GetExcerciseGroup - {Helpers.BuildErrorMessage(ex)}";
                _logger.LogError(error);
                var response = req.CreateResponse();
                await response.WriteAsJsonAsync(error, HttpStatusCode.InternalServerError);
                return response;
            }
        }
    }
}
