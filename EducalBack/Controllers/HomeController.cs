using System.Diagnostics;
using EducalBack.Models;
using EducalBack.Services.Interfaces;
using EducalBack.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace EducalBack.Controllers;

public class HomeController : Controller
{
    private readonly ICategoryService _categoryService;
    private readonly ICourseService _courseService;

    public HomeController(ICategoryService categoryService,
                         ICourseService courseService)
    {
        _categoryService = categoryService;
        _courseService = courseService;
    }

    public async Task<IActionResult> Index()
    {
        HomeVM model = new()
        {
            Categories = await _categoryService.GetAllAsync(),
            Courses = await _courseService.GetAllAsync()
        };
    
        return View(model);
    }

    //public async Task<IActionResult> GetByIdForCategoryTabMenu(int? id)
    //{
    //    //IEnumerable<Course> courses = await _courseService.GetAllAsync();

    //    if (id is null) return BadRequest();

    //    List<Course> courses = await _courseService.GetAllCoursesById((int)id);

    //    if (courses.Count == 0) return NotFound();

    //    return Ok(courses);
    //}

   
}

