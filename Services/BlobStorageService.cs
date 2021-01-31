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

        public string UploadFileToBlob(string strFileName, byte[] fileData, string fileMimeType, string containerName)
        {
            try
            {
                var _task = Task.Run(() => this.UploadFileToBlobAsync(strFileName, fileData, fileMimeType, containerName));
                _task.Wait();
                string fileUrl = _task.Result;
                return fileUrl;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        public async Task<bool> DeleteBlobData(int garageId, string containerName)
        {
            try
            {
                string fileName = $"{garageId}.png";

                CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(_configuration.GetConnectionString("StorageConnection"));
                CloudBlobClient cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();
                CloudBlobContainer cloudBlobContainer = cloudBlobClient.GetContainerReference(containerName);

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

        private async Task<string> UploadFileToBlobAsync(string strFileName, byte[] fileData, string fileMimeType, string containerName)
        {
            try
            {
                CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(_configuration.GetConnectionString("StorageConnection")); 
                CloudBlobClient cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();
                CloudBlobContainer cloudBlobContainer = cloudBlobClient.GetContainerReference(containerName);
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


        public async Task<bool> CopyBlob(int garageId, string container)
        {
            try
            {
                string fileName = $"{garageId}.png";

                CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(_configuration.GetConnectionString("StorageConnection"));
                CloudBlobClient cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();
                CloudBlobContainer cloudBlobContainer = cloudBlobClient.GetContainerReference(container);

                // Get the name of the first blob in the container to use as the source.
                var sourceBlob = cloudBlobContainer.GetBlobReference("99999.png");
                // Ensure that the source blob exists.
                if (await sourceBlob.ExistsAsync())
                {
                    // Lease the source blob for the copy operation 
                    // to prevent another client from modifying it.
                    var lease = sourceBlob.AcquireLeaseAsync(TimeSpan.FromSeconds(20));
                                     
                    // Get a BlobClient representing the destination blob with a unique name.
                    var destBlob = cloudBlobContainer.GetBlockBlobReference(fileName);

                    // Start the copy operation.
                    await destBlob.StartCopyAsync(sourceBlob.Uri);

                    // Update the source blob's properties.
                    await sourceBlob.FetchAttributesAsync();

                    if (sourceBlob.Properties.LeaseState == LeaseState.Leased)
                    {
                        // Break the lease on the source blob.
                        await sourceBlob.BreakLeaseAsync(TimeSpan.FromSeconds(0));
                    }

                    await DeleteBlobData(99999, "logos");
                }

                return true;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
    }
}
