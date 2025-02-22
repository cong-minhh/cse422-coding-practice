public interface IReaderRepository
{
    void AddReader(Reader reader);
    Reader GetReaderById(string id);
    IEnumerable<Reader> GetAllReaders();
} 