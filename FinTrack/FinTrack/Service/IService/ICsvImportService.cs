namespace FinTrack.Service.IService
{
    public interface ICsvImportService
    {
        Task ImportCsv(IFormFile csvFile);
    }
}
