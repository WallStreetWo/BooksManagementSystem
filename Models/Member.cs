using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace BooksManagementSystem.Models
{
    public class Member
    {
        public int MemberID { get; set; }

        [Required]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Name can only contain letters and spaces")]
        public string Name { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Phone number must be exactly 10 digits")]
        public string PhoneNumber { get; set; }
    }
}
