﻿using AutoMapper;
using LMS.API.Data;
using LMS.API.Models.Dtos;
using LMS.API.Models.Entities;
using LMS.API.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LMS.API.Controllers
{
    [Route("api/courses")]
    [ApiController]
    public class CourseController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly DatabaseContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        //private readonly ServiceManager _serviceManager;
        public CourseController(IMapper mapper, DatabaseContext context, UserManager<ApplicationUser> userManager)
        {
            _mapper = mapper;
            _context = context;
            _userManager = userManager;
            //_serviceManager = serviceManager;

        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CourseDto>>> GetCourses()
        {
            var courses = await _context.Set<Course>().Include(c => c.Modules).ToListAsync();
            var courseDtos = _mapper.Map<IEnumerable<CourseDto>>(courses);

            return Ok(courseDtos);
        }

        [HttpPost]
        public async Task<ActionResult<CourseDto>> CreateCourse(CourseDto courseDto)
        {
            var course = _mapper.Map<Course>(courseDto);
            _context.Set<Course>().Add(course);
            await _context.SaveChangesAsync();

            return Ok(_mapper.Map<CourseDto>(course));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CourseDto>> GetCourse(string id)
        {
            var user = await _context.Users.Where(u => u.Id == id).Select(u => new {u.CourseId}).FirstOrDefaultAsync();
            if (user == null)
            {
                return NotFound();
            }
            if (user.CourseId == null || !user.CourseId.HasValue)
            {
                return NotFound();
            }
            var course = await _context.Courses.Include(c => c.Modules).FirstOrDefaultAsync(c => c.Id == user.CourseId);
            var courseDto = _mapper.Map<CourseDto>(course);
            return Ok(courseDto);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<CourseDto>> UpdateCourse(string id, CourseForUpdateDto courseDto)
        {
            var course = await _context.Courses.FirstOrDefaultAsync(c => c.Id.ToString() == id);
            if (course == null)
            {
                return NotFound();
            }
            _mapper.Map(courseDto, course);
            await _context.SaveChangesAsync();
            return Ok(_mapper.Map<CourseForUpdateDto>(course));
        }
    }
}
