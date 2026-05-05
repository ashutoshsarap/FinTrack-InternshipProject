using FinTrack.Data;
using FinTrack.Models.DTOs;
using FinTrack.Repository;
using Microsoft.AspNetCore.Mvc;

namespace FinTrack.Controllers
{
    public class TransactionController : Controller
    {

        private readonly ApplicationDbContext _context;

        public TransactionController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Get(int id)
        {
            var response = _context.Transactions
                                   .Where(t => t.Id == id)
                                   .Select(t => new TransactionResponseDto
                                   {
                                        Id = t.Id,
                                        Amount = t.Amount,
                                        Date = t.Date,
                                        Type = t.Type,
                                        PaymentMode = t.PaymentMode,
                                        Description = t.Description,
                                        CategoryId = t.CategoryId,
                                        CategoryName = t.Category.Name
                                   });
            return Ok(response);
        }
    }
}
