using Microsoft.AspNetCore.Mvc.Rendering;
using OCHPlanner3.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OCHPlanner3.Services.Interfaces
{
    public interface IBlobStorageService
    {
        string UploadFileToBlob(string strFileName, byte[] fileData, string fileMimeType, string containerName);
        Task<bool> DeleteBlobData(int garageId, string containerName);
        Task<bool> CopyBlob(int garageId, string container);
    }
}
