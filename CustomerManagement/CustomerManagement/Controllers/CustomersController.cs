using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CustomerManagement.Data;
using CustomerManagement.Models;
using CustomerManagement.Repository;

namespace CustomerManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerDetailRepository _customerDetailRepository;

        public CustomersController(ICustomerDetailRepository customerDetailRepository)
        {
            _customerDetailRepository = customerDetailRepository;
        }

        // GET: api/Customers/1
        //[HttpGet("{id}")]
        [HttpGet("byId")]
        public async Task<ActionResult<Customer>> Get(string id)
        {
            var customer = await _customerDetailRepository.GetCustomer(id);

            if (customer == null)
            {
                return NotFound();
            }

            return customer;
        }

        // GET: api/Customers/eh111uf
        //[HttpGet("{postCode:required}")]
        [HttpGet("byPostCode")]
        public async Task<ActionResult<IEnumerable<Customer>>> GetCustomer(string postCode, string dateOfBirth)
        {
            var customers = await _customerDetailRepository.GetCustomers(postCode, dateOfBirth);

            if (customers == null)
            {
                return NotFound();
            }

            return customers.ToList();
        }

        // PUT: api/Customers/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCustomer(string id, Customer customer)
        {
            if (id != customer.Id)
            {
                return BadRequest();
            }

            await _customerDetailRepository.Update(id, customer);

            return NoContent();
        }

        // POST: api/Customers
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Customer>> PostCustomer(Customer customer)
        {
            await _customerDetailRepository.Insert(customer);

            return CreatedAtAction("Get", new { id = customer.Id }, customer);
        }

        // DELETE: api/Customers/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Customer>> DeleteCustomer(string id, Customer customer)
        {
            if (id != customer.Id)
            {
                return BadRequest();
            }

            customer.Ttl = 1;
            var result = await _customerDetailRepository.Update(id, customer);

            if (result == null)
            {
                return NotFound();
            }
            return result;
        }
    }
}
