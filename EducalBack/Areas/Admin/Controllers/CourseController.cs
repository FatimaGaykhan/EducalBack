using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EducalBack.Helpers.Extensions;
using EducalBack.Models;
using EducalBack.Services.Interfaces;
using EducalBack.ViewModels.Courses;
using Microsoft.AspNetCore.Mvc;


namespace EducalBack.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CourseController : Controller
    {
        private readonly ICourseService _courseService;
        private readonly ICategoryService _categoryService;
        private readonly IWebHostEnvironment _env;

        public CourseController(ICourseService courseService,
                                ICategoryService categoryService,
                                IWebHostEnvironment env)
        {
            _courseService = courseService;
            _categoryService = categoryService;
            _env = env;
        }

        public async Task< IActionResult> Index()
        {
            IEnumerable<CourseVM> model = await _courseService.GetAllForAdminAsync();
            return View(model);
        }

        public async Task<IActionResult> Detail(int? id)
        {
            if (id is null) return BadRequest();

            var existProduct = await _courseService.GetByIdWithAllDatasAsync((int)id);

            if (existProduct is null) return NotFound();

            List<CourseImageVM> images = new();

            foreach (var item in existProduct.CourseImages)
            {
                images.Add(new CourseImageVM
                {
                    Image = item.Name,
                    IsMain = item.IsMain
                });
            }


            CourseDetailVM response = new()
            {
                Name = existProduct.Name,
                Description = existProduct.Description,
                Price = existProduct.Price,
                Category = existProduct.Category.Name,
                Images = images
            };

            return View(response);
        }


        public async Task< IActionResult> Create()
        {
            ViewBag.categories = await _categoryService.GetAllSelectedAsync();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CourseCreateVM request)
        {
            ViewBag.categories = await _categoryService.GetAllSelectedAsync();

            if (!ModelState.IsValid)
            {
                return View();
            }

            foreach (var item in request.Images)
            {
                if (!item.CheckFileSize(500))
                {
                    ModelState.AddModelError("Images", "Image size must be 500KB");
                    return View();
                }

                if (!item.CheckFileType("image/"))
                {
                    ModelState.AddModelError("Images", "File type must be only image ");
                    return View();
                }
            }

            List<CourseImage> images = new();

            foreach (var item in request.Images)
            {
                string fileName = $"{Guid.NewGuid()}-{item.FileName}";

                string path = _env.GenerateFilePath("assets/img", fileName);

                await item.SaveFileToLocalAsync(path);

                images.Add(new CourseImage { Name = fileName });
            }

            images.FirstOrDefault().IsMain = true;

            Course course = new()
            {
                Name = request.Name,
                Description = request.Description,
                CategoryId = request.CategoryId,
                Price = decimal.Parse(request.Price.Replace(".", ",")),
                CourseImages = images

            };

            await _courseService.CreateAsync(course);

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null) return BadRequest();

            var existCourse = await _courseService.GetByIdWithAllDatasAsync((int)id);

            if (existCourse is null) return NotFound();

            foreach (var item in existCourse.CourseImages)
            {
                string path = _env.GenerateFilePath("assets/img", item.Name);
                path.DeleteFileFromLocal();
            }

            await _courseService.DeleteAsync(existCourse);

            return RedirectToAction(nameof(Index));
        }
    }
}

