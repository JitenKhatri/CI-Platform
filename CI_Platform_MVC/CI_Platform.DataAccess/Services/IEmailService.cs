using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform.DataAccess.Services
{
    public interface IEmailService
    {
        Task SendAsync(string email, string subject, string message);
    }
}
