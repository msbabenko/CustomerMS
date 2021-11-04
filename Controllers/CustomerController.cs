using CustomerLogin.Models;
using CustomerLogin.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CustomerLogin.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
   
    public class CustomerController : ControllerBase
    {
        private  readonly CustomerService _customerService;

        public CustomerController(CustomerService customerService)
        {
            _customerService = customerService;

        }

        // GET: api/<CustomerController>
        [HttpGet]
        public IEnumerable<Customer> Get()
        {
            if (_customerService.GetAll() != null)
            {
                return   _customerService.GetAll();
            }
            return null;
        }

        // GET api/<CustomerController>/5
        [HttpGet("{Customerid}")]
        public Customer Get(int Customerid)
        {

            if (_customerService.GetCustomer(Customerid) != null)
            {
                return _customerService.GetCustomer(Customerid);
            }
            return null;
        }

        //public Customer Get(int id)
        //{
        //    Customer Cid = _customerService.GetCustomer(id);
        //    if ( Cid != null)
        //    {
        //        return Cid ;
        //    }
        //    return null;
        //}

        // POST api/<CustomerController>
        [HttpPost]
        public async Task<ActionResult<CustomerDTO>> Post([FromBody] CustomerDTO loginDto)
        {
            CustomerDTO loginDto1 = _customerService.Register(loginDto);
            if (loginDto1 != null)
            {
                return Ok(loginDto1);
            }
            return BadRequest("ID Already Exists");
        }

        [Route("Update")]
        [HttpPost]
        public ActionResult<CustomerDTO> Update([FromBody] CustomerDTO customer)
        {
            var CustomerDTO = _customerService.Update(customer);
            if (CustomerDTO != null)
                return Ok(CustomerDTO);
            return BadRequest("Please Enter Valid CustomerID");

        }

        // PUT api/<CustomerController>/5
        [Route("Login")]
        [HttpPost]
        public async Task<ActionResult<CustomerDTO>> Login([FromBody] CustomerDTO loginDto)
        {
            CustomerDTO dto = _customerService.Login(loginDto);
            if (dto != null)
            {
                return dto;
            }
            return BadRequest("Invalid User");
        }

       
    }
}
