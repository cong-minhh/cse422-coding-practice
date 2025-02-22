# Library Management System: A SOLID Approach

## Introduction

This report outlines the design and implementation of a Library Management System developed in C# following SOLID principles. The system is designed to handle core library functions such as adding books, searching for books, lending books, returning books, and generating reports. The design ensures ease of maintenance and future extensibility.

## System Design

### UML Class Diagram

The UML class diagram illustrates the relationships between the various components of the system. The system is divided into three main components: Book Management, Reader Management, and Report Generation Service. Each component is designed to adhere to SOLID principles, ensuring a clean and maintainable architecture.

### Class and Interface Definitions

1. **Models**:
   - **IBook**: Interface defining the properties and methods for a book.
   - **Book**: Concrete implementation of IBook, representing a physical book in the library.
   - **Reader**: Represents a library reader with a unique ID and a list of borrowed books.

2. **Repositories**:
   - **IBookRepository**: Interface for book storage operations.
   - **IReaderRepository**: Interface for reader storage operations.
   - **InMemoryBookRepository**: In-memory implementation of IBookRepository.
   - **InMemoryReaderRepository**: In-memory implementation of IReaderRepository.

3. **Services**:
   - **ILibraryService**: Interface defining library operations such as adding books, lending, and returning books.
   - **LibraryService**: Implementation of ILibraryService, handling core library operations.
   - **IReportService**: Interface for generating reports.
   - **ReportService**: Implementation of IReportService, generating reports on borrowed books.

## Implementation

### Core Functionalities

1. **Add New Books**: Administrators can add books with details such as Title, Author, Category, and Quantity. Each book is assigned a unique ID.

2. **Search for Books**: Users can search for books by Title or Category. The search functionality is case-insensitive and returns a list of matching books.

3. **Lend Books**: Readers can borrow books if available. Each reader can borrow up to 3 books. The system updates the book's available quantity upon lending.

4. **Return Books**: Readers return books, and the system updates the stock quantity accordingly.

5. **Generate Reports**: The system generates a report of readers and the books they have borrowed, providing a clear overview of current library activity.

### Sample Data

The system includes sample data for testing purposes, with a variety of books and readers to demonstrate all functionalities.

### Application of SOLID Principles

1. **Single Responsibility Principle**: Each class has a single responsibility. For example, the `Book` class manages book data, while the `LibraryService` handles library operations.

2. **Open/Closed Principle**: The system is open for extension but closed for modification. New book types can be added by implementing the `IBook` interface without altering existing code.

3. **Liskov Substitution Principle**: Any implementation of `IBook` can be used wherever `IBook` is expected, ensuring interchangeable components.

4. **Interface Segregation Principle**: Interfaces are specific to client needs, preventing unnecessary dependencies.

5. **Dependency Inversion Principle**: High-level modules depend on abstractions rather than concrete implementations, promoting flexibility and scalability.

## Extensibility and Future Enhancements

The system is designed to be extensible, allowing for future enhancements such as:

1. **Managing eBooks**: Implement a new `EBook` class that adheres to the `IBook` interface, allowing the system to manage digital books.

2. **Book Reservations**: Introduce a `IReservationService` interface and corresponding implementation to handle book reservations, enabling users to reserve books in advance.

3. **Integration with External Systems**: The system can be extended to integrate with external databases or library management systems, enhancing its functionality and reach.

## Conclusion

The Library Management System is a robust, maintainable, and extensible solution that adheres to SOLID principles. It effectively manages core library functions while providing a foundation for future enhancements. The design ensures that the system can evolve with changing requirements, making it a practical and optimal solution for library management.

This report demonstrates the successful application of SOLID principles in designing a system that is both functional and adaptable, meeting the needs of a modern library environment.