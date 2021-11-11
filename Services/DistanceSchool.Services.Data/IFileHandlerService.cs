namespace DistanceSchool.Services.Data
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Http;

    public interface IFileHandlerService
    {
        Task SaveFileAsync(IFormFile sourseFile, string directoyPath, string fileName);
    }
}
