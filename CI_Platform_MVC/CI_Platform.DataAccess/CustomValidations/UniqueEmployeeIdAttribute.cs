using CI_Platform.Models;
using CI_Platform.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform.DataAccess.CustomValidations
{
    public class UniqueEmployeeIdAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var viewModel = (AddUserViewModel)validationContext.ObjectInstance;

            // Check if the employee ID already exists in the database
            if (IsEmployeeIdExists(viewModel.EmployeeId))
            {
                return new ValidationResult("Employee ID already exists");
            }
            
            return ValidationResult.Success;
        }

        public bool IsEmployeeIdExists(string employeeId)
        {
          using (var dbContext = new CiPlatformContext())
            {
                var User = dbContext.Users.FirstOrDefault(u => u.EmployeeId == employeeId);
                if(User !=null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
    }
}
