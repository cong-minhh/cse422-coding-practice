using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Lab6.Models;

// For Exercise 2
namespace Lab6.Repositories
{
    public abstract class BaseRepository<T>
    {
        protected readonly SqlConnection _connection;
        public BaseRepository(SqlConnection connection) => _connection = connection;

        public List<T> GetAll()
        {
            var command = new SqlCommand($"SELECT * FROM {typeof(T).Name}s", _connection);
            var reader = command.ExecuteReader();
            return ReadData(reader);
        }

        private List<T> ReadData(SqlDataReader reader)
        {
            var list = new List<T>();
            while (reader.Read())
            {
                var obj = Activator.CreateInstance<T>();
                typeof(T).GetProperty("Id")?.SetValue(obj, reader.GetInt32(0));
                typeof(T).GetProperty("Name")?.SetValue(obj, reader.GetString(1));
                list.Add(obj);
            }
            return list;
        }
    }

    public class StudentRepository : BaseRepository<Student>
    {
        public StudentRepository(SqlConnection connection) : base(connection) { }
    }

    public class TeacherRepository : BaseRepository<Teacher>
    {
        public TeacherRepository(SqlConnection connection) : base(connection) { }
    }
}