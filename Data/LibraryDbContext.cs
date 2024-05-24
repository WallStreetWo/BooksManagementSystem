using System.Collections.Generic;
using System.Threading.Tasks;
using BooksManagementSystem.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;


namespace BooksManagementSystem.Data;
public class LibraryDbContext
{
    private readonly string _connectionString;

    //My constructor that is used to initialize my connection string from IIConfiguration
    public LibraryDbContext(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("LibraryDB");
    }


    // Method to that is used to get books from the database
    public async Task<IEnumerable<Book>> GetBooksAsync()
    {
        var books = new List<Book>();

        // Establishing connection to the database
        using (SqlConnection conn = new SqlConnection(_connectionString))
        {

            // SQL query to select all books from the Books table
            SqlCommand cmd = new SqlCommand("SELECT * FROM Books", conn);
            conn.Open();

            // Executing the SQL command and reading data
            using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
            {
                // Looping through the result set and adding books to the list
                while (reader.Read())
                {
                    books.Add(new Book
                    {
                        BookID = reader.GetInt32(0),
                        Title = reader.GetString(1),
                        Author = reader.GetString(2),
                        PublicationYear = reader.GetInt32(3)
                    });
                }
            }
        }
        // Returning the list of books
        return books;
    }


    // Method used to get members from the database
    public async Task<IEnumerable<Member>> GetMembersAsync()
    {
        var members = new List<Member>();

        // Establishing connection to the database
        using (SqlConnection conn = new SqlConnection(_connectionString))
        {
            // SQL query to select all members from the Members table
            SqlCommand cmd = new SqlCommand("SELECT * FROM Members", conn);
            conn.Open();

            // Executing the SQL command and reading data
            using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
            {
                // Looping through the result set and adding members to the list
                while (reader.Read())
                {
                    members.Add(new Member
                    {
                        // Mapping database columns to Member properties
                        MemberID = reader.GetInt32(0),
                        Name = reader.GetString(1),
                        Address = reader.GetString(2),
                        PhoneNumber = reader.GetString(3)
                    });
                }
            }
        }

        // Returning the list of members
        return members;
    }


    public async Task<IEnumerable<Borrowing>> GetBorrowingsAsync()
    {
        var borrowings = new List<Borrowing>();

        // Establishing connection to the database
        using (SqlConnection conn = new SqlConnection(_connectionString))
        {
            // SQL query to select all borrowings from the Borrowings table
            SqlCommand cmd = new SqlCommand("SELECT * FROM Borrowings", conn);
            conn.Open();

            // Asynchronously executing the SQL command and reading data
            using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
            {
                // Looping through the result set and adding borrowings to the list
                while (reader.Read())
                {
                    borrowings.Add(new Borrowing
                    {
                        // Mapping database columns to Borrowing properties
                        BorrowingID = reader.GetInt32(0),
                        BookID = reader.GetInt32(1),
                        MemberID = reader.GetInt32(2),
                        BorrowDate = reader.GetDateTime(3),
                        ReturnDate = reader.IsDBNull(4) ? null : (DateTime?)reader.GetDateTime(4)
                    });
                }
            }
        }

        // Returning the list of borrowings
        return borrowings;
    }

     // Method to add a new book to the database
    public async Task AddBookAsync(Book book)
    {
        using (SqlConnection conn = new SqlConnection(_connectionString))
        {
            SqlCommand cmd = new SqlCommand("INSERT INTO Books (Title, Author, PublicationYear) VALUES (@Title, @Author, @PublicationYear)", conn);
            cmd.Parameters.AddWithValue("@Title", book.Title);
            cmd.Parameters.AddWithValue("@Author", book.Author);
            cmd.Parameters.AddWithValue("@PublicationYear", book.PublicationYear);
            conn.Open();
            await cmd.ExecuteNonQueryAsync();
        }
    }

    // Method to update an existing book in the database
    public async Task UpdateBookAsync(Book book)
    {
        using (SqlConnection conn = new SqlConnection(_connectionString))
        {
            SqlCommand cmd = new SqlCommand("UPDATE Books SET Title = @Title, Author = @Author, PublicationYear = @PublicationYear WHERE BookID = @BookID", conn);
            cmd.Parameters.AddWithValue("@BookID", book.BookID);
            cmd.Parameters.AddWithValue("@Title", book.Title);
            cmd.Parameters.AddWithValue("@Author", book.Author);
            cmd.Parameters.AddWithValue("@PublicationYear", book.PublicationYear);
            conn.Open();
            await cmd.ExecuteNonQueryAsync();
        }
    }

