using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace IntegratedProject3.Models
{
    /// <summary>
    /// Code first representation of the Version Table of the database
    /// </summary>
    public class Revisions
    {
        /// <summary>
        /// Reference of the document class - Document that this revision is applicable to.
        /// </summary>
        [Key]
        public virtual Document document { get; set; }

        /// <summary>
        /// UID
        /// </summary>
        [Key]
        public string id { get; set; }

        /// <summary>
        /// User specificed revision number
        /// </summary>
        public double RevisionNum { get; set; }

        /// <summary>
        /// Title of the document 
        /// </summary>
        public string DocumentTitle { get; set; }

        
        /// <summary>
        /// Date of Document Creation
        /// </summary>
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString ="{0:d}", ApplyFormatInEditMode =true)]
        public DateTime DocCreationDate { get; set; }
        
        /// <summary>
        /// List of account Distributees for the system
        /// </summary>
        public virtual ICollection<Account>Distributees { get; set; }

        /// <summary>
        /// Defines the state of the document. 
        /// </summary>
        public DocumentState State { get; set; }
        /// <summary>
        /// Date of document activation
        /// </summary>
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        public DateTime ActivationDate { get; set; }

    }

    public enum DocumentState { Active, Draft, Archived }
}