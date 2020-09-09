using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerManagement.Models
{
    public class CosmosDbSettings
    {
        public string Account { get; set; }

        public string Key { get; set; }

        public string DatabaseName { get; set; }

        public string CustomerDataContainerName { get; set; }

        public string CustomerLeaseContainerName { get; set; }

        public string CustomerSearchContainerName { get; set; }
    }
}
