using System.ComponentModel.DataAnnotations;
using System.Net;
using GTT.Api.Configuration;
using GTT.Application.Extensions;
using GTT.Application.Queries;
using GTT.Application.Response;
using MediatR;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

namespace GTT_API.ClassManagement
{
    public class GetClass
    {
        #region Private Members
        private readonly ILogger _logger;
        private readonly IMediator _mediator;
        #endregion

        #region Constructors
        public GetClass(ILoggerFactory loggerFactory, IMediator mediator)
        {
            _logger = loggerFactory.CreateLogger<GetClass>();
            _mediator = mediator;
        }
        #endregion

        [Function("GetClass")]
        [OpenApiOperation(nameof(GetClass), "ClassManagement")]
        [OpenApiParameter("PageSize", In = ParameterLocation.Query, Required = false,Type = typeof(int))]
        [OpenApiParameter("PageIndex", In = ParameterLocation.Query, Required = false, Type = typeof(int))]
        [OpenApiParameter("Keyword", In = ParameterLocation.Query, Type = typeof(string))]
        [OpenApiResponseWithBody(HttpStatusCode.OK, "application/json", bodyType: typeof(GTTPageResults<ClassResponse>))]
        [OpenApiResponseWithBody(HttpStatusCode.BadRequest, "application/json", bodyType: typeof(BaseResponseModel))]
        [OpenApiResponseWithoutBody(HttpStatusCode.InternalServerError, Description = "Internal Server Error.")]
        public async Task<HttpResponseData> GetAllClass([HttpTrigger(AuthorizationLevel.Function, "get", Route = Routes.ClassV1)] HttpRequestData req,
            int pageSize, int pageIndex, string keyword)
        {
            try
            {
                _logger.LogInformation("C# HTTP trigger function GetClass a request.");

                var response = req.CreateResponse();
                var result = await _mediator.Send(new GetAllClasses.Query(pageSize, pageIndex, keyword));
                await response.WriteAsJsonAsync(result);

                return response;
            }
            catch (ValidationException ex)
            {
                var error = $"[AzureFunction] AddClass - {Helpers.BuildErrorMessage(ex)}";
                _logger.LogError(error);
                var response = req.CreateResponse();
                await response.WriteAsJsonAsync(error, HttpStatusCode.BadRequest);
                return response;
            }
            catch (Exception ex)
            {
                var error = $"[AzureFunction] GetClass - {Helpers.BuildErrorMessage(ex)}";
                _logger.LogError(error);
                var response = req.CreateResponse();
                await response.WriteAsJsonAsync(error, HttpStatusCode.InternalServerError);

                return response;
            }
        }
    }
}