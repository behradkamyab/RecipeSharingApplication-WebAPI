using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicesLibrary.Interfaces
{
    // Todo need to implement confirmation email!
    public interface IMailService
    {
        Task SendEmailAsync(string toEmail , string subject , string body );
        Task<bool> ConfirmEmail(string userId, string token);

    }
}
