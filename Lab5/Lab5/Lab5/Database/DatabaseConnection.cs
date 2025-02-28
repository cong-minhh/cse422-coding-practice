using System;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;

namespace Lab5.Database
{
    // Singleton Pattern implementation for database connection
    public sealed class DatabaseConnection
    {
        // Private static instance variable
        private static DatabaseConnection? _instance;
        
        // Lock object for thread safety
        private static readonly object _lock = new object();
        
        // Connection string
        private string _connectionString;
        
        // SQL Server connection object
        private SqlConnection? _connection;
        
        // Flag to indicate if we're using mock functionality
        private bool _useMockDatabase = false;
        
        // Private constructor to prevent instantiation from outside
        private DatabaseConnection()
        {
            // Initialize connection strings
            string serverConnectionString = "Server=localhost;Integrated Security=True;TrustServerCertificate=True";
            // Set the connection string with default value first to avoid null reference
            _connectionString = "Server=localhost;Database=LibraryDB;Integrated Security=True;TrustServerCertificate=True";
            
            Console.WriteLine("Database connection initialized.");
            
            try
            {
                // First check if we can connect to the SQL Server instance
                using (SqlConnection serverConnection = new SqlConnection(serverConnectionString))
                {
                    serverConnection.Open();
                    Console.WriteLine("Connected to SQL Server successfully.");
                    
                    // Initialize the database (create if it doesn't exist)
                    InitializeDatabase();
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"Warning: Could not connect to SQL Server: {ex.Message}");
                Console.WriteLine("Falling back to mock database functionality.");
                _useMockDatabase = true;
            }
        }
        
        // Public method to get the instance
        public static DatabaseConnection Instance
        {
            get
            {
                // Double-check locking pattern for thread safety
                if (_instance == null)
                {
                    lock (_lock)
                    {
                        if (_instance == null)
                        {
                            _instance = new DatabaseConnection();
                        }
                    }
                }
                return _instance;
            }
        }
        
        // Initialize database and create tables if they don't exist
        private void InitializeDatabase()
        {
            try
            {
                // First, try to connect to SQL Server without specifying a database
                string serverConnectionString = "Server=localhost;Integrated Security=True;TrustServerCertificate=True";
                using (SqlConnection serverConnection = new SqlConnection(serverConnectionString))
                {
                    serverConnection.Open();
                    
                    // Check if database exists, create if it doesn't
                    string checkDbQuery = "SELECT name FROM sys.databases WHERE name = 'LibraryDB'";
                    using (SqlCommand cmd = new SqlCommand(checkDbQuery, serverConnection))
                    {
                        var result = cmd.ExecuteScalar();
                        if (result == null)
                        {
                            // Create database
                            string createDbQuery = "CREATE DATABASE LibraryDB";
                            using (SqlCommand createCmd = new SqlCommand(createDbQuery, serverConnection))
                            {
                                createCmd.ExecuteNonQuery();
                                Console.WriteLine("LibraryDB database created successfully.");
                            }
                        }
                    }
                }
                
                // Now connect to the LibraryDB and create tables if needed
                using (SqlConnection dbConnection = new SqlConnection(_connectionString))
                {
                    dbConnection.Open();
                    
                    // Create Documents table
                    string createDocumentsTable = @"
                    IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Documents')
                    BEGIN
                        CREATE TABLE Documents (
                            DocumentId INT IDENTITY(1,1) PRIMARY KEY,
                            Title NVARCHAR(255) NOT NULL,
                            Author NVARCHAR(255) NOT NULL,
                            PublicationDate DATETIME NOT NULL,
                            DocumentType NVARCHAR(50) NOT NULL,
                            ISBN NVARCHAR(20) NULL,
                            Pages INT NULL,
                            Volume NVARCHAR(50) NULL,
                            Publisher NVARCHAR(255) NULL,
                            Edition NVARCHAR(50) NULL
                        )
                    END";
                    
                    using (SqlCommand cmd = new SqlCommand(createDocumentsTable, dbConnection))
                    {
                        cmd.ExecuteNonQuery();
                    }
                    
                    // Create Users table
                    string createUsersTable = @"
                    IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Users')
                    BEGIN
                        CREATE TABLE Users (
                            UserId INT IDENTITY(1,1) PRIMARY KEY,
                            Name NVARCHAR(255) NOT NULL,
                            Email NVARCHAR(255) NOT NULL UNIQUE
                        )
                    END";
                    
                    using (SqlCommand cmd = new SqlCommand(createUsersTable, dbConnection))
                    {
                        cmd.ExecuteNonQuery();
                    }
                    
                    // Create Loans table
                    string createLoansTable = @"
                    IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Loans')
                    BEGIN
                        CREATE TABLE Loans (
                            LoanId INT IDENTITY(1,1) PRIMARY KEY,
                            DocumentId INT NOT NULL,
                            UserId INT NOT NULL,
                            LoanDate DATETIME NOT NULL,
                            ReturnDate DATETIME NULL,
                            FOREIGN KEY (DocumentId) REFERENCES Documents(DocumentId),
                            FOREIGN KEY (UserId) REFERENCES Users(UserId)
                        )
                    END";
                    
                    using (SqlCommand cmd = new SqlCommand(createLoansTable, dbConnection))
                    {
                        cmd.ExecuteNonQuery();
                    }
                    
                    Console.WriteLine("Database tables initialized successfully.");
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"Database initialization error: {ex.Message}");
                // In a production environment, you might want to handle this differently
                // For now, we'll just log the error and continue with mock functionality
            }
        }
        
