using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using OCHPlanner3.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OCHPlanner3.Services
{
    public class BlobStorageService : IBlobStorageService
    {
        private readonly IConfiguration _configuration;

        public BlobStorageService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string UploadFileToBlob(string strFileName, byte[] fileData, string fileMimeType)
        {
            try
            {
                var _task = Task.Run(() => this.UploadFileToBlobAsync(strFileName, fileData, fileMimeType));
                _task.Wait();
                string fileUrl = _task.Result;
                return fileUrl;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        public async Task<bool> DeleteBlobData(int garageId)
        {
            try
            {
                string fileName = $"{garageId}.png";

                CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(_configuration.GetConnectionString("StorageConnection"));
                CloudBlobClient cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();
                string strContainerName = "logos";
                CloudBlobContainer cloudBlobContainer = cloudBlobClient.GetContainerReference(strContainerName);

                if (await cloudBlobContainer.ExistsAsync())
                {
                    CloudBlob file = cloudBlobContainer.GetBlobReference(fileName);

                    if (await file.ExistsAsync())
                    {
                        await file.DeleteAsync();
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }



        private async Task<string> UploadFileToBlobAsync(string strFileName, byte[] fileData, string fileMimeType)
        {
            try
            {
                CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(_configuration.GetConnectionString("StorageConnection")); 
                CloudBlobClient cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();
                string strContainerName = "logos";
                CloudBlobContainer cloudBlobContainer = cloudBlobClient.GetContainerReference(strContainerName);
                string fileName = strFileName;

                if (fileName != null && fileData != null)
                {
                    CloudBlockBlob cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference(fileName);
                    cloudBlockBlob.Properties.ContentType = fileMimeType;
                    await cloudBlockBlob.UploadFromByteArrayAsync(fileData, 0, fileData.Length);
                    return cloudBlockBlob.Uri.AbsoluteUri;
                }
                return "";
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
    }
}
