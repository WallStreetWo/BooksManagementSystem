using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace BooksManagementSystem.Models;

public class Book
{
    public int BookID { get; set; }
    
    [Required]
    public string Title { get; set; }

    [Required]
    [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Author Name can only contain letters and spaces")]
    public string Author { get; set; }

    [Range(1000, 2100, ErrorMessage = "Publication year must be between 1000 and 2100.")]
    public int PublicationYear { get; set; }
}