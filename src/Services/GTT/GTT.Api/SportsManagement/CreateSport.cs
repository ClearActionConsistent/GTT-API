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
using GTT.Application.Extensions;
using GTT.Application.Response;

namespace GTT_API.ExcerciseGroupManagement
{
    public class CreateSport
    {
        #region Private Members
        private readonly ILogger _logger;
        private readonly IMediator _mediator;
        #endregion

        #region Constructors
        public CreateSport(ILoggerFactory loggerFactory, IMediator mediator)
        {
            _logger = loggerFactory.CreateLogger<CreateSport>();
            _mediator = mediator;
        }
        #endregion

        #region Azure Function
        [Function("CreateSport")]
        [OpenApiOperation(nameof(CreateSport), "Sports")]
        [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(CreateSportRequestModel), Required = true)]
        [OpenApiResponseWithBody(HttpStatusCode.OK, "application/json", bodyType: typeof(BaseResponseModel))]
        [OpenApiResponseWithBody(HttpStatusCode.BadRequest, "application/json", bodyType: typeof(BaseResponseModel))]
        [OpenApiResponseWithoutBody(HttpStatusCode.InternalServerError, Description = "Internal Server Error.")]
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = Routes.CreateSports)] HttpRequestData req)
        {
            try
            {
                _logger.LogInformation("C# HTTP Trigger function CreateSportsFunction request.");
                var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                var data = JsonConvert.DeserializeObject<CreateSportRequestModel>(requestBody);
                var result = await _mediator.Send(new GTT.Application.Commands.CreateSport.Command(data));
                var respone = req.CreateResponse();
                await respone.WriteAsJsonAsync(result, result.Status);

                return respone;
            }
            catch (ValidationException ex)
            {
                var error = $"[AzureFunction] CreateSportsFunction - {Helpers.BuildErrorMessage(ex)}";
                _logger.LogError(error);
                var response = req.CreateResponse();
                await response.WriteAsJsonAsync(ex, HttpStatusCode.BadRequest);

                return response;
            }
            catch (Exception ex)
            {
                var error = $"[AzureFunction] CreateSportsFunction - {Helpers.BuildErrorMessage(ex)}";
                _logger.LogError(error);
                var response = req.CreateResponse();
                await response.WriteAsJsonAsync(error, HttpStatusCode.InternalServerError);

                return response;
            }
        }
        #endregion
    }
}
