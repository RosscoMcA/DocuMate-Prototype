using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IntegratedProject3.Models
{
    public class DocumentViewModel
    {
       
      public ICollection<Document> Documents { get; set; }
       // public Document Documents { get; set; }

       public ICollection<Revision> Revisions { get; set; }
       //public Revision Revisions { get; set; }
    }

    public class SingleDocViewModel
    {
        public Document Document { get; set; }
        public string DocTitle { get; set; }
    }

    public class  SDVMDetails
    {
        public Document Document { get; set; }
        public string DocTitle { get; set; }
        public double RevisionNum { get; set; }
    }
}