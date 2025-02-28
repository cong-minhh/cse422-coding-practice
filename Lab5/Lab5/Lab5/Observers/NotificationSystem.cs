using System;
using System.Collections.Generic;
using Lab5.Models;

namespace Lab5.Observers
{
    // Observer Pattern Implementation
    
    // Subject interface
    public interface ILibrarySubject
    {
        void RegisterObserver(ILibraryObserver observer);
        void RemoveObserver(ILibraryObserver observer);
        void NotifyObservers(LibraryEvent libraryEvent);
    }
    
    // Observer interface
    public interface ILibraryObserver
    {
        void Update(LibraryEvent libraryEvent);
    }
    
    // Concrete Subject
    public class LibraryManager : ILibrarySubject
    {
        private List<ILibraryObserver> _observers = new List<ILibraryObserver>();
        private List<Document> _documents = new List<Document>();
        
        public void RegisterObserver(ILibraryObserver observer)
        {
            if (!_observers.Contains(observer))
            {
                _observers.Add(observer);
            }
        }
        
        public void RemoveObserver(ILibraryObserver observer)
        {
            _observers.Remove(observer);
        }
        
        public void NotifyObservers(LibraryEvent libraryEvent)
        {
            foreach (var observer in _observers)
            {
                observer.Update(libraryEvent);
            }
        }
        
        // Library operations that trigger notifications
        public void AddDocument(Document document)
        {
            _documents.Add(document);
            NotifyObservers(new LibraryEvent(EventType.DocumentAdded, document));
        }
        
        public void BorrowDocument(Document document, User user)
        {
            if (document.IsAvailable)
            {
                document.IsAvailable = false;
                NotifyObservers(new LibraryEvent(EventType.DocumentBorrowed, document, user));
            }
        }
        
        public void ReturnDocument(Document document, User user)
        {
            if (!document.IsAvailable)
            {
                document.IsAvailable = true;
                NotifyObservers(new LibraryEvent(EventType.DocumentReturned, document, user));
            }
        }
        
        public List<Document> GetAllDocuments()
        {
            return _documents;
        }
    }
    
    // Concrete Observer
    public class User : ILibraryObserver
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public List<Document> InterestedDocuments { get; set; } = new List<Document>();
        
        public User(int id, string name, string email)
        {
            Id = id;
            Name = name;
            Email = email;
        }
        
        public void AddInterest(Document document)
        {
            if (!InterestedDocuments.Contains(document))
            {
                InterestedDocuments.Add(document);
            }
        }
        
        public void Update(LibraryEvent libraryEvent)
        {
            switch (libraryEvent.EventType)
            {
                case EventType.DocumentAdded:
                    Console.WriteLine($"Notification to {Name}: New document added: {libraryEvent.Document.Title}");
                    break;
                    
                case EventType.DocumentBorrowed:
                    if (InterestedDocuments.Contains(libraryEvent.Document))
                    {
                        Console.WriteLine($"Notification to {Name}: Document you're interested in has been borrowed: {libraryEvent.Document.Title}");
                    }
                    break;
                    
                case EventType.DocumentReturned:
                    if (InterestedDocuments.Contains(libraryEvent.Document))
                    {
                        Console.WriteLine($"Notification to {Name}: Document you're interested in has been returned: {libraryEvent.Document.Title}");
                    }
                    break;
            }
        }
    }
    
    // Event class to hold event information
    public class LibraryEvent
    {
        public EventType EventType { get; private set; }
        public Document Document { get; private set; }
        public User? User { get; private set; }
        
        public LibraryEvent(EventType eventType, Document document, User? user = null)
        {
            EventType = eventType;
            Document = document;
            User = user;
        }
    }
    
    // Event types
    public enum EventType
    {
        DocumentAdded,
        DocumentBorrowed,
        DocumentReturned
    }
}