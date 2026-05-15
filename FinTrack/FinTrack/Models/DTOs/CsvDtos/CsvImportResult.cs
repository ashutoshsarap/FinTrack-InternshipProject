namespace FinTrack.Models.DTOs.CsvDtos
{
    //Using this DTO to return the result of the CSV import process, such as the number of records imported, any errors encountered during the import, and a list of the imported transactions.
    //This will allow us to provide feedback to the user about the success or failure of the import process and any issues that need to be addressed.
    public class CsvImportResult
    {
        public int RecordsImported { get; set; }
        public int TotalRecordsAdded { get; set; }
        public int DuplicateRecordsFound { get; set; }
        public int InvalidRecordsFound { get; set; }
        public List<string> Errors { get; set; } = new List<string>();
        
    }
}
