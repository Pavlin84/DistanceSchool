using DistanceSchool.Data.Common.Repositories;
using DistanceSchool.Data.Models;
using DistanceSchool.Services.Messaging;
using DistanceSchool.Web.ViewModels.CustomEmailSender;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DistanceSchool.Services.Mapping;

namespace DistanceSchool.Services.Data
{
    public class CustomEmailSenderService : ICustomEmailSenderService
    {
        private readonly IEmailSender emailSender;
        private readonly IDeletableEntityRepository<Candidacy> candidacyRepository;

        public CustomEmailSenderService(IEmailSender emailSender, IDeletableEntityRepository<Candidacy> candidacyRepository )
        {
            this.emailSender = emailSender;
            this.candidacyRepository = candidacyRepository;
        }

        public async Task ApprovedUserSend(int candidacyId)
        {
            var userData = this.GetUserData(candidacyId);

            var htmlText = new StringBuilder();

            htmlText.AppendLine("<h3>Одобрена кандидатура</h3>");
            htmlText.AppendLine($"<p>Уважаеми {userData.Name}  пишем за да ви уведомим че вече сте част от нашият екип.</p>");
            htmlText.AppendLine("<a href = \"https://localhost:44319\">Моя влезте в профилът си.</a>");

            await this.emailSender
                .SendEmailAsync("pvs.k.brod@gmail.com", $"Администрацията на {userData.SchoolName}", userData.ApplicationUserEmail, "Кандидатура в Дистанционно училище", htmlText.ToString());
        }

        public async Task DiapprovedUserSend(int candidacyId)
        {
            var userData = this.GetUserData(candidacyId);

            var htmlText = new StringBuilder();

            htmlText.AppendLine("<h3>Отхвърлена кандидатура</h3>");
            htmlText.AppendLine($"<p>Уважаеми {userData.Name}  за съжаление молбатави беше отхвърлен.</p>");
            htmlText.AppendLine("<a href = \"https://localhost:44319\">Моля разгледайте останалите ни предложения</a>");

            await this.emailSender
                .SendEmailAsync("pvs.k.brod@gmail.com", $"Администрацията на {userData.SchoolName}", userData.ApplicationUserEmail, "Кандидатура в Дистанционно училище", htmlText.ToString());
        }

        private EmailSenderDataModel GetUserData(int candidacyId)
        {
            var dataModel = this.candidacyRepository.AllWithDeleted().Where(x => x.Id == candidacyId)
                .To<EmailSenderDataModel>()
                .FirstOrDefault();

            return dataModel;
        }

    }
}
