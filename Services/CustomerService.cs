using CustomerLogin.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
                       //CustomerID=loginDto.CustomerID,
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
                   
                    loginDto.Password = "";

                    var creationstatus = new CustomerCreationStatus()
                    {
                        CustomerID = loginDto.CustomerID,
                        AccountCreationStatus = "Customer Created"
                    };

                    _customerContext.CustomerCreationStatuses.Add(creationstatus);

                    try
                    {
                        _customerContext.SaveChanges();
                    }
                    catch (DbUpdateConcurrencyException ce)
                    {
                        Debug.WriteLine(ce.Message);
                    }
                    catch (DbUpdateException ue)
                    {
                        Debug.WriteLine(ue.Message);
                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine(e.Message);
                    }

                    loginDto.CustomerID = _customerContext.customers.Where(b => b.Email == login.Email).FirstOrDefault().CustomerID; 
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
       
        public Customer Update(int id, CustomerDTO customer)
        {
            //using var hmac = new HMACSHA512();
            try
            {
                var editCust = _customerContext.customers.FirstOrDefault(p => p.CustomerID == id);
                if (editCust != null)
                {

                    editCust.Name = customer.Name;
                   // editCust.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(customer.Password));
                   // editCust.PasswordSalt = hmac.Key;
                    editCust.Address = customer.Address;
                    editCust.Email = customer.Email;
                    editCust.PANnumber = customer.PANnumber;
                    editCust.Aadhaarnumber = customer.Aadhaarnumber;
                    editCust.DateOfBirth = customer.DateOfBirth;

                    _customerContext.customers.Update(editCust);
                    _customerContext.SaveChanges();
                    return editCust;
                }
                else
                {
                    Console.WriteLine(" ID IS Not Found");
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
