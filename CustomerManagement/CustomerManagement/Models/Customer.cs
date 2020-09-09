using Newtonsoft.Json;
using System;

namespace CustomerManagement.Models
{
    public class Customer
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime DateOfBirth { get; set; }

        public string AddressLine1 { get; set; }

        public string AddressLine2 { get; set; }

        [JsonProperty("postCode")]
        public string Postcode { get; set; }

        public string City { get; set; }

        public string Country { get; set; }

        public string LastUpdatedBy { get; set; }

        public DateTime LastUpdateAt { get; set; }

        [JsonProperty("ttl")]
        public int Ttl { get; set; }
    }
}
