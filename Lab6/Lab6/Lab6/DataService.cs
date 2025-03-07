using System;
using System.Collections.Generic;

// For Exercise 5
namespace Lab6.Services
{
    public interface IDataService
    {
        string GetData(int id);
    }

    public class ProductService : IDataService
    {
        public string GetData(int id) => $"Product {id}";
    }

    public class CacheDecorator : IDataService
    {
        private readonly IDataService _service;
        private readonly Dictionary<int, string> _cache = new();

        public CacheDecorator(IDataService service) => _service = service;

        public string GetData(int id)
        {
            if (_cache.ContainsKey(id)) return _cache[id];
            var data = _service.GetData(id);
            _cache[id] = data;
            return data;
        }
    }
}