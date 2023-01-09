using System.Net;
using FluentValidation;
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

namespace GTT_API.ExerciseLibraryManagement
{
    public class GetExerciseLibrary
    {
        #region Private Members
        private readonly ILogger _logger;
        private readonly IMediator _mediator;
        #endregion

        #region Constructors
        public GetExerciseLibrary(ILoggerFactory loggerFactory, IMediator mediator)
        {
            _logger = loggerFactory.CreateLogger<GetExerciseLibrary>();
            _mediator = mediator;
        }
        #endregion

        [Function("GetExerciseLibrary")]
        [OpenApiOperation(nameof(GetExerciseLibrary), "ExcerciseLibrary")]
        [OpenApiParameter("PageSize", In = ParameterLocation.Query, Required = false, Type = typeof(int))]
        [OpenApiParameter("PageIndex", In = ParameterLocation.Query, Required = false, Type = typeof(int))]
        [OpenApiParameter("Keyword", In = ParameterLocation.Query, Required = false, Type = typeof(string))]
        [OpenApiResponseWithBody(HttpStatusCode.OK, "application/json", bodyType: typeof(GTTPageResults<ExerciseLibraryResponse>))]
        [OpenApiResponseWithBody(HttpStatusCode.NotFound, "application/json", bodyType: typeof(BaseResponseModel))]
        [OpenApiResponseWithoutBody(HttpStatusCode.InternalServerError, Description = "Internal Server Error.")]
        public async Task<HttpResponseData> RunAsync([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = Routes.ExerciseLibrary)] HttpRequestData req,
            int pageSize, int pageIndex, string keyword)
        {
            try
            {
                _logger.LogInformation("C# HTTP trigger function GetExerciseLibrary a request.");

                var response = req.CreateResponse();
                var result = await _mediator.Send(new GetExLibrary.Query(pageSize, pageIndex, keyword));
                await response.WriteAsJsonAsync(result);

                return response;
            }
            catch (ValidationException ex)
            {

                var error = $"[AzureFunction] GetExerciseLibrary - {Helpers.BuildErrorMessage(ex)}";
                _logger.LogError(error);
                var response = req.CreateResponse();
                await response.WriteAsJsonAsync(error, HttpStatusCode.BadRequest);

                return response;
            }
            catch (Exception ex)
            {
                var error = $"[AzureFunction] GetExerciseLibrary - {Helpers.BuildErrorMessage(ex)}";
                _logger.LogError(error);
                var response = req.CreateResponse();
                await response.WriteAsJsonAsync(error, HttpStatusCode.InternalServerError);

                return response;
            }
        }
    }
}
