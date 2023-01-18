using System.Net;
using FluentValidation;
using GTT.Api.Configuration;
using GTT.Application.Extensions;
using GTT.Application.Response;
using MediatR;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

namespace GTT_API.SportsManagement
{
    public class GetSports
    {
        #region Private Members
        private readonly ILogger _logger;
        private readonly IMediator _mediator;
        #endregion

        #region Constructors
        public GetSports(ILoggerFactory loggerFactory, IMediator mediator)
        {
            _logger = loggerFactory.CreateLogger<GetSports>();
            _mediator = mediator;
        }
        #endregion

        [Function("GetListSports")]
        [OpenApiOperation(nameof(GetSports), "Sports")]
        [OpenApiParameter("PageIndex", In = ParameterLocation.Query, Required = true, Type = typeof(int))]
        [OpenApiParameter("PageSize", In = ParameterLocation.Query, Required = true, Type = typeof(int))]
        [OpenApiResponseWithBody(HttpStatusCode.OK, "application/json", bodyType: typeof(GTTPageResults<ListSportsResponse>))]
        [OpenApiResponseWithBody(HttpStatusCode.BadRequest, "application/json", bodyType: typeof(GTTPageResults<>))]
        [OpenApiResponseWithoutBody(HttpStatusCode.InternalServerError, Description = "Internal Server Error.")]
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = Routes.Sports)] HttpRequestData req,
            int pageIndex, int pageSize)
        {
            try
            {
                _logger.LogInformation("C# HTTP trigger GetListSports function processed a request.");

                var result = await _mediator.Send(new GTT.Application.Queries.GetSports.Query(pageIndex, pageSize));
                var response = req.CreateResponse();
                await response.WriteAsJsonAsync(result);

                return response;
            }
            catch (ValidationException ex)
            {
                var error = $"[AzureFunction] GetListSportsFunction - {Helpers.BuildErrorMessage(ex)}";
                _logger.LogError(error);
                var response = req.CreateResponse();
                await response.WriteAsJsonAsync(ex, HttpStatusCode.BadRequest);

                return response;
            }
            catch (Exception ex)
            {
                var error = $"[AzureFunction] GetListSportsFunction - {Helpers.BuildErrorMessage(ex)}";
                _logger.LogError(error);
                var response = req.CreateResponse();
                await response.WriteAsJsonAsync(error, HttpStatusCode.InternalServerError);

                return response;
            }
        }
    }
}
