using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab3
{
    internal class BookClass
    {
        public string ISBN { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }

        public BookClass(string isbn, string title, string author)
        {
            ISBN = isbn;
            Title = title;
            Author = author;
        }

        // Override ToString for better display
        public override string ToString()
        {
            return $"BookClass {{ ISBN = {ISBN}, Title = {Title}, Author = {Author} }}";
        }

        // Override Equals for comparison demonstration
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            BookClass other = (BookClass)obj;
            return ISBN == other.ISBN && 
                   Title == other.Title && 
                   Author == other.Author;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(ISBN, Title, Author);
        }
    }
}
