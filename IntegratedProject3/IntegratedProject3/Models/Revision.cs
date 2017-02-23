using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace IntegratedProject3.Models
{

    /// <change_log>
    /// Class has been renamed from "Version" to "Revision" due to Visual Studio having it's on conflicting class "System.Version".
    /// It was agreed that it was better to rename to "Revision" instead of using "Model.Version" for the sake of readiblity.
    /// </change_log>

    /// <summary>
    /// Code first representation of the Version Table of the database
    /// </summary>
    public class Revision
    {
        /// <summary>
        /// Reference of the document class - Document that this version is applicable to.
        /// </summary>
        [Key]
        public virtual Document document { get; set; }

        /// <summary>
        ///  Composite key
        /// </summary>
        [Key]
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

    public enum DocumentState { Active, Draft, Archived}
}