    // Method to delete a book from the database
    public async Task DeleteBookAsync(int BookID)
    {
        using (SqlConnection conn = new SqlConnection(_connectionString))
        {
            SqlCommand cmd = new SqlCommand("DELETE FROM Books WHERE BookID = @BookID", conn);
            cmd.Parameters.AddWithValue("@BookID", BookID);
            conn.Open();
            await cmd.ExecuteNonQueryAsync();
        }
    }

    // Method to add a new member to the database
    public async Task AddMemberAsync(Member member)
    {
        using (SqlConnection conn = new SqlConnection(_connectionString))
        {
            SqlCommand cmd = new SqlCommand("INSERT INTO Members (Name, Address, PhoneNumber) VALUES (@Name, @Address, @PhoneNumber)", conn);
            cmd.Parameters.AddWithValue("@Name", member.Name);
            cmd.Parameters.AddWithValue("@Address", member.Address);
            cmd.Parameters.AddWithValue("@PhoneNumber", member.PhoneNumber);
            conn.Open();
            await cmd.ExecuteNonQueryAsync();
        }
    }

    // Method to update an existing member in the database
    public async Task UpdateMemberAsync(Member member)
    {
        using (SqlConnection conn = new SqlConnection(_connectionString))
        {
            SqlCommand cmd = new SqlCommand("UPDATE Members SET Name = @Name, Address = @Address, PhoneNumber = @PhoneNumber WHERE MemberID = @MemberID", conn);
            cmd.Parameters.AddWithValue("@MemberID", member.MemberID);
            cmd.Parameters.AddWithValue("@Name", member.Name);
            cmd.Parameters.AddWithValue("@Address", member.Address);
            cmd.Parameters.AddWithValue("@PhoneNumber", member.PhoneNumber);
            conn.Open();
            await cmd.ExecuteNonQueryAsync();
        }
    }

    // Method to delete a member from the database
    public async Task DeleteMemberAsync(int MemberID)
    {
        using (SqlConnection conn = new SqlConnection(_connectionString))
        {
            SqlCommand cmd = new SqlCommand("DELETE FROM Members WHERE MemberID = @MemberID", conn);
            cmd.Parameters.AddWithValue("@MemberID", MemberID);
            conn.Open();
            await cmd.ExecuteNonQueryAsync();
        }
    }

    // Method to add a new borrowing to the database
    public async Task AddBorrowingAsync(Borrowing borrowing)
    {
        using (SqlConnection conn = new SqlConnection(_connectionString))
        {
            SqlCommand cmd = new SqlCommand("INSERT INTO Borrowings (BookID, MemberID, BorrowDate, ReturnDate) VALUES (@BookID, @MemberID, @BorrowDate, @ReturnDate)", conn);
            cmd.Parameters.AddWithValue("@BookID", borrowing.BookID);
            cmd.Parameters.AddWithValue("@MemberID", borrowing.MemberID);
            cmd.Parameters.AddWithValue("@BorrowDate", borrowing.BorrowDate);
            cmd.Parameters.AddWithValue("@ReturnDate", (object)borrowing.ReturnDate ?? DBNull.Value);
            conn.Open();
            await cmd.ExecuteNonQueryAsync();
        }
    }

    // Method to update an existing borrowing in the database
    public async Task UpdateBorrowingAsync(Borrowing borrowing)
    {
        using (SqlConnection conn = new SqlConnection(_connectionString))
        {
            SqlCommand cmd = new SqlCommand("UPDATE Borrowings SET BookID = @BookID, MemberID = @MemberID, BorrowDate = @BorrowDate, ReturnDate = @ReturnDate WHERE BorrowingID = @BorrowingID", conn);
            cmd.Parameters.AddWithValue("@BorrowingID", borrowing.BorrowingID);
            cmd.Parameters.AddWithValue("@BookID", borrowing.BookID);
            cmd.Parameters.AddWithValue("@MemberID", borrowing.MemberID);
            cmd.Parameters.AddWithValue("@BorrowDate", borrowing.BorrowDate);
            cmd.Parameters.AddWithValue("@ReturnDate", (object)borrowing.ReturnDate ?? DBNull.Value);
            conn.Open();
            await cmd.ExecuteNonQueryAsync();
        }
    }

    // Method to delete a borrowing from the database
    public async Task DeleteBorrowingAsync(int BorrowingID)
    {
        using (SqlConnection conn = new SqlConnection(_connectionString))
        {
            SqlCommand cmd = new SqlCommand("DELETE FROM Borrowings WHERE BorrowingID = @BorrowingID", conn);
            cmd.Parameters.AddWithValue("@BorrowingID", BorrowingID);
            conn.Open();
            await cmd.ExecuteNonQueryAsync();
        }
    }
}



