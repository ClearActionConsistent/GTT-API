using System.ComponentModel.DataAnnotations;
using System.Net;
using GTT.Api.Configuration;
using GTT.Application.Extensions;
using GTT.Application.Requests;
using GTT.Application.Response;
using MediatR;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace GTT_API.GroupManagement
{
    public class UpdateGroup
    {
        #region Private Members
        private readonly ILogger _logger;
        private readonly IMediator _mediator;
        #endregion

        #region Constructors
        public UpdateGroup(ILoggerFactory loggerFactory, IMediator mediator)
        {
            _logger = loggerFactory.CreateLogger<UpdateGroup>();
            _mediator = mediator;
        }
        #endregion

        [Function("UpdateGroup")]
        [OpenApiOperation(nameof(UpdateGroup), "Groups")]
        [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(UpdateGroupRequestModel), Required = true)]
        [OpenApiResponseWithBody(HttpStatusCode.OK, "application/json", bodyType: typeof(BaseResponseModel))]
        [OpenApiResponseWithBody(HttpStatusCode.BadRequest, "application/json", bodyType: typeof(BaseResponseModel))]
        [OpenApiResponseWithoutBody(HttpStatusCode.InternalServerError, Description = "Internal Server Error.")]
        public async Task<HttpResponseData> UpdateGroupFunction([HttpTrigger(AuthorizationLevel.Function, "put", Route = Routes.Group)] HttpRequestData req)
        {
            try
            {
                _logger.LogInformation("C# HTTP Trigger function UpdateGroupFunction request.");
                var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                var data = JsonConvert.DeserializeObject<UpdateGroupRequestModel>(requestBody);
                var result = await _mediator.Send(new GTT.Application.Commands.UpdateGroup.Command(data));
                var respone = req.CreateResponse();
                await respone.WriteAsJsonAsync(result, result.Status);

                return respone;
            }
            catch (ValidationException ex)
            {
                var error = $"[AzureFunction] UpdateGroupFunction - {Helpers.BuildErrorMessage(ex)}";
                _logger.LogError(error);
                var response = req.CreateResponse();
                await response.WriteAsJsonAsync(ex, HttpStatusCode.BadRequest);

                return response;
            }
            catch (Exception ex)
            {
                var error = $"[AzureFunction] UpdateGroupFunction - {Helpers.BuildErrorMessage(ex)}";
                _logger.LogError(error);
                var response = req.CreateResponse();
                await response.WriteAsJsonAsync(error, HttpStatusCode.InternalServerError);

                return response;
            }
        }
    }
}