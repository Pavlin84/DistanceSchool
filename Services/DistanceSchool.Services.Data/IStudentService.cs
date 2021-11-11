namespace DistanceSchool.Services.Data
{
    using System.Threading.Tasks;

    public interface IStudentService

    {
        bool CheckUserIsStudent(string id);

        Task<string> CreateNewStudentAsync(int id);
    }
}
