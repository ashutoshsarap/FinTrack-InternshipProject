using CsvHelper;
using FinTrack.Models.DTOs;
using FinTrack.Models.DTOs.CsvDtos;
using FinTrack.Repository.IRepository;
using FinTrack.Service.IService;
using System.Globalization;
using System.Threading.Tasks;

namespace FinTrack.Service
{
    public class CsvExportService : ICsvExportService
    {

        private readonly IUnitOfWork _unitOfWork;

        public CsvExportService(IUnitOfWork unitOfWork)
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
            csvExportDtosList = userTransactions.Select(transaction => new CsvExportDto
            {
                Amount = transaction.Amount,
                Date = transaction.Date,
                Type = transaction.Type.ToString(),
                PaymentMode = transaction.PaymentMode.ToString(),
                Description = transaction.Description,
                CategoryName = transaction.Category.Name
            }).ToList();

            

            //Creates a temporary empty container in server RAM to hold the CSV data, which will be written to and then returned as a stream.
            //Alternative could have been using a FileStream to write to a temporary file on disk, but using MemoryStream is more efficient for this use case as it avoids disk I/O overhead.
            var memoryStream = new MemoryStream();

            //Since memorystream only handles raw byte data, we use a StreamWriter to write text data (CSV format) into the memory stream. Streamwriter basically acts like a translator that converts the CSV text into bytes and writes it to the memory stream.
            //when we are passing the memoryStream to StreamWriter, basically we are telling StreamWriter to write the CSV data into that memory stream. 
            var writer = new StreamWriter(memoryStream);

            //CsvWriter is a helper tool which converts C# objects (in this case, the list of CsvExportDto) into CSV format and writes it to the provided StreamWriter. It handles all the formatting and escaping needed for CSV files.
            //We are passing the StreamWriter to CsvWriter, basically we are telling CsvWriter whenever it writes the CSV data, it should write it to that StreamWriter, which in turn writes it to the memory stream. So the flow is: CsvWriter -> StreamWriter -> MemoryStream.
            var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);

            //This is where the actual writing happens. When we call WriteRecords and pass the list of CsvExportDto, CsvWriter takes each object in the list, converts it to a CSV row (using the properties of the object as columns), and writes it to the StreamWriter, which then writes it to the MemoryStream. 
            csv.WriteRecords(csvExportDtosList);

            //Moves all the data from the StreamWriter's internal buffer to the MemoryStream. This is important because StreamWriter may not write data to the underlying stream immediately for performance reasons, so we need to flush it to ensure all data is written before we read from the MemoryStream.
            await writer.FlushAsync();

            //Resets the position of the MemoryStream back to the beginning (position 0) so that when we return it, the caller can read from the start of the stream. If we didn't reset the position, it would be at the end of the stream after writing, and any attempt to read from it would return no data.
            memoryStream.Position = 0;
            
            return memoryStream;
        }
    }
}
