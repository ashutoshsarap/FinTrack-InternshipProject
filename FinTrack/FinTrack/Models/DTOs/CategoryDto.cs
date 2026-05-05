using System.ComponentModel.DataAnnotations;
//V1
namespace FinTrack.Models.DTOs
{
    public class CategoryDto
    {
        [Required]
        public string Name { get; set; } = null!;
    }
}
