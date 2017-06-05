using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EESLP.Services.Logging.API.Infrastructure.Exceptions
{
    public class EntityNotFoundException : Exception
    {
        public EntityNotFoundException() : base()
        {
        }

        public EntityNotFoundException(string message) : base(message)
        {

        }

        public EntityNotFoundException(string message, Exception inner) : base(message, inner)
        {

        }
    }
}
