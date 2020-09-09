using Microsoft.Azure.Cosmos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerManagement.Data
{
    public interface ICustomerDbContext
    {
        public Container GetCustomerDataContainer { get; }

        public Container GetCustomerLeaseContainer { get; }

        public Container GetCustomerSearchContainer { get; }
    }
}
