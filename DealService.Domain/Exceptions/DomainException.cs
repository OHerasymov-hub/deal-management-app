using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace DealService.Domain.Exceptions
{
    public class DomainException : Exception
    {
        public DomainException(string message) : base(message)
        {
        }
    }
}
