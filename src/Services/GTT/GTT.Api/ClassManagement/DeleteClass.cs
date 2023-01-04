using System.ComponentModel.DataAnnotations;
using System.Net;
using GTT.Api.Configuration;
using GTT.Application.Commands;
using GTT.Application.Extensions;
using GTT.Application.Response;
using MediatR;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

namespace GTT_API.ClassManagement
{
    public class DeleteClass
    {
        #region Private Members
        private readonly ILogger _logger;
        private readonly IMediator _mediator;
        #endregion

        #region Constructors
        public DeleteClass(ILoggerFactory loggerFactory, IMediator mediator)
        {
            _logger = loggerFactory.CreateLogger<DeleteClass>();
            _mediator = mediator;
        }
        #endregion

        [Function("DeleteClass")]
        [OpenApiOperation(nameof(DeleteClass), "ClassManagement")]
        [OpenApiParameter("classId", In = ParameterLocation.Query, Required = false, Type = typeof(int))]
        [OpenApiResponseWithBody(HttpStatusCode.OK, "application/json", bodyType: typeof(BaseResponseModel))]
        [OpenApiResponseWithBody(HttpStatusCode.NotFound, "application/json", bodyType: typeof(BaseResponseModel))]
        [OpenApiResponseWithoutBody(HttpStatusCode.InternalServerError, Description = "Internal Server Error.")]
        public async Task<HttpResponseData> DeleteClassById([HttpTrigger(AuthorizationLevel.Function, "delete", Route = Routes.ClassV1)] HttpRequestData req, int classId)
        {
            try
            {
                _logger.LogInformation("C# HTTP trigger function processed a request.");

                var response = req.CreateResponse(HttpStatusCode.OK);
                var result = await _mediator.Send(new RemoveClass.Command(classId));
                await response.WriteAsJsonAsync(result, result.Status);

                return response;
            }
            catch (ValidationException ex)
            {
                var error = $"[AzureFunction] DeleteClass - {Helpers.BuildErrorMessage(ex)}";
                _logger.LogError(error);
                var response = req.CreateResponse();
                await response.WriteAsJsonAsync(error, HttpStatusCode.BadRequest);

                return response;
            }
            catch (Exception ex)
            {
                var error = $"[AzureFunction] DeleteClass - {Helpers.BuildErrorMessage(ex)}";
                _logger.LogError(error);
                var response = req.CreateResponse();
                await response.WriteAsJsonAsync(error, HttpStatusCode.InternalServerError);

                return response;
            }
        }
    }
}