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

                //Adding a check to see if the category exists in the database, if not create a new category and then associate the transaction with the category
                if (category == null)
                {
                    category = new Category()
                    {
                        Name = record.CategoryName,
                        ApplicationUserId = currentUserId,
                        IsSystemDefined = false
                    };
                    await _unitOfWork.Category.CreateAsync(category);
                    await _unitOfWork.Save();
                }

                //Parse the transaction type and payment mode from the CSV from string to enum and if the parsing fails, throw an exception
                TransactionType transactionType = Enum.TryParse<TransactionType>(record.Type, ignoreCase: true, out var parsedType) ? parsedType : throw new Exception($"Invalid transaction type: {record.Type}");
                PaymentMode paymentMode = Enum.TryParse<PaymentMode>(record.PaymentMode, ignoreCase: true, out var parsedPaymentMode) ? parsedPaymentMode : throw new Exception($"Invalid payment mode: {record.PaymentMode}");

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

                if (transaction != null)
                {
                    var exist = _unitOfWork.Transaction.FindTransactionByFilterAsync(t => t.Description == transaction.Description &&
                                                                                     t.Amount == transaction.Amount &&
                                                                                     t.Date == transaction.Date &&
                                                                                     t.PaymentMode == transaction.PaymentMode &&
                                                                                     t.Type == transaction.Type &&
                                                                                     t.CategoryId == transaction.CategoryId, null);

                    if (exist != null)
                    {
                        throw new Exception($"Duplicate transaction found: {transaction.Description} on {transaction.Date}");
                    }
                }

                    await _unitOfWork.Transaction.CreateAsync(transaction);
            }
            await _unitOfWork.Save();
        }

    }
}
