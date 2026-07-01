using KiddooPlaySchool.Domain.Entities;
using KiddooPlaySchool.Infrastructure.Data;

namespace KiddooPlaySchool.Infrastructure.Repositories;

public class StudentRepository : BaseRepository<Student>
{
    public StudentRepository(AppDbContext context) : base(context)
    {
    }
}
