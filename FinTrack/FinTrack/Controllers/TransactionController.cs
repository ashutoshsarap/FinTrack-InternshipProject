using FinTrack.Data;
using FinTrack.Models.DTOs;
using FinTrack.Repository;
using FinTrack.Service;
using FinTrack.Service.IService;
using Microsoft.AspNetCore.Mvc;

namespace FinTrack.Controllers
{
    public class TransactionController : Controller
    {

        private readonly ApplicationDbContext _context;
        private readonly ITransactionService _transactionService;
        public TransactionController(ApplicationDbContext context, ITransactionService transactionService)
        {
            _context = context;
            _transactionService = transactionService;   
        }
        public IActionResult Index()
        {
            var transactions = _transactionService.GetAllTransactionsAsync().Result;
            return Ok(transactions);
        }



        


        

        
    }
}
