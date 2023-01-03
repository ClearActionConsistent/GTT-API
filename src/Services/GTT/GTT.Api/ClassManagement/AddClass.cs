using System.Net;
using FluentValidation;
using GTT.Application.Commands;
using GTT.Application.Requests;
using MediatR;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using GTT.Application.Extensions;
using GTT.Application.Response;

namespace GTT_API.ClassManagement
{
    public class AddClass
    {
        #region Private Members
        private readonly ILogger _logger;
        private readonly IMediator _mediator;
        #endregion

        #region Constructors
        public AddClass(ILoggerFactory loggerFactory, IMediator mediator)
        {
            _logger = loggerFactory.CreateLogger<AddClass>();
            _mediator = mediator;
        }
        #endregion

        [Function("AddClass")]
        [OpenApiOperation(nameof(AddClass), "Add Class")]
        [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(CreateClassRequestModel), Required = true)]
        [OpenApiResponseWithBody(HttpStatusCode.OK, "application/json", bodyType: typeof(BaseResponseModel))]
        [OpenApiResponseWithBody(HttpStatusCode.BadRequest, "application/json", bodyType: typeof(BaseResponseModel))]
        [OpenApiResponseWithoutBody(HttpStatusCode.InternalServerError, Description = "Internal Server Error.")]
        public async Task<HttpResponseData> AddClassHandler([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequestData req)
        {
            try
            {
                _logger.LogInformation("C# HTTP Trigger function CreateClass request.");
                var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                var data = JsonConvert.DeserializeObject<CreateClassRequestModel>(requestBody);
                var result = await _mediator.Send(new CreateClass.Command(data));
                var respone = req.CreateResponse();
                await respone.WriteAsJsonAsync(result, result.Status);

                return respone;
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
                var error = $"[AzureFunction] CreateClass - {Helpers.BuildErrorMessage(ex)}";
                _logger.LogError(error);
                var response = req.CreateResponse();
                await response.WriteAsJsonAsync(error, HttpStatusCode.InternalServerError);
                return response;
            }
        }
    }
}