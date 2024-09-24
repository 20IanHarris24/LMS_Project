﻿using System.ComponentModel.DataAnnotations;

namespace LMS.API.Models.Dtos
{
    public record CourseDto
    {
     
        [Required] public string? Name { get; init; }
        [Required] public string? Description { get; init; }
        [Required] public DateTime Start {  get; init; }
        public required IEnumerable<ModuleManipulationDto> Modules { get; init; }
    }
}
