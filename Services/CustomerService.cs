using CustomerLogin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CustomerLogin.Services
{
    public class CustomerService
    {
        private  readonly CustomerContext _customerContext;

        public CustomerService(CustomerContext customerContext)
        {
            _customerContext = customerContext;
        }
        public CustomerDTO Register(CustomerDTO loginDto)
        {
            try
            {
                if (_customerContext.customers.Where(e => e.Email == loginDto.Email).Any())
                {
                    return null;
                }
                else
                {
                    using var hmac = new HMACSHA512();
                    var PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));
                    var PasswordSalt = hmac.Key;
                    Customer login = new Customer()
                    {
                          CustomerID=loginDto.CustomerID,
                        Email = loginDto.Email,
                        PasswordHash = PasswordHash,
                        PasswordSalt = PasswordSalt,
                        Address = loginDto.Address,
                        Phone = loginDto.Phone,
                        Name = loginDto.Name,
                        PANnumber=loginDto.PANnumber,
                         Aadhaarnumber=loginDto.Aadhaarnumber,
                         DateOfBirth=loginDto.DateOfBirth
    };
                    _customerContext.customers.Add(login);
                    _customerContext.SaveChanges();
                    loginDto.Password = "";
                    var creationstatus = new CustomerCreationStatus()
                    {
                        CustomerID = loginDto.CustomerID,
                        AccountCreationStatus = "Customer Created"
                    };
                    _customerContext.CustomerCreationStatuses.Add(creationstatus);
                    _customerContext.SaveChanges();
                    return loginDto;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return null;
        }
        public CustomerDTO Login(CustomerDTO loginDto)
        {
            try
            {
                Customer dto = null;
                dto = _customerContext.customers.FirstOrDefault(e => e.Email == loginDto.Email);
                if (dto == null)
                {
                    return null;
                }
                else
                {
                    using var hmac = new HMACSHA512(dto.PasswordSalt);
                    var PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));
                    for (int i = 0; i < PasswordHash.Length; i++)
                    {
                        if (PasswordHash[i] != dto.PasswordHash[i])
                            return null;
                    }
                    loginDto.CustomerID = dto.CustomerID;
                    loginDto.Name = dto.Name;
                    loginDto.Password = "";
                    return loginDto;
                }


            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return null;
        }
        public List<Customer> GetAll()
        {
            List<Customer> customers = _customerContext.customers.ToList();
            return customers;
        }
        public CustomerDTO Update(CustomerDTO customer)
        {
            int flag = 0;
            try
            {
                using var hmac = new HMACSHA512();
                foreach (var item in _customerContext.customers)
                {
                    if (item.CustomerID == customer.CustomerID)
                    {

                        flag = 1;
                        item.Name = customer.Name;
                        item.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(customer.Password));
                        item.PasswordSalt = hmac.Key;
                        item.Address = customer.Address;
                        item.Phone = customer.Phone;
                        item.Email = customer.Email;
                        item.PANnumber = customer.PANnumber;
                        item.Aadhaarnumber = customer.Aadhaarnumber;
                        item.DateOfBirth = customer.DateOfBirth;
                    }
                   
                }
                if(flag==1)
                {
                    _customerContext.SaveChanges();
                    return customer;
                }
                
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            
            return null;
        }
        public Customer GetCustomer(int id)
        {
            Customer FoundCustomer = null;
            foreach (var item in _customerContext.customers)
            {
                if (item.CustomerID.Equals(id))
                {
                    FoundCustomer = item;
                }
            }
            return FoundCustomer;
        }
    }
}
