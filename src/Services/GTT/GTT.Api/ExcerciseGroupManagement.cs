using System.Net;
using FluentValidation;
using GTT.Api.Configuration;
using GTT.Application.Commands;
using GTT.Application.Requests;
using MediatR;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using GTT.Application;
using GTT.Application.Response;
using FluentValidation.Results;

namespace GTT_API
{
    public class ExcerciseGroupManagement
    {
        #region Private Members
        private readonly ILogger _logger;
        private readonly IMediator _mediator;
        #endregion

        #region Constructors
        public ExcerciseGroupManagement(ILoggerFactory loggerFactory, IMediator mediator)
        {
            _logger = loggerFactory.CreateLogger<ExcerciseGroupManagement>();
            _mediator = mediator;
        }
        #endregion

        #region Azure Function
        [Function("CreateExcerciseGroup")]
        [OpenApiOperation(nameof(ExcerciseGroupManagement), "Excercise")]
        [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(ExGroupRequestModel), Required = true)]
        [OpenApiResponseWithBody(HttpStatusCode.OK,"application/json",bodyType:typeof(BaseResponseModel))]
        [OpenApiResponseWithBody(HttpStatusCode.BadRequest, "application/json", bodyType: typeof(BaseResponseModel))]
        [OpenApiResponseWithoutBody(HttpStatusCode.InternalServerError, Description = "Internal Server Error.")]
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = Routes.CreateExGroup)] HttpRequestData req)
        {
            try
            {
                _logger.LogInformation("C# HTTP Trigger function CreateExcerciseGroup request.");
                var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                var data = JsonConvert.DeserializeObject<ExGroupRequestModel>(requestBody);
                var result = await _mediator.Send(new CreateExGroup.Command(data));
                var respone = req.CreateResponse();
                await respone.WriteAsJsonAsync(result);

                return respone;
            }
            catch (ValidationException ex)
            {
                var error = $"[AzureFunction] CreateExcerciseGroup - {Helpers.BuildErrorMessage(ex)}";
                _logger.LogError(error);
                var response = req.CreateResponse();
                await response.WriteAsJsonAsync(error, HttpStatusCode.BadRequest);
                return response;
            }
            catch (Exception ex)
            {
                var error = $"[AzureFunction] CreateExcerciseGroup - {Helpers.BuildErrorMessage(ex)}";
                _logger.LogError(error);
                var response = req.CreateResponse();
                await response.WriteAsJsonAsync(error, HttpStatusCode.InternalServerError);
                return response;
            }
        }
        #endregion
    }
}
