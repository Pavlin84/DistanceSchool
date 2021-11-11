namespace DistanceSchool.Web.ViewModels.CustomEmailSender

{
    using AutoMapper;
    using DistanceSchool.Data.Models;
    using DistanceSchool.Services.Mapping;

    public class EmailSenderDataModel : IMapFrom<Candidacy>, IHaveCustomMappings
    {
        public string ApplicationUserEmail { get; set; }

        public string SchoolName { get; set; }

        public string Name { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Candidacy, EmailSenderDataModel>()
                .ForMember(x => x.Name, opt => opt.MapFrom(x => x.FirstName != null
                ? x.FirstName + " " + x.LastName
                : x.ApplicationUser.Teacher.FirstName + " " + x.ApplicationUser.Teacher.LastName));
        }
    }
}
