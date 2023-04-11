using CI_Platform.Areas.Admin.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform.DataAccess.Repository.IRepository
{
    public interface IAdminRepository
    {
        UserCrudViewModel GetAllUsers();
    }
}
