using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform.DataAccess.Repository.IRepository
{
   public interface IAllRepository
    {

        void save();
        public IUserAuthentication UserAuthentication { get; }
        public IPasswordResetRepository PasswordResetRepository { get; }
        //public IMission Mission { get; }
    }
}
