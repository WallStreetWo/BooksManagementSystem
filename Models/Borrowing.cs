namespace BooksManagementSystem.Models;

public class Borrowing
{
    public int BorrowingID { get; set; }
    public int BookID { get; set; }
    public int MemberID { get; set; }
    public DateTime BorrowDate { get; set; }
    public DateTime? ReturnDate { get; set; }
}