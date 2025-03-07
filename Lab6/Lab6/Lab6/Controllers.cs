using System;
using Microsoft.AspNetCore.Mvc;
using Lab6.Models;

// For Exercise 3
namespace Lab6.Controllers
{
    public abstract class BaseController<T> : ControllerBase
        where T : class, new()
    {
        [HttpPost]
        public IActionResult Create(T entity)
        {
            if (entity == null || string.IsNullOrEmpty((string)typeof(T).GetProperty("Name")?.GetValue(entity)))
                return BadRequest("Invalid data");
            return Ok($"{typeof(T).Name} {typeof(T).GetProperty("Name")?.GetValue(entity)} created successfully.");
        }
    }

    public class StudentController : BaseController<Student> { }
    public class TeacherController : BaseController<Teacher> { }
}