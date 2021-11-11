namespace DistanceSchool.Services.Data
{
    using System;
    using System.IO;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Http;

    public class FileHandlerService : IFileHandlerService
    {
        public async Task SaveFileAsync(IFormFile sourseFile, string directoyPath, string fileName)
        {
            Directory.CreateDirectory(directoyPath);

            using Stream imageStream = new FileStream($"{directoyPath}/{fileName}", FileMode.Create);
            await sourseFile.CopyToAsync(imageStream);
        }
    }
}
