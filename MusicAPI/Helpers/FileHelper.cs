using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Http;

namespace MusicAPI.Helpers
{
    //Static indica que estos metodos se invocan directamente desde otras funciones
    public static class FileHelper
    {
        public static async Task<string> UploadImage(IFormFile file)
        {
            //Azure connection string
            string ConnectionString = "DefaultEndpointsProtocol=https;AccountName=laarmusicstorage;AccountKey=ZD+WhQ9CY3zEif7qLU6hgca/i8JIthBoWU01iB65NBPpIxSdXJG5VRqhN0XDT/9h1vEnqWwFHgaSFnswj0LEPA==;EndpointSuffix=core.windows.net";
            string ContainerName = "songscover";

            //inicio de upload del file a Azure
            BlobContainerClient blobContainerClient = new BlobContainerClient(ConnectionString, ContainerName);
            BlobClient blobClient = blobContainerClient.GetBlobClient(file.FileName);

            var memoryStream = new MemoryStream();
            await file.CopyToAsync(memoryStream);
            memoryStream.Position = 0;
            await blobClient.UploadAsync(memoryStream);
            //fin de upload del file a Azure
            return blobClient.Uri.AbsoluteUri;
        }

        public static async Task<string> UploadAudio(IFormFile file)
        {
            //Azure connection string
            string ConnectionString = "DefaultEndpointsProtocol=https;AccountName=laarmusicstorage;AccountKey=ZD+WhQ9CY3zEif7qLU6hgca/i8JIthBoWU01iB65NBPpIxSdXJG5VRqhN0XDT/9h1vEnqWwFHgaSFnswj0LEPA==;EndpointSuffix=core.windows.net";
            string ContainerName = "audiofiles";

            //inicio de upload del file a Azure
            BlobContainerClient blobContainerClient = new BlobContainerClient(ConnectionString, ContainerName);
            BlobClient blobClient = blobContainerClient.GetBlobClient(file.FileName);

            var memoryStream = new MemoryStream();
            await file.CopyToAsync(memoryStream);
            memoryStream.Position = 0;
            await blobClient.UploadAsync(memoryStream);
            //fin de upload del file a Azure
            return blobClient.Uri.AbsoluteUri;
        }
    }
}
