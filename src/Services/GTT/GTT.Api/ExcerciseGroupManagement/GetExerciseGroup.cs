using System.Net;
using FluentValidation;
using GTT.Api.Configuration;
using GTT.Application.Extensions;
using GTT.Application.Queries;
using GTT.Application.Response;
using GTT.Domain.Entities;
using MediatR;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

namespace GTT_API.ExcerciseGroupManagement.ExcerciseGroupManagement
{
    public class GetExerciseGroup
    {
        #region Private Members
        private readonly ILogger _logger;
        private readonly IMediator _mediator;
        #endregion

        #region Constructors
        public GetExerciseGroup(ILoggerFactory loggerFactory, IMediator mediator)
        {
            _logger = loggerFactory.CreateLogger<GetExerciseGroup>();
            _mediator = mediator;
        }
        #endregion

        [Function("GetExcerciseGroup")]
        [OpenApiOperation(nameof(GetExerciseGroup), "Excercise")]
        [OpenApiParameter("PageSize", In = ParameterLocation.Query, Required = false, Type = typeof(int))]
        [OpenApiParameter("PageIndex", In = ParameterLocation.Query, Required = false, Type = typeof(int))]
        [OpenApiParameter("Keyword", In = ParameterLocation.Query, Required = false, Type = typeof(string))]
        [OpenApiResponseWithBody(HttpStatusCode.OK, "application/json", bodyType: typeof(GTTPageResults<ExerciseGroupResponse>))]
        [OpenApiResponseWithBody(HttpStatusCode.NotFound, "application/json", bodyType: typeof(BaseResponseModel))]
        [OpenApiResponseWithoutBody(HttpStatusCode.InternalServerError, Description = "Internal Server Error.")]
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Function, "get", Route = Routes.GetExGroup)] HttpRequestData req,
            int pageSize, int pageIndex, string keyword)
        {
            try
            {
                _logger.LogInformation("C# HTTP trigger function GetExcerciseGroup a request.");

                var response = req.CreateResponse();
                var result = await _mediator.Send(new GetExGroup.Query(pageSize, pageIndex, keyword));
                await response.WriteAsJsonAsync(result);

                return response;
            }
            catch (ValidationException ex)
            {
                var error = $"[AzureFunction] GetExcerciseGroup - {Helpers.BuildErrorMessage(ex)}";
                _logger.LogError(error);
                var response = req.CreateResponse();
                await response.WriteAsJsonAsync(error, HttpStatusCode.BadRequest);

                return response;
            }
            catch (Exception ex)
            {
                var error = $"[AzureFunction] GetExcerciseGroup - {Helpers.BuildErrorMessage(ex)}";
                _logger.LogError(error);
                var response = req.CreateResponse();
                await response.WriteAsJsonAsync(error, HttpStatusCode.InternalServerError);

                return response;
            }
        }
    }
}
