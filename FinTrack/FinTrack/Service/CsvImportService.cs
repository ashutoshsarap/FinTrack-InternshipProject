using CsvHelper;
using FinTrack.Repository.IRepository;
using FinTrack.Service.IService;
using NuGet.Packaging.Signing;
using System.Globalization;
using FinTrack.Models.Entity;
using FinTrack.Models.Enums;

namespace FinTrack.Service
{
    public class CsvImportService : ICsvImportService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;

        public CsvImportService(IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
        }
        public async Task ImportCsv(IFormFile csvFile)
        {

            string currentUserId = _currentUserService.UserId;

            var stream = csvFile.OpenReadStream();

            var reader = new StreamReader(stream);

            var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

            var records = csv.GetRecords<Models.DTOs.CsvDtos.CsvImportDto>().ToList();

            foreach (var record in records)
            {
                var category = _unitOfWork.Category.FindCategoryByFilter(c => c.Name == record.CategoryName);


                //Parse the transaction type and payment mode from the CSV from string to enum
                TransactionType transactionType = Enum.Parse<TransactionType>(record.Type, true);
                PaymentMode paymentMode = Enum.Parse<PaymentMode>(record.PaymentMode, true);

                var transaction = new Transaction()
                {
                    Amount = record.Amount,
                    Date = record.Date,
                    Type = transactionType,
                    PaymentMode = paymentMode,
                    Description = record.Description,
                    CategoryId = category.Id,
                    ApplicationUserId = currentUserId,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    IsDeleted = false,
                    DeletedAt = null
                };
                try
                {
                    await _unitOfWork.Transaction.CreateAsync(transaction);
                }
                catch (Exception ex)
                {
                    // Handle exceptions (e.g., log the error, skip the record, etc.)
                    Console.WriteLine($"Error importing record: {ex.Message}");
                }
            }
            await _unitOfWork.Save();
        }

    }
}
