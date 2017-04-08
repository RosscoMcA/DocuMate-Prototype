using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IntegratedProject3.Models
{
    public class RevisonViewModel
    {

        public string RevisionNum { get; set; }
        public string DocumentTitle { get; set; }
        public virtual IEnumerable<Account> Distributees { get; set; }
        public virtual IEnumerable<Account> Accounts { get; set; }

        public RevisonViewModel(IEnumerable<Account> accounts)
        {
            this.Accounts = accounts;
        }


    }
}