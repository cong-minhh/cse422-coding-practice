using System;
using System.Collections.Generic;

public class ReaderRepository : IReaderRepository
{
    private readonly Dictionary<string, Reader> _readers;

    public ReaderRepository()
    {
        _readers = new Dictionary<string, Reader>();
    }

    public void AddReader(Reader reader)
    {
        if (!_readers.ContainsKey(reader.Id))
        {
            _readers.Add(reader.Id, reader);
        }
    }

    public Reader GetReaderById(string id)
    {
        if (!_readers.ContainsKey(id))
            throw new KeyNotFoundException($"Reader with ID {id} not found");
            
        return _readers[id];
    }

    public IEnumerable<Reader> GetAllReaders()
    {
        return _readers.Values.ToList();
    }
} 