        // Method to open connection
        public SqlConnection OpenConnection()
        {
            if (_useMockDatabase)
            {
                Console.WriteLine("Using mock database connection.");
                return null;
            }
            
            try
            {
                // Check if connection string is initialized
                if (string.IsNullOrEmpty(_connectionString))
                {
                    throw new InvalidOperationException("The ConnectionString property has not been initialized.");
                }
                
                if (_connection == null)
                {
                    _connection = new SqlConnection(_connectionString);
                }
                
                if (_connection.State != ConnectionState.Open)
                {
                    _connection.Open();
                    Console.WriteLine("Database connection opened successfully.");
                }
                
                return _connection;
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"Error opening database connection: {ex.Message}");
                _useMockDatabase = true;
                Console.WriteLine("Falling back to mock database functionality.");
                return null;
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"Invalid operation: {ex.Message}");
                _useMockDatabase = true;
                Console.WriteLine("Falling back to mock database functionality.");
                return null;
            }
        }
        
        // Method to close connection
        public void CloseConnection()
        {
            try
            {
                if (_connection != null && _connection.State == ConnectionState.Open)
                {
                    _connection.Close();
                    Console.WriteLine("Database connection closed.");
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"Error closing database connection: {ex.Message}");
            }
        }
        
        // Method to execute a non-query command (INSERT, UPDATE, DELETE)
        public int ExecuteNonQuery(string query, Dictionary<string, object>? parameters = null)
        {
            try
            {
                using (SqlConnection connection = OpenConnection())
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    // Add parameters if provided
                    if (parameters != null)
                    {
                        foreach (var param in parameters)
                        {
                            command.Parameters.AddWithValue(param.Key, param.Value);
                        }
                    }
                    
                    Console.WriteLine($"Executing non-query: {query}");
                    return command.ExecuteNonQuery();
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"Error executing non-query: {ex.Message}");
                throw;
            }
        }
        
        // Method to execute a query that returns a scalar value
        public object? ExecuteScalar(string query, Dictionary<string, object>? parameters = null)
        {
            try
            {
                using (SqlConnection connection = OpenConnection())
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    // Add parameters if provided
                    if (parameters != null)
                    {
                        foreach (var param in parameters)
                        {
                            command.Parameters.AddWithValue(param.Key, param.Value);
                        }
                    }
                    
                    Console.WriteLine($"Executing scalar query: {query}");
                    return command.ExecuteScalar();
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"Error executing scalar query: {ex.Message}");
                throw;
            }
        }
        
        // Method to execute a query that returns a data reader
        public SqlDataReader ExecuteReader(string query, Dictionary<string, object>? parameters = null)
        {
            try
            {
                SqlConnection connection = OpenConnection();
                SqlCommand command = new SqlCommand(query, connection);
                
                // Add parameters if provided
                if (parameters != null)
                {
                    foreach (var param in parameters)
                    {
                        command.Parameters.AddWithValue(param.Key, param.Value);
                    }
                }
                
                Console.WriteLine($"Executing reader query: {query}");
                // CommandBehavior.CloseConnection will close the connection when the reader is closed
                return command.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"Error executing reader query: {ex.Message}");
                throw;
            }
        }
        
        // Method to execute a query that returns a DataTable
        public DataTable ExecuteQuery(string query, Dictionary<string, object>? parameters = null)
        {
            try
            {
                // Check if we're using mock database
                if (_useMockDatabase)
                {
                    Console.WriteLine($"Using mock database for query: {query}");
                    return new DataTable(); // Return empty DataTable for mock mode
                }
                
                SqlConnection connection = OpenConnection();
                // Check if connection is null
                if (connection == null)
                {
                    Console.WriteLine("Cannot execute query: Database connection is null");
                    return new DataTable(); // Return empty DataTable when connection fails
                }
                
                using (connection)
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    // Add parameters if provided
                    if (parameters != null)
                    {
                        foreach (var param in parameters)
                        {
                            command.Parameters.AddWithValue(param.Key, param.Value);
                        }
                    }
                    
                    Console.WriteLine($"Executing query: {query}");
                    
                    DataTable dataTable = new DataTable();
                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        adapter.Fill(dataTable);
                    }
                    
                    return dataTable;
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"Error executing query: {ex.Message}");
                throw;
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"Invalid operation: {ex.Message}");
                return new DataTable(); // Return empty DataTable on invalid operation
            }
        }
        
        // Method to begin a transaction
        public SqlTransaction BeginTransaction()
        {
            try
            {
                SqlConnection connection = OpenConnection();
                return connection.BeginTransaction();
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"Error beginning transaction: {ex.Message}");
                throw;
            }
        }
    }
}