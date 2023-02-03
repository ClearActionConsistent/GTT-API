using System.Net;
using FluentValidation;
using GTT.Api.Configuration;
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
using Microsoft.OpenApi.Models;

namespace GTT_API.SportsManagement
{
    public class UpdateSport
    {
        #region Private Members
        private readonly ILogger _logger;
        private readonly IMediator _mediator;
        #endregion

        #region Constructors
        public UpdateSport(ILoggerFactory loggerFactory, IMediator mediator)
        {
            _logger = loggerFactory.CreateLogger<UpdateSport>();
            _mediator = mediator;
        }
        #endregion

        #region Azure Function
        [Function("UpdateSport")]
        [OpenApiOperation(nameof(UpdateSport), "Sports")]
        [OpenApiParameter("id", Required = true, In = ParameterLocation.Path, Type = typeof(int))]
        [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(SportRequestModel), Required = true)]
        [OpenApiResponseWithBody(HttpStatusCode.OK, "application/json", bodyType: typeof(BaseResponseModel))]
        [OpenApiResponseWithBody(HttpStatusCode.BadRequest, "application/json", bodyType: typeof(BaseResponseModel))]
        [OpenApiResponseWithoutBody(HttpStatusCode.InternalServerError, Description = "Internal Server Error.")]
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = Routes.UpdateSportById)] HttpRequestData req, int id)
         {
            try
            {
                _logger.LogInformation("C# HTTP Trigger function UpdateSportsFunction request.");
                var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                var data = JsonConvert.DeserializeObject<SportRequestModel>(requestBody);
                var result = await _mediator.Send(new GTT.Application.Commands.UpdateSport.Command(id, data));
                var respone = req.CreateResponse();
                await respone.WriteAsJsonAsync(result, result.Status);

                return respone;
            }
            catch (ValidationException ex)
            {
                var error = $"[AzureFunction] UpdateSportFunction - {Helpers.BuildErrorMessage(ex)}";
                _logger.LogError(error);
                var response = req.CreateResponse();
                await response.WriteAsJsonAsync(ex, HttpStatusCode.BadRequest);

                return response;
            }
            catch (Exception ex)
            {
                var error = $"[AzureFunction] UpdateSportFunction - {Helpers.BuildErrorMessage(ex)}";
                _logger.LogError(error);
                var response = req.CreateResponse();
                await response.WriteAsJsonAsync(error, HttpStatusCode.InternalServerError);

                return response;
            }
        }
        #endregion
    }
}
