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
        private readonly string _currentUserName;
        private readonly IAuditService _auditService;
        public CsvImportService(IUnitOfWork unitOfWork, ICurrentUserService currentUserService, IAuditService auditService)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
            _currentUserName = currentUserService.UserName;
            _auditService = auditService;
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
            
            //Creates stream of byte data from the uploaded file since uploaded files arrive as raw bytes over the HTTP request
            var stream = csvFile.OpenReadStream();

            //StreamReader is a tool that helps in converting bytes -> text
            var streamReader = new StreamReader(stream);

            //CsvReader is a special parser for CSV files, it understands the structure of CSV files and can convert rows of CSV data into C# objects based on the mapping we define in CsvImportDto
            var csvReader = new CsvReader(streamReader, CultureInfo.InvariantCulture);

            //Converting CSV rows into C# object
            var records = csvReader.GetRecords<CsvImportDto>().ToList();

            csvImportResult.RecordsImported = records.Count;

            foreach (var record in records)
            {

                if (record.Amount <= 0)
                {
                    csvImportResult.InvalidRecordsFound++;
                    csvImportResult.Errors.Add($"Invalid amount {record.Amount} for record with description: {record.Description}. Skipping this record.");
                    continue; // Skip this record and continue with the next one
                }
                if (record.Date > DateTime.Now)
                {
                    csvImportResult.InvalidRecordsFound++;
                    csvImportResult.Errors.Add($"Invalid date {record.Date} for record with description: {record.Description}. Skipping this record.");
                    continue; // Skip this record and continue with the next one
                }

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

            AuditData auditData = new AuditData()
            {
                UserName = _currentUserName,
                Action = "Import",
                EntityActedUpon = "Transaction",
                Timestamp = DateTime.Now
            };
            await _auditService.LogAuditDataAsync(auditData);

            return csvImportResult;
        }
    }
}
