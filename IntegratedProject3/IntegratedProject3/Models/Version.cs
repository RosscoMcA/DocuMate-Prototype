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
    public class Version
    {
        /// <summary>
        /// 
        /// </summary>
        [Key]
        public virtual Document document { get; set; }

        [Key]
        public double RevisionNum { get; set; }

        public string DocumentTitle { get; set; }
        
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString ="{0:d}", ApplyFormatInEditMode =true)]
        public DateTime DocCreationDate { get; set; }
        
        public virtual ICollection<Acccount>Distributees { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        public DateTime ActivationDate { get; set; }

    }
}