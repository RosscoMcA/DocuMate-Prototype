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
    
    /// <notes>
    /// 24/02/2017 - Nikki: Currently there is not a way to store the actual document. Ross is looking at methods to store the documents in the database.
    /// It has been agreed that the actual documents will be stored along side the revisions to allow for the storage of multiple different versions of each
    /// document. The documents class will be used as a folder to hold all of the revisions.
    /// </notes>

    /// <summary>
    /// Code first representation of the Version Table of the database
    /// </summary>
    public class Revision
    {
        /// <summary>
        /// Reference of the document class - Document that this version is applicable to.
        /// </summary>
        
        public virtual Document document { get; set; }

        /// <summary>
        /// UID
        /// </summary>
        [Key]
        public string id { get; set; }

        /// <summary>
        ///  Revision identifer 
        /// </summary>
        [Display(Name = "Revision Number")]
        public double RevisionNum { get; set; }

        /// <summary>
        /// Title of the document 
        /// </summary>
        [Display(Name = "Document Title")]
        public string DocumentTitle { get; set; }
        
        /// <summary>
        /// Date of Document Creation
        /// </summary>
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString ="{0:d}", ApplyFormatInEditMode =true)]
        [Display(Name = "Creation Date")]
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
        [Display(Name = "Activation Date")]
        public DateTime? ActivationDate { get; set; }

        /// <summary>
        /// Key for revisions file stored on the Amazon File Store Service
        /// </summary>
        public string fileStoreKey { get; set; }

    }

    public enum DocumentState { Active, Draft, Archived}

}