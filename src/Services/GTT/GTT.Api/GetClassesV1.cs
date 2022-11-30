using System.Collections.Generic;
using System.Net;
using FluentValidation;
using FluentValidation.Results;
using GTT.Api.Configuration;
using GTT.Application.Commands;
using GTT.Application.ViewModels;
using MediatR;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Thrive.Customers.Application.Queries;

namespace GTT.API
{
    public class GetClassesV1
    {
        private readonly ILogger _logger;
        private readonly IMediator _mediator;

        public GetClassesV1(ILoggerFactory loggerFactory, IMediator mediator)
        {
            _logger = loggerFactory.CreateLogger<GetClassesV1>();
            _mediator = mediator;
        }

        [Function("GetClassesV1")]
        [OpenApiSecurity("bearer_auth", SecuritySchemeType.Http, Scheme = OpenApiSecuritySchemeType.Bearer, BearerFormat = "JWT")]
        [OpenApiOperation(nameof(GetClassesV1), "Animals", Summary = "Search list of tags by search terms", Description = "Search list of tags by search terms",
            Visibility = OpenApiVisibilityType.Important)]
        [OpenApiParameter("className", In = ParameterLocation.Query, Required = true, Type = typeof(string))]
        [OpenApiResponseWithBody(HttpStatusCode.OK, "application/json", typeof(List<object>))]
        [OpenApiResponseWithBody(HttpStatusCode.BadRequest, "application/json", typeof(IEnumerable<ValidationFailure>))]
        [OpenApiResponseWithoutBody(HttpStatusCode.InternalServerError, Description = "Internal Server Error.")]
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = Routes.GetClassesV1)] HttpRequestData req)
        {
            try
            {
                //this is sample code for sending a query
                //var classes = await _mediator.Send(new GetClasses.Query(""));
                //this is sample code for sending a command
                var command = new CreateChallange.Command(
                        new CreateChallengeData
                        {
                            Name = ""
                        }
                    );
                var challenge = await _mediator.Send(command);
                var response = req.CreateResponse(HttpStatusCode.OK);
                response.Headers.Add("Content-Type", "text/plain; charset=utf-8");
                response.WriteString("Welcome to Azure Functions!");

                return response;
            }
            catch(ValidationException ex)
            {
                var responseUnauthorized = req.CreateResponse(HttpStatusCode.BadRequest);
                await responseUnauthorized.WriteAsJsonAsync(ex.Errors, HttpStatusCode.BadRequest);
                return responseUnauthorized;
            }
            catch(Exception ex)
            {
                var response = req.CreateResponse(HttpStatusCode.InternalServerError);
                await response.WriteStringAsync("Unhandle exception has occured");
                return response;
            }
            
        }
    }
}
