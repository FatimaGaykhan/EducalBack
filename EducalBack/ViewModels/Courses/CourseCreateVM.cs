﻿using System;
using System.ComponentModel.DataAnnotations;

namespace EducalBack.ViewModels.Courses
{
	public class CourseCreateVM
	{
        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string Price { get; set; }

        public int CategoryId { get; set; }

        [Required]
        public List<IFormFile> Images { get; set; }
    }
}

