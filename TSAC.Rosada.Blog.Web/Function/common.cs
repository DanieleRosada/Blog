using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;

namespace TSAC.Rosada.Blog.Web.Function
{
    public class Common
    {
        private string _azureUser;
        private string _azureKey;

        public Common(IConfiguration configuration)
        {
            _azureUser = configuration.GetConnectionString("AzureUser");
            _azureKey= configuration.GetConnectionString("AzureKey");
        }

        public async Task InsertPhoto(IFormFile image, string filename)
        {
            //directory
            //var file = Path.Combine(
            //    Directory.GetCurrentDirectory(),
            //    "wwwroot", "files", filename);
            //using (var stream = new FileStream(file, FileMode.Create))
            //{
            //    await image.CopyToAsync(stream);
            //}


            //azure
            var storageCredentials = new StorageCredentials(_azureUser, _azureKey);
            var cloudStorageAccount = new CloudStorageAccount(storageCredentials, true);
            var cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();

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
