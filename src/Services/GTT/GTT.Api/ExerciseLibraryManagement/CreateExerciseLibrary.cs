using System.Net;
using FluentValidation;
using GTT.Api.Configuration;
using GTT.Application;
using GTT.Application.Commands;
using GTT.Application.Commands.ExerciseLibrary;
using GTT.Application.Requests;
using GTT.Application.Requests.ExerciseLib;
using GTT.Application.Response;
using MediatR;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace GTT_API.ExerciseLibrary
{
    public class CreateExerciseLibrary
    {
        #region Private Members
        private readonly ILogger _logger;
        private readonly IMediator _mediator;
        #endregion

        #region Constructors
        public CreateExerciseLibrary(ILoggerFactory loggerFactory, IMediator mediator)
        {
            _logger = loggerFactory.CreateLogger<CreateExerciseLibrary>();
            _mediator = mediator;
        }
        #endregion

        #region Azure Function
        [Function("CreateExerciseLibrary")]
        [OpenApiOperation(nameof(CreateExerciseLibrary), "ExcerciseLibrary")]
        [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(CreateExerciseLibRequestModel), Required = true)]
        [OpenApiResponseWithBody(HttpStatusCode.OK, "application/json", bodyType: typeof(BaseResponseModel))]
        [OpenApiResponseWithBody(HttpStatusCode.BadRequest, "application/json", bodyType: typeof(BaseResponseModel))]
        [OpenApiResponseWithoutBody(HttpStatusCode.InternalServerError, Description = "Internal Server Error.")]
        public async Task<HttpResponseData> RunAsync([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = Routes.ExerciseLibrary)] HttpRequestData req)
        {
            try
            {
                _logger.LogInformation("C# HTTP trigger function CreateExerciseLibrary request.");
                var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                var data = JsonConvert.DeserializeObject<CreateExerciseLibRequestModel>(requestBody);
                var result = await _mediator.Send(new CreateExerciseLib.Command(data));
                var respone = req.CreateResponse();
                await respone.WriteAsJsonAsync(result, result.Status);

                return respone;
            }
            catch (ValidationException ex)
            {
                var error = $"[AzureFunction] CreateExerciseLibrary - {Helpers.BuildErrorMessage(ex)}";
                _logger.LogError(error);
                var response = req.CreateResponse();
                await response.WriteAsJsonAsync(error, HttpStatusCode.InternalServerError);
                return response;
            }
            catch (Exception ex)
            {
                var error = $"[AzureFunction] CreateExerciseLibrary - {Helpers.BuildErrorMessage(ex)}";
                _logger.LogError(error);
                var response = req.CreateResponse();
                await response.WriteAsJsonAsync(error, HttpStatusCode.InternalServerError);
                return response;
            }
        }
        #endregion
    }
}
