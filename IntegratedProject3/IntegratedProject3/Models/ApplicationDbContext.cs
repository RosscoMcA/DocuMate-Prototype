
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace IntegratedProject3.Models
{ 
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {

        /// <summary>
        /// Content Tables
        /// </summary>
        /// 
        public IDbSet<Account>Accounts { get; set; }
        public IDbSet<Document>Documents { get; set; }
        public IDbSet<Revision>Revisions { get; set; }

        public ApplicationDbContext()
            : base("IP3", throwIfV1Schema: false)
        {
         
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

    }
}