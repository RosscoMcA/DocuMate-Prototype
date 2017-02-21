using IntegratedProject3.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IP3.Data
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
        public int ID { get; set; }

        /// <summary>
        /// Navigational Property for The author
        /// </summary>
        public virtual Account Author { get; set; }
    }

    
}