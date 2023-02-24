using BlazorRestaurantApp.Data;
using Microsoft.AspNetCore.Components.Forms;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;

namespace BlazorRestaurantApp.Services
{
    public class FileSystemService
    {
        private readonly ILogger<FileSystemService> _logger;
        private readonly GridFSBucket _gridFS;
        public FileSystemService(ILogger<FileSystemService> logger)
        {
            _logger = logger;
            var client = new MongoClient("mongodb://localhost");
            var database = client.GetDatabase("RestaurantApp");
            _gridFS = new GridFSBucket(database);
        }

        #region Download
        public byte[] DownloadItemFromDbByIdAsBytes(ObjectId id)
        {
            return _gridFS.DownloadAsBytesAsync(id).Result;
        }
        #endregion

        #region Upload
        public async Task<ObjectId> UploadItemToDb(Stream stream, string fileName)
        {
            return await _gridFS.UploadFromStreamAsync(fileName, stream);
        }
        #endregion


        #region Update
        public async Task<ObjectId> UpdateItemInDb(Stream stream, MenuItem menuItem, string newFileName)
        {
            if(menuItem.ImageId != ObjectId.Empty) 
            {
                await _gridFS.DeleteAsync(menuItem.ImageId);
            }
            return await _gridFS.UploadFromStreamAsync(newFileName, stream);
        }
        #endregion

        #region Download

        #endregion
    }
}
