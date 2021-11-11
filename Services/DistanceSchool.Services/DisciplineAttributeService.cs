namespace DistanceSchool.Services
{
    using System.Linq;

    using DistanceSchool.Data.Common.Repositories;
    using DistanceSchool.Data.Models;

    public class DisciplineAttributeService : IDisciplineAttributeService
    {
        private readonly IDeletableEntityRepository<Discipline> disciplineRepository;

        public DisciplineAttributeService(IDeletableEntityRepository<Discipline> disciplineRepository)
        {
            this.disciplineRepository = disciplineRepository;
        }

        public bool IsDisciplineExsist(string discipline)
        {
            return this.disciplineRepository.All().Any(x => x.Name.ToLower() == discipline.ToLower());
        }
    }
}
