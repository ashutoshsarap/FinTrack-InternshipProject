using FinTrack.Models.Entity;
using System.ComponentModel.DataAnnotations;
//V1
namespace FinTrack.Models.DTOs.CategoryDtos
{
    public class CategoryDto
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = null!;
        
    }
}
