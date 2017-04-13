using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IntegratedProject3.Models
{
    public class DocumentViewModel
    {
        public string id { get; set; }
        [Display(Name = "Document Title")]
        public string DocumentTitle { get; set; }
        [Display(Name = "Author")]
        public Account Author { get; set; }
        [Display(Name = "Revision Number")]
        public double RevisionNum { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        [Display(Name = "Creation Date")]
        public DateTime DocCreationDate { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        [Display(Name = "Activation Date")]
        public DateTime? ActivationDate { get; set; }
    }
}