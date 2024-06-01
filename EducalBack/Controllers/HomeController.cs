using System.Diagnostics;
using EducalBack.Services.Interfaces;
using EducalBack.ViewModels;
using Microsoft.AspNetCore.Mvc;

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

   
}

