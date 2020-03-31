using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace Recruitment.Services
{
    public static class BlobStorageService
    {
        public static async Task<(bool, string)> UploadToBlob(string filename, string storageConnectionString, byte[] imageBuffer = null,
            Stream stream = null)
        {
            if (string.IsNullOrEmpty(storageConnectionString) || string.IsNullOrEmpty(filename))
            {
                throw new Exception("Wrong file name or connection string");
            }
            // Check whether the connection string can be parsed.
            if (CloudStorageAccount.TryParse(storageConnectionString, out var storageAccount))
                try
                {
                    // Create the CloudBlobClient that represents the Blob storage endpoint for the storage account.
                    var cloudBlobClient = storageAccount.CreateCloudBlobClient();

                    // Create a container called 'uploadblob' and append a GUID value to it to make the name unique. 
                    var cloudBlobContainer = cloudBlobClient.GetContainerReference("uploadblob" + Guid.NewGuid());
                    await cloudBlobContainer.CreateAsync();

                    // Set the permissions so the blobs are public. 
                    var permissions = new BlobContainerPermissions
                    {
                        PublicAccess = BlobContainerPublicAccessType.Blob
                    };
                    await cloudBlobContainer.SetPermissionsAsync(permissions);

                    // Get a reference to the blob address, then upload the file to the blob.
                    var cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference(filename);

                    if (imageBuffer != null)
                        // OPTION A: use imageBuffer (converted from memory stream)
                        await cloudBlockBlob.UploadFromByteArrayAsync(imageBuffer, 0, imageBuffer.Length);
                    else if (stream != null)
                        // OPTION B: pass in memory stream directly
                        await cloudBlockBlob.UploadFromStreamAsync(stream);
                    else
                        return (false, null);

                    return (true, cloudBlockBlob.SnapshotQualifiedStorageUri.PrimaryUri.ToString());
                }
                catch (StorageException)
                {
                    return (false, null);
                }

            return (false, null);
        }
    }
}