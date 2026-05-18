using System.ComponentModel.DataAnnotations;

namespace FinTrack.Models.DTOs.BudgetDtos
{
    public class BudgetCreateDto
    {
        [Required]
        public decimal MonthlyLimitAmount { get; set; }
        [Required]
        public int CategoryId { get; set; }
    }
}
