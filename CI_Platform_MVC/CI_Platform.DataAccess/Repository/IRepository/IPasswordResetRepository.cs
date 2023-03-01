using CI_Platform.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform.DataAccess.Repository.IRepository
{
    public interface IPasswordResetRepository : IRepository<PasswordReset>
    {
        void Add(PasswordReset passwordReset);
        void Remove(PasswordReset passwordReset);
    }
}
