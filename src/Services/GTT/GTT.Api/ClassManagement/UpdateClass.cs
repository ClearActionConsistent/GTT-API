using System.ComponentModel.DataAnnotations;
using System.Net;
using GTT.Application.Extensions;
using MediatR;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using GTT.Application.Commands;
using GTT.Application.Requests;
using Newtonsoft.Json;
using GTT.Application.Response;

namespace GTT_API.ClassManagement
{
    public class UpdateClass
    {
        #region Private Members
        private readonly ILogger _logger;
        private readonly IMediator _mediator;
        #endregion

        #region Constructors
        public UpdateClass(ILoggerFactory loggerFactory, IMediator mediator)
        {
            _logger = loggerFactory.CreateLogger<UpdateClass>();
            _mediator = mediator;
        }
        #endregion

        [Function("UpdateClass")]
        [OpenApiOperation(nameof(UpdateClass), "ClassManagement")]
        [OpenApiRequestBody( contentType: "application/json", bodyType: typeof(UpdateClassData), Required = true)]
        [OpenApiResponseWithBody(HttpStatusCode.OK, "application/json", bodyType: typeof(BaseResponseModel))]
        [OpenApiResponseWithBody(HttpStatusCode.BadRequest, "application/json", bodyType: typeof(BaseResponseModel))]
        [OpenApiResponseWithBody(HttpStatusCode.NotFound, "application/json", bodyType: typeof(BaseResponseModel))]
        [OpenApiResponseWithoutBody(HttpStatusCode.InternalServerError, Description = "Internal Server Error.")]
        public async Task<HttpResponseData> UpdateClassById([HttpTrigger(AuthorizationLevel.Function, "put")] HttpRequestData req)
        {
            try
            {
                _logger.LogInformation("C# HTTP trigger function processed a request.");
                var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                var data = JsonConvert.DeserializeObject<UpdateClassData>(requestBody);
                var result = await _mediator.Send(new UpdateClassInformation.Command(data));
                var response = req.CreateResponse(HttpStatusCode.OK);
                await response.WriteAsJsonAsync(result, result.Status);

                return response;
            }
            catch(ValidationException ex)
            {
                var error = $"[AzureFunction] UpdateClass - {Helpers.BuildErrorMessage(ex)}";
                _logger.LogError(error);
                var response = req.CreateResponse();
                await response.WriteAsJsonAsync(error, HttpStatusCode.BadRequest);

                return response;
            }
            catch(Exception ex)
            {
                var error = $"[AzureFunction] UpdateClass - {Helpers.BuildErrorMessage(ex)}";
                _logger.LogError(error);
                var response = req.CreateResponse();
                await response.WriteAsJsonAsync(error, HttpStatusCode.InternalServerError);

                return response;
            }
        }
    }
}
