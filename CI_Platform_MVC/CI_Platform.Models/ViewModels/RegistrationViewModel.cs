using System.ComponentModel.DataAnnotations;

namespace CI_Platform.ViewModels
{
    public class RegistrationViewModel
    {
        [Required(ErrorMessage = "First name is required")]
        public string? FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required")]
        public string? LastName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Password is required")]
        [RegularExpression(@"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[\W_]).{8,15}$", ErrorMessage = "Password must be at least 8 characters long and contain at least one uppercase letter, one lowercase letter, one number and one special character and not longer than 15")]
        public string Password { get; set; }= null!;

        [Required(ErrorMessage = "Phone number is required")]
        [MaxLength(10, ErrorMessage = "Phone number can have maximum 10 numbers")]
        public int PhoneNumber { get; set; }
        
        [Required(ErrorMessage = "Confirm password is required")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords do not match")]
        public string ConfirmPassword { get; set; }
    }
}
