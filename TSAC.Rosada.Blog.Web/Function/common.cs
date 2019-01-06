using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace TSAC.Rosada.Blog.Web.Function
{
    public class common
    {
        public static string _azureConnection;

        public common(IConfiguration configuration)
        {
            _azureConnection = configuration.GetConnectionString("AzureConnection");
        }

        public static async Task InsertPhoto(IFormFile image, string filename)
        {
            //directory

            var file = Path.Combine(
                Directory.GetCurrentDirectory(),
                "wwwroot", "files", filename);
            using (var stream = new FileStream(file, FileMode.Create))
            {
                await image.CopyToAsync(stream);
            }


            //azure
            string storageConnection = CloudConfigurationManager.GetSetting(_azureConnection);

            CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(storageConnection);

            CloudBlobClient cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();

            CloudBlobContainer cloudBlobContainer = cloudBlobClient.GetContainerReference("image");

            //create a container if it is not already exists

            if (await cloudBlobContainer.CreateIfNotExistsAsync())
            {

                await cloudBlobContainer.SetPermissionsAsync(new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Blob });

            }

            //get Blob reference

            CloudBlockBlob cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference(filename);
            
            cloudBlockBlob.Properties.ContentType = image.ContentType;

            await cloudBlockBlob.UploadFromStreamAsync(image.OpenReadStream());
        }


    }
}
