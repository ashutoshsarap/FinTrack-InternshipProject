using CsvHelper;
using FinTrack.Repository.IRepository;
using FinTrack.Service.IService;
using NuGet.Packaging.Signing;
using System.Globalization;
using FinTrack.Models.Entity;
using FinTrack.Models.Enums;
using FinTrack.Models.DTOs.CsvDtos;

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
        public async Task<CsvImportResult> ImportCsv(IFormFile csvFile)
        {

            string currentUserId = _currentUserService.UserId;

            CsvImportResult csvImportResult = new CsvImportResult()
            {
                RecordsImported = 0,
                TotalRecordsAdded = 0,
                DuplicateRecordsFound = 0,
                InvalidRecordsFound = 0,
                Errors = new List<string>()
            };

            var stream = csvFile.OpenReadStream();

            var reader = new StreamReader(stream);

            var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

            var records = csv.GetRecords<CsvImportDto>().ToList();

            csvImportResult.RecordsImported = records.Count;

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

                //Checking if the transaction type and payment mode are valid enums, if not add an error message to the csvImportResult and skip that record
                if (!Enum.TryParse(record.Type, true, out TransactionType transactionType))
                {
                    csvImportResult.InvalidRecordsFound++;
                    csvImportResult.Errors.Add($"Invalid transaction type {record.Type} for record with description: {record.Description}. Skipping this record.");
                    continue; // Skip this record and continue with the next one
                }
                if (!Enum.TryParse(record.PaymentMode, true, out PaymentMode paymentMode))
                {
                    csvImportResult.InvalidRecordsFound++;
                    csvImportResult.Errors.Add($"Invalid Payment mode {record.PaymentMode} for record with description: {record.Description}. Skipping this record.");
                    continue; // Skip this record and continue with the next one
                }

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

                //Check for duplicate transactions based on the description, date and amount. If a duplicate transaction is found, throw an exception with the details of the duplicate transaction
                if (await _unitOfWork.Transaction.IsDuplicateTransaction(transaction))
                {
                    csvImportResult.DuplicateRecordsFound++;
                    csvImportResult.Errors.Add($"Duplicate transaction found: Description: {transaction.Description}, Date: {transaction.Date}, Amount: {transaction.Amount}");
                    continue; // Skip the duplicate transaction and continue with the next record
                }
                csvImportResult.TotalRecordsAdded++;
                await _unitOfWork.Transaction.CreateAsync(transaction);
            }
            await _unitOfWork.Save();
            return csvImportResult;
        }
    }
}
