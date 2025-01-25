using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicesLibrary.Exceptions.UserManagerExceptions
{
    public class ProfileCreationFailedException : Exception
    {
        public ProfileCreationFailedException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
