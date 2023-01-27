using GTT.Api.Configuration;
using GTT.Application.Extensions;
using GTT.Application.Response;
using MediatR;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net;
using GTT.Application.Requests;
using GTT.Application.Commands.GroupLib;
using System.ComponentModel.DataAnnotations;

namespace GTT_API.GroupManagement
{
    public class CreateGroup
    {
        #region Private Members
        private readonly ILogger _logger;
        private readonly IMediator _mediator;
        #endregion

        #region Constructors
        public CreateGroup(ILoggerFactory loggerFactory, IMediator mediator)
        {
            _logger = loggerFactory.CreateLogger<CreateGroup>();
            _mediator = mediator;
        }
        #endregion

        #region Azure Function
        [Function("CreateGroup")]
        [OpenApiOperation(nameof(CreateGroup), "Groups")]
        [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(CreateGroupRequestModel), Required = true)]
        [OpenApiResponseWithBody(HttpStatusCode.OK, "application/json", bodyType: typeof(BaseResponseModel))]
        [OpenApiResponseWithBody(HttpStatusCode.BadRequest, "application/json", bodyType: typeof(BaseResponseModel))]
        [OpenApiResponseWithoutBody(HttpStatusCode.InternalServerError, Description = "Internal Server Error.")]
        public async Task<HttpResponseData> RunAsync([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = Routes.Group)] HttpRequestData req)
        {
            try
            {
                _logger.LogInformation("C# HTTP trigger function CreateGroup request.");
                var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                var data = JsonConvert.DeserializeObject<CreateGroupRequestModel>(requestBody);
                var result = await _mediator.Send(new CreateGroupLib.Command(data));
                var respone = req.CreateResponse();
                await respone.WriteAsJsonAsync(result, result.Status);

                return respone;
            }
            catch (ValidationException ex)
            {
                var error = $"[AzureFunction] CreateGroup - {Helpers.BuildErrorMessage(ex)}";
                _logger.LogError(error);
                var response = req.CreateResponse();
                await response.WriteAsJsonAsync(error, HttpStatusCode.InternalServerError);
                return response;
            }
            catch (Exception ex)
            {
                var error = $"[AzureFunction] CreateGroup - {Helpers.BuildErrorMessage(ex)}";
                _logger.LogError(error);
                var response = req.CreateResponse();
                await response.WriteAsJsonAsync(error, HttpStatusCode.InternalServerError);
                return response;
            }
        }
        #endregion
    }
}