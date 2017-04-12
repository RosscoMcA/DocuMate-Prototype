using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace IntegratedProject3.Models
{
    /// <summary>
    /// Code First Representation of the Account table, with different criteria of their role in the system
    /// </summary>
    public class Account:ApplicationUser
    {
        
        public AccountType AccountType { get; set; }
        
        public string FirstName { get; set; }

        public string Surname { get; set; }
        
    }
    /// <summary>
    /// ENUM Representation of the system 
    /// </summary>
    public enum AccountType { Author, Admin, Distributee, Archived}

    
}