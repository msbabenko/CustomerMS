using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerLogin.Models
{
    public class CustomerCreationStatus
    {
        [Key]
        [ForeignKey("Customer")]
        public int CustomerID { get; set; }
        public string AccountCreationStatus { get; set; }
    }
}
