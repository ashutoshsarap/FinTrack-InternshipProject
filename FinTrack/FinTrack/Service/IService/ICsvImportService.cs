using FinTrack.Models.DTOs.CsvDtos;

namespace FinTrack.Service.IService
{
    public interface ICsvImportService
    {
        Task<CsvImportResult> ImportCsv(IFormFile csvFile);
    }
}
