using Application.Features.Schools;
using Domain.Entities;
using Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Schools
{
    public class SchoolService(ApplicationDbContext context) : ISchoolService
    {
        public async Task<int> CreateSchoolAsync(School school)
        {
            await context.Schools.AddAsync(school);
            await context.SaveChangesAsync();
            return school.Id;
        }

        public async Task<int> DeleteSchoolAsync(School school)
        {
            context.Schools.Remove(school);
            await context.SaveChangesAsync();
            return school.Id;
        }

        public async Task<School> GetSchoolByIdAsync(int schoolId)
        {
            var schoolInDb = await context
                .Schools
                .Where(s => s.Id == schoolId)
                .FirstOrDefaultAsync();
            return schoolInDb;
        }

        public async Task<School> GetSchoolByNameAsync(string name)
        {
            var schoolInDb = await context
                .Schools
                .Where(s => s.Name == name)
                .FirstOrDefaultAsync();
            return schoolInDb;
        }

        public async Task<List<School>> GetSchoolsAsync()
        {
            return await context.Schools.ToListAsync();
        }

        public async Task<int> UpdateSchoolAsync(School school)
        {
            context.Schools.Update(school);
            await context.SaveChangesAsync();
            return school.Id;
        }
    }
}
