using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
//V1
namespace FinTrack.Models.Entity
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string ApplicationUserId { get; set; }
        [ForeignKey("ApplicationUserId")]
        public ApplicationUser ApplicationUser { get; set; }

    }
}
