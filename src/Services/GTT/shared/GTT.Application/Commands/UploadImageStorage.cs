using Azure.Storage.Blobs;
using GTT.Application.Response;
using GTT.Application.StorageOptionsModel;
using GTT.Application.Utils;
using GTT.Domain.Entities;
using HttpMultipartParser;
using MediatR;
using Microsoft.Extensions.Options;
using System.Net;

namespace GTT.Application.Commands
{
    public class UploadImageStorage
    {
        public record Command(
        MultipartFormDataParser data
        ) : IRequest<BaseResponseModel>;

        internal class Handler : IRequestHandler<Command, BaseResponseModel>
        {
            private readonly IOptions<AzureStorageOptions> _settings;

            public Handler(IOptions<AzureStorageOptions> settings)
            {
                _settings = settings;
            }
            public async Task<BaseResponseModel> Handle(Command command, CancellationToken cancellationToken)
            {
                var connection = _settings.Value.AzureWebJobsStorage;

                var container = _settings.Value.ContainerName;

                Stream myBlob = new MemoryStream();

                var file = command.data.Files.FirstOrDefault();

                if (!file.ContentType.Contains(Constants.AzureStorageImage.Type))
                {
                    return new BaseResponseModel(HttpStatusCode.InternalServerError, "File Type Should Be Image");
                }

                var data = file.Data;
                if (ConvertBytesToMegabytes(data.Length) > Constants.AzureStorageImage.Size)
                {
                    return new BaseResponseModel(HttpStatusCode.InternalServerError, "File Size Too Large, Must Be Less Than 10MB");
                }

                await data.CopyToAsync(myBlob);
                myBlob.Position = 0;

                var blobClient = new BlobContainerClient(connection, container);

                var blob = blobClient.GetBlobClient(file.FileName);
                await blob.UploadAsync(myBlob);

                var result = new UploadImage { ImageUrl = blob.Uri.AbsoluteUri };

                return new BaseResponseModel(result);
            }

            static double ConvertBytesToMegabytes(long bytes)
            {
                return (bytes / 1024f) / 1024f;
            }
        }
    }

    
}
