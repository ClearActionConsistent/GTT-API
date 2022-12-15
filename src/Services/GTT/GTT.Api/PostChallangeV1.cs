using System.Collections.Generic;
using System.Net;
using FluentValidation;
using FluentValidation.Results;
using GTT.Api.Configuration;
using GTT.Application.Commands;
using GTT.Application.ViewModels;
using GTT.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Thrive.Customers.Application.Queries;

namespace GTT.API
{
    public class PostChallengeV1
    {
        private readonly ILogger _logger;
        private readonly IMediator _mediator;

        public PostChallengeV1(ILoggerFactory loggerFactory, IMediator mediator)
        {
            _logger = loggerFactory.CreateLogger<GetClassesV1>();
            _mediator = mediator;
        }

        [Function("PostChallenge")]
        [OpenApiSecurity("bearer_auth", SecuritySchemeType.Http, Scheme = OpenApiSecuritySchemeType.Bearer, BearerFormat = "JWT")]
        [OpenApiOperation(nameof(GetClassesV1), "Test Challenge", Summary = "Search list of tags by search terms", Description = "Search list of tags by search terms",
            Visibility = OpenApiVisibilityType.Advanced)]

        [OpenApiRequestBody("application/json", typeof(CreateChallengeData), Description = "Json request body containing")]
        [OpenApiResponseWithBody(HttpStatusCode.Created, "application/json", typeof(List<string>))]
        [OpenApiResponseWithBody(HttpStatusCode.BadRequest, "application/json", typeof(IEnumerable<ValidationFailure>))]
        [OpenApiResponseWithoutBody(HttpStatusCode.InternalServerError, Description = "Internal Server Error.")]
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "v1/createChallenge")]  HttpRequestData req, ILogger log)
        {
            try
            {

                // Get request body data.
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                dynamic data = JsonConvert.DeserializeObject(requestBody); // Get data

                // Convert data from UI
                int? calories = data?.calories;
                int? splatPoints = data?.splatPoints;
                int? avgHr = data?.avgHr;
                int? maxHr = data?.maxHr;
                int? miles = data?.miles;
                int? steps = data?.steps;



                var createChallengeData = new CreateChallengeData
                {
                    Calories = (int)calories,
                    SplatPoints = (int)splatPoints,
                    AvgHr = (int)avgHr,
                    MaxHr = (int)maxHr,
                    Miles= (int)miles,
                    Steps= (int)steps,
                };

                var command = new CreateChallange.Command(createChallengeData);
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
