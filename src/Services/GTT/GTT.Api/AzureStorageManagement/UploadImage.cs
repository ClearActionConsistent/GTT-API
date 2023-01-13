using System.Net;
using GTT.Api.Configuration;
using GTT.Application.Commands;
using GTT.Application.Extensions;
using GTT.Application.Requests;
using GTT.Application.Response;
using GTT_API.ExcerciseGroupManagement.ExcerciseGroupManagement;
using HttpMultipartParser;
using MediatR;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;

namespace GTT_API.AzureStorageManagement
{
    public class UploadImage
    {
        #region Private Members
        private readonly ILogger _logger;
        private readonly IMediator _mediator;
        #endregion

        #region Constructors
        public UploadImage(ILoggerFactory loggerFactory, IMediator mediator)
        {
            _logger = loggerFactory.CreateLogger<GetExerciseGroup>();
            _mediator = mediator;
        }
        #endregion

        [OpenApiOperation(nameof(UploadImage), "AzureStorage")]
        [OpenApiRequestBody(contentType: "multipart/form-data", bodyType: typeof(MultiPartFormDataModel), Required = true, Description = "Image data")]
        [OpenApiResponseWithBody(HttpStatusCode.OK, "application/json", bodyType: typeof(BaseResponseModel))]
        [OpenApiResponseWithBody(HttpStatusCode.BadRequest, "application/json", bodyType: typeof(BaseResponseModel))]
        [OpenApiResponseWithoutBody(HttpStatusCode.InternalServerError, Description = "Internal Server Error.")]
        [Function("UploadImage")]
        public async Task<HttpResponseData> RunAsync([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = Routes.UploadImage)] HttpRequestData req)
        {
            try
            {
                _logger.LogInformation("C# HTTP trigger function UploadImage a request.");

                var response = req.CreateResponse();

                var parsedFormBody = await MultipartFormDataParser.ParseAsync(req.Body);

                var result = await _mediator.Send(new UploadImageStorage.Command(parsedFormBody));
                await response.WriteAsJsonAsync(result);

                return response;
            }
            catch (Exception ex)
            {
                var error = $"[AzureFunction] UploadImage - {Helpers.BuildErrorMessage(ex)}";
                _logger.LogError(error);
                var response = req.CreateResponse();
                await response.WriteAsJsonAsync(error, HttpStatusCode.InternalServerError);

                return response;
            }
        }
    }
}
