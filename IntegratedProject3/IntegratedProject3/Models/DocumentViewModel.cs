using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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

    public class SDVMDetails
    {
        public string id { get; set; }

        /// <summary>
        /// Navigational Property for The author
        /// </summary>
        public virtual Account Author { get; set; }

        
        public string DocTitle { get; set; }
        public double RevisionNum { get; set; }
       

        public static Expression<Func<Document, SDVMDetails>> ViewModel
        {
            get
            {
                return doc => new SDVMDetails()
                {
                    id = doc.id,
                    Author = doc.Author,
                    DocTitle = doc.Revisions.Where(r => r.document.id == doc.id && r.State == DocumentState.Active)
                    .SingleOrDefault().DocumentTitle,
                    RevisionNum = doc.Revisions.Where(r => r.document.id == doc.id && r.State == DocumentState.Active)
                    .SingleOrDefault().RevisionNum

                };
            }
        }
    }
}