using CsvHelper;
using FinTrack.Models.DTOs;
using FinTrack.Models.DTOs.CsvDtos;
using FinTrack.Repository.IRepository;
using FinTrack.Service.IService;
using System.Globalization;
using System.Threading.Tasks;

namespace FinTrack.Service
{
    public class GenerateCsvService : IGenerateCsvService
    {

        private readonly IUnitOfWork _unitOfWork;

        public GenerateCsvService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<MemoryStream> GenerateCsv()
        {

            TransactionFilterDto transactionFilterDto = new TransactionFilterDto
            {
                StartDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1),
                EndDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.DaysInMonth(DateTime.Today.Year, DateTime.Today.Month))
            };

            var userTransactions = await _unitOfWork.Transaction.FindAllTransactionByFilterAsync(transactionFilterDto, "Category");
            List<CsvExportDto> csvExportDtosList = new List<CsvExportDto>();

            foreach (var transaction in userTransactions)
            {
                var csvExportDto = new CsvExportDto
                {
                    Amount = transaction.Amount,
                    Date = transaction.Date,
                    Type = transaction.Type.ToString(),
                    PaymentMode = transaction.PaymentMode.ToString(),
                    Description = transaction.Description,
                    CategoryName = transaction.Category.Name
                };
                
                csvExportDtosList.Add(csvExportDto);
            }

            var memoryStream = new MemoryStream();
            var writer = new StreamWriter(memoryStream);
            var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);
            csv.WriteRecords(csvExportDtosList);

            writer.Flush();
            memoryStream.Position = 0;
            
            return memoryStream;
        }
    }
}
