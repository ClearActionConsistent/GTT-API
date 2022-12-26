using FluentValidation;
using FluentValidation.Results;
using GTT.Api.Configuration;
using GTT.Application.Commands;
using GTT.Application.Requests;
using MediatR;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using System.Net;

namespace GTT_API.ChallengeManagement
{
    public class InsertChallengeV1
    {
        private readonly ILogger _logger;
        private readonly IMediator _mediator;

        public InsertChallengeV1(ILoggerFactory loggerFactory, IMediator mediator)
        {
            _logger = loggerFactory.CreateLogger<InsertChallengeV1>();
            _mediator = mediator;
        }

        [Function("InsertChallenge")]
        [OpenApiSecurity("bearer_auth", SecuritySchemeType.Http, Scheme = OpenApiSecuritySchemeType.Bearer, BearerFormat = "JWT")]
        [OpenApiOperation(nameof(InsertChallengeV1), "Challenge", Visibility = OpenApiVisibilityType.Advanced)]

        [OpenApiRequestBody("application/json", typeof(CreateChallengeData), Description = "Json request body containing")]
        [OpenApiResponseWithBody(HttpStatusCode.Created, "application/json", typeof(List<string>))]
        [OpenApiResponseWithBody(HttpStatusCode.BadRequest, "application/json", typeof(IEnumerable<ValidationFailure>))]
        [OpenApiResponseWithoutBody(HttpStatusCode.InternalServerError, Description = "Internal Server Error.")]
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = Routes.CreateChallenge)] HttpRequestData req)
        {
            try
            {
                _logger.LogInformation("C# HTTP Trigger function CreateChallenge request.");

                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                dynamic data = JsonConvert.DeserializeObject<CreateChallengeData>(requestBody);       

                var command = new CreateChallange.Command(data);
                var challenge = await _mediator.Send(command);

                var response = req.CreateResponse(HttpStatusCode.OK);
                await response.WriteAsJsonAsync(challenge);

                return response;

            }
            catch (ValidationException ex)
            {
                var responseUnauthorized = req.CreateResponse(HttpStatusCode.BadRequest);
                await responseUnauthorized.WriteAsJsonAsync(ex.Errors, HttpStatusCode.BadRequest);
                return responseUnauthorized;
            }
            catch (Exception ex)
            {
                var response = req.CreateResponse(HttpStatusCode.InternalServerError);
                await response.WriteStringAsync("Unhandle exception has occured");
                return response;
            }

        }
    }
}
