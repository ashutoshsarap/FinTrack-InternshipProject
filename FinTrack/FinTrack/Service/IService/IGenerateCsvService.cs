namespace FinTrack.Service.IService
{
    public interface IGenerateCsvService
    {
        public Task<MemoryStream> GenerateCsv();
    }
}
