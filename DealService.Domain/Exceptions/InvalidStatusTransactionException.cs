using DealService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Text;

namespace DealService.Domain.Exceptions
{
    public class InvalidStatusTransactionException : DomainException
    {
        public InvalidStatusTransactionException(DealStatus from, DealStatus to) : base($"Invalid status transition from {from} to {to}")
        {
        }
    }
}
