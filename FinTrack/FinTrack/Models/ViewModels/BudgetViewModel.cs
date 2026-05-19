using FinTrack.Models.DTOs.BudgetDtos;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace FinTrack.Models.ViewModels
{
    public class BudgetViewModel
    {
        public int Id { get; set; }
        public decimal MonthlyLimitAmount { get; set; }
        [ValidateNever]
        public string CategoryName { get; set; }
        public int CategoryId { get; set; }
    }
}
