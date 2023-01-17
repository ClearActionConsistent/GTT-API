using System.Net;
using FluentValidation;
using GTT.Api.Configuration;
using GTT.Application.Extensions;
using GTT.Application.Response;
using MediatR;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

namespace GTT_API.GroupManagement
{
    public class GetGroups
    {
        #region Private Members
        private readonly ILogger _logger;
        private readonly IMediator _mediator;
        #endregion

        #region Constructors
        public GetGroups(ILoggerFactory loggerFactory, IMediator mediator)
        {
            _logger = loggerFactory.CreateLogger<GetGroups>();
            _mediator = mediator;
        }
        #endregion

        [Function("GetGroup")]
        [OpenApiOperation(nameof(GetGroups), "Groups")]
        [OpenApiParameter("PageSize", In = ParameterLocation.Query, Required = false, Type = typeof(int))]
        [OpenApiParameter("PageIndex", In = ParameterLocation.Query, Required = false, Type = typeof(int))]
        [OpenApiResponseWithBody(HttpStatusCode.OK, "application/json", bodyType: typeof(GTTPageResults<GroupsResponse>))]
        [OpenApiResponseWithBody(HttpStatusCode.NotFound, "application/json", bodyType: typeof(BaseResponseModel))]
        [OpenApiResponseWithoutBody(HttpStatusCode.InternalServerError, Description = "Internal Server Error.")]
        public async Task<HttpResponseData> RunAsync([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = Routes.Group)] HttpRequestData req,
            int pageSize, int pageIndex)
        {
            try
            {
                _logger.LogInformation("C# HTTP trigger function GetGroup a request.");

                var response = req.CreateResponse();
                var result = await _mediator.Send(new GTT.Application.Queries.GetGroups.Query(pageSize, pageIndex));
                await response.WriteAsJsonAsync(result);

                return response;
            }
            catch (ValidationException ex)
            {
                var error = $"[AzureFunction] GetGroup - {Helpers.BuildErrorMessage(ex)}";
                _logger.LogError(error);
                var response = req.CreateResponse();
                await response.WriteAsJsonAsync(error, HttpStatusCode.BadRequest);

                return response;
            }
            catch (Exception ex)
            {
                var error = $"[AzureFunction] GetGroup - {Helpers.BuildErrorMessage(ex)}";
                _logger.LogError(error);
                var response = req.CreateResponse();
                await response.WriteAsJsonAsync(error, HttpStatusCode.InternalServerError);

                return response;
            }
        }
    }
}
