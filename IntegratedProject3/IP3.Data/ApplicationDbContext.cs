using IP3.Data;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace IP3.Data
{ 
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {

        /// <summary>
        /// Content Tables
        /// </summary>
        /// 
        public IDbSet<Account>Accounts { get; set; }
        public IDbSet<Document>Documents { get; set; }
        public IDbSet<Version>Versions { get; set; }

        public ApplicationDbContext()
            : base("IP3Project", throwIfV1Schema: false)
        {
            Database.SetInitializer(new DropCreateDatabaseAlways<ApplicationDbContext>());
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}