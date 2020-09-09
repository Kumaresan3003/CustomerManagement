using CustomerManagement.Data;
using CustomerManagement.Models;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerManagement.Repository
{
    public class CustomerDetailRepository : ICustomerDetailRepository
    {
        private readonly ICustomerDbContext _customerDbContext;

        public CustomerDetailRepository(ICustomerDbContext customerDbContext)
        {
            _customerDbContext = customerDbContext;
        }

        public async Task<Customer> GetCustomer(string id) => await _customerDbContext.GetCustomerDataContainer.ReadItemAsync<Customer>(id, new PartitionKey(id));

        public async Task<IEnumerable<Customer>> GetCustomers(string postCode, string dateOfBirth)
        {
            var result = _customerDbContext.GetCustomerSearchContainer.GetItemLinqQueryable<Customer>();
            var queryResultSetIterator = result.Where(x => x.Postcode == postCode).ToFeedIterator();
            if (!string.IsNullOrEmpty(dateOfBirth))
            {
                queryResultSetIterator = result.Where(x => x.Postcode == postCode && x.DateOfBirth == Convert.ToDateTime(dateOfBirth)).ToFeedIterator();
            }
            List<Customer> customers = new List<Customer>();

            while (queryResultSetIterator.HasMoreResults)
            {
                FeedResponse<Customer> currentResultSet = await queryResultSetIterator.ReadNextAsync();
                foreach (Customer customer in currentResultSet)
                {
                    customers.Add(customer);
                }
            }

            return customers;
        }
        public async Task<Customer> Insert(Customer entity)
        {
            await this._customerDbContext.GetCustomerDataContainer.CreateItemAsync(entity, new PartitionKey(entity.Id));
            return entity;

        }

        public async Task<Customer> Update(string id, Customer entity)
        {
            return await _customerDbContext.GetCustomerDataContainer.ReplaceItemAsync(entity, entity.Id, new PartitionKey(entity.Id));
        }
    }
}
