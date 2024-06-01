using System;
using EducalBack.Data;
using EducalBack.Models;
using EducalBack.Services.Interfaces;
using EducalBack.ViewModels.Courses;
using Microsoft.EntityFrameworkCore;

namespace EducalBack.Services
{
	public class CourseService:ICourseService
	{
        private readonly AppDbContext _context;

		public CourseService(AppDbContext context)
		{
            _context = context;
		}

        public async Task CreateAsync(Course course)
        {
            await _context.Courses.AddAsync(course);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Course course)
        {
             _context.Courses.Remove(course);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Course>> GetAllAsync()
        {
            return await _context.Courses.Include(m => m.Category)
                                         .Include(m => m.CourseImages).ToListAsync();
        }

        public async Task<IEnumerable<CourseVM>> GetAllForAdminAsync()
        {
            IEnumerable<Course> courses= await _context.Courses.Include(m => m.Category)
                                         .Include(m => m.CourseImages).ToListAsync();

            return courses.Select(m => new CourseVM
            {
                Id=m.Id,
                Name=m.Name,
                CategoryName=m.Category.Name,
                Description=m.Description,
                MainImage=m.CourseImages.FirstOrDefault(m => m.IsMain)?.Name,
                Price=m.Price
            });
        }

        public async Task<Course> GetByIdAsync(int id)
        {
            return await _context.Courses.FindAsync(id);
        }

        public async Task<Course> GetByIdWithAllDatasAsync(int id)
        {
            return await _context.Courses.Where(m => m.Id == id)
                                          .Include(m => m.Category)
                                          .Include(m => m.CourseImages)
                                          .FirstOrDefaultAsync();
        }
    }
}

