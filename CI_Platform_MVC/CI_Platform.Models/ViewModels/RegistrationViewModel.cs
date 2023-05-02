using System.ComponentModel.DataAnnotations;

namespace CI_Platform.ViewModels
{
    public class RegistrationViewModel
    {
        [Required(ErrorMessage = "First name is required")]
        [RegularExpression(@"^[^\s]{3,20}$", ErrorMessage = "First name must have between 3 and 20 characters and no spaces.")]
        public string? FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required")]
        [RegularExpression(@"^[^\s]{3,20}$", ErrorMessage = "Last name must have between 3 and 20 characters and no spaces.")]
        public string? LastName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [RegularExpression(@"^((([!#$%&'*+\-/=?^_`{|}~\w])|([!#$%&'*+\-/=?^_`{|}~\w][!#$%&'*+\-/=?^_`{|}~\.\w]{0,}[!#$%&'*+\-/=?^_`{|}~\w]))[@]\w+([-.]\w+)*\.\w+([-.]\w+)*)$", ErrorMessage = "Please enter a valid Email")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Password is required")]
        [RegularExpression(@"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[\W_]).{8,15}$", ErrorMessage = "Password must be at least 8 characters long and contain at least one uppercase letter, one lowercase letter, one number and one special character and not longer than 15")]
        public string Password { get; set; }= null!;

        [Required(ErrorMessage = "Phone number is required")]
        [Range(1000000000, 9999999999, ErrorMessage = "Phone number has to have only/must 10 positive numbers")]
        public long PhoneNumber { get; set; }
        
        [Required(ErrorMessage = "Confirm password is required")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords do not match")]
        public string ConfirmPassword { get; set; }
    }
}
