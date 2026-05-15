namespace FinTrack.Service.IService
{
    public interface ICsvExportService
    {
        public Task<MemoryStream> GenerateCsv();
    }
}
