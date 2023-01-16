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
                var connection = _settings.Value.AzureWebJobsStorageImage;

                var container = _settings.Value.ContainerName;

                Stream myBlob = new MemoryStream();

                var file = command.data.Files.FirstOrDefault();

                var allowUploadedImageTypes = Constants.AzureStorageImage.AllowUploadedImageFileTypes;

                if (!allowUploadedImageTypes.Any(file.ContentType.Contains))
                {
                    return new BaseResponseModel(HttpStatusCode.InternalServerError, "File type is not support for application");
                }

                var data = file.Data;
                if (ConvertBytesToMegabytes(data.Length) > Constants.AzureStorageImage.UploadedImageFileSize)
                {
                    return new BaseResponseModel(HttpStatusCode.InternalServerError, $"File Size Too Large, Must Be Less Than {Constants.AzureStorageImage.UploadedImageFileSize} MB");
                }

                await data.CopyToAsync(myBlob);
                myBlob.Position = 0;

                var blobClient = new BlobContainerClient(connection, container);

                var blob = blobClient.GetBlobClient(file.FileName);
                await blob.UploadAsync(myBlob);

                var result = new UploadImageResponse { ImageUrl = blob.Uri.AbsoluteUri };

                return new BaseResponseModel(result);
            }

            private static double ConvertBytesToMegabytes(long bytes)
            {
                return (bytes / 1024f) / 1024f;
            }
        }
    }

    
}
