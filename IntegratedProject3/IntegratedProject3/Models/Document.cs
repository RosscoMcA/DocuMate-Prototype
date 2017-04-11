using IntegratedProject3.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IntegratedProject3.Models
{
    /// <summary>
    /// Code first representation of the Document Table of the database.
    /// </summary>
    public class Document
    {
        /// <summary>
        /// Primary Key of Document, has the unique ID of a document
        /// </summary>
        [Key]
        public string id { get; set; }

        /// <summary>
        /// Navigational Property for The author
        /// </summary>
        public virtual Account Author { get; set; }

        public virtual ICollection<Revision>Revisions { get; set; }

    }

}