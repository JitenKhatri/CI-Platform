using CI_Platform.Models;
using CI_Platform.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform.DataAccess.Repository.IRepository
{
    public interface IUserAuthentication : IRepository<User>
    {
        User ResetPassword(string password, long id);
        void Add(PasswordReset resetPasswordInfo);
        EditProfileViewModel GetUser(long UserId, int country);

        bool ChangePassword(long UserId, string password);

        bool UpdateProfile(EditProfileViewModel model, long user_id);

        bool ContactUs(long user_id, string Subject, string Message);
    }
}
