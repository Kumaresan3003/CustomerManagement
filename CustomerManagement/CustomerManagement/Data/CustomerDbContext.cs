using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CustomerManagement.Models;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Options;
using System.Threading;

namespace CustomerManagement.Data
{
    public class CustomerDbContext : CosmosClient, ICustomerDbContext
    {
        private readonly CosmosDbSettings cosmosDbSetting;

        private ChangeFeedProcessor processor;

        public CustomerDbContext(CosmosDbSettings cosmosDbSetting) : base(cosmosDbSetting.Account, cosmosDbSetting.Key, new CosmosClientOptions { ApplicationName = "CustomerManagement" })
        {
            this.cosmosDbSetting = cosmosDbSetting;
            this.CreateDatabaseAsync();
        }
        public Container GetCustomerDataContainer { get; private set; }

        public Container GetCustomerLeaseContainer { get; private set; }

        public Container GetCustomerSearchContainer { get; private set; }

        private async void CreateDatabaseAsync()
        {

            Database database = await this.CreateDatabaseIfNotExistsAsync(cosmosDbSetting.DatabaseName);

            this.GetCustomerDataContainer = await database.CreateContainerIfNotExistsAsync(new ContainerProperties(cosmosDbSetting.CustomerDataContainerName, "/id"));

            this.GetCustomerLeaseContainer = await database.CreateContainerIfNotExistsAsync(new ContainerProperties(cosmosDbSetting.CustomerLeaseContainerName, "/id"));

            this.GetCustomerSearchContainer = await database.CreateContainerIfNotExistsAsync(new ContainerProperties(cosmosDbSetting.CustomerSearchContainerName, "/postCode"));

            this.processor = await StartChangeFeedProcessorAsync();
        }

        private async Task<ChangeFeedProcessor> StartChangeFeedProcessorAsync()
        {
            var changeFeedProcessor = GetCustomerDataContainer
                .GetChangeFeedProcessorBuilder<Customer>(processorName: "customerChangeFeed", HandleChangesAsync)
                .WithInstanceName("customerManagement")
                .WithLeaseContainer(GetCustomerLeaseContainer)
                .Build();

            await changeFeedProcessor.StartAsync();
            return changeFeedProcessor;
        }

        private async Task HandleChangesAsync(IReadOnlyCollection<Customer> changes, CancellationToken cancellationToken)
        {
            foreach (Customer customerItem in changes)
            {
                try
                {
                    await GetCustomerSearchContainer.ReadItemAsync<Customer>(customerItem.Id, new PartitionKey(customerItem.Postcode));

                    await GetCustomerSearchContainer.ReplaceItemAsync(customerItem, customerItem.Id, new PartitionKey(customerItem.Postcode));
                }
                catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    await GetCustomerSearchContainer.CreateItemAsync(customerItem, new PartitionKey(customerItem.Postcode));
                }
            }

        }
    }
}
