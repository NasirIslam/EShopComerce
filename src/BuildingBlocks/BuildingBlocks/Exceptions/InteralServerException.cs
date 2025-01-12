using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildingBlocks.Exceptions
{
    public class InteralServerException:Exception
    {
        public string? Details{get;}
        public InteralServerException(string message):base(message) { }
        public InteralServerException(string message,string details):base(message)
        {
            Details = details;            
        }

    }
}
