using System.ComponentModel.DataAnnotations;
//V1
namespace FinTrack.Models.Entity
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

    }
}
