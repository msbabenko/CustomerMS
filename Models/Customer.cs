using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerLogin.Models
{
    public class Customer
    {
       
        [Key]
        public int CustomerID { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string PANnumber { get; set; }
        public string Aadhaarnumber { get; set; }
        public string DateOfBirth { get; set; }
     
    }
}
