using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IntegratedProject3.Models
{
    public class RevisionViewModel
    {
        public string DocID { get; set; }
        [Display(Name = "Revision Number")]
        public double RevisionNum { get; set; }
        [Display(Name = "Document Title")]
        public string DocumentTitle { get; set; }
        public string FileStoreKey { get; set; }
        
    }

    /// <summary>
    /// Handles the selection of distributees for a single revision
    /// </summary>
    public class DistributeeSelectModel
    {
        /// <summary>
        /// Attributes
        /// </summary>
        public string RevID { get; set; }

        public IEnumerable<Account>Accounts { get; set; }

    }
}