using CustomerManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerManagement.Repository
{
    public interface ICustomerDetailRepository
    {
        Task<IEnumerable<Customer>> GetCustomers(string postCode, string dateOfBirth);

        Task<Customer> GetCustomer(string id);

        Task<Customer> Insert(Customer entity);

        Task<Customer> Update(string id, Customer entity);
    }
}
