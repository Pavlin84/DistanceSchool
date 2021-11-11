
using System.Threading.Tasks;

namespace DistanceSchool.Services.Data
{
    public interface ICustomEmailSenderService
    {
        Task DiapprovedUserSend(int candidacyId);

        Task ApprovedUserSend(int candidacyId);
    }
}
