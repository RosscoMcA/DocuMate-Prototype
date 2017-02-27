using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using IntegratedProject3.Models;

namespace IntegratedProject3.Controllers
{
    public class RootController : Controller
    {
        // GET: Root
        public ActionResult Index()
        {
            return View();
        }

        public Boolean VerifyAuthor(Revision revision)
        {

            object currentUser = Membership.GetUser().ProviderUserKey;
            return (currentUser == revision.document.Author);

        }

        public Boolean VerifyDocumentState(Revision revision)
        {
            return (revision.State.Equals(DocumentState.Draft));
        }

    }
}