using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using IntegratedProject3.Models;
using Microsoft.AspNet.Identity;

namespace IntegratedProject3.Controllers
{
    public class RootController : Controller
    {
        public ApplicationDbContext db = new ApplicationDbContext(); 


        /// <summary>
        /// Checks that the current user is the owner of this revision
        /// </summary>
        /// <param name="revision">The Revision in question</param>
        /// <returns></returns>
        public bool VerifyAuthor(Revision revision)
        {
            //was object, object isn't going to help in this case because an Object may not be returned
            var currentUser = getAccount();
            return (currentUser.Id == revision.document.Author.Id);

        }

        public bool VerifyAuthor(Document document)
        {
            var currentUser = getAccount();
            return (currentUser.Id == document.Author.Id);
        }

        /// <summary>
        /// Checks the document state is "Draft".
        /// </summary>
        /// <param name="revision"></param>
        /// <returns>Boolean value, true if the document state is "Draft".</returns>
        public bool VerifyDocumentState(Revision revision)
        {
            return (revision.State.Equals(DocumentState.Draft));
        }

        /// <summary>
        /// gets the current users account
        /// </summary>
        /// <returns>The account if it is found, otherwise null</returns>
        public Account getAccount()
        {
            var userID = User.Identity.GetUserId();

            var accountFound = db.Accounts.Where(u=> u.Id == userID).SingleOrDefault();
            if (accountFound != null) return accountFound;
            else
            {
                return null;
            }
        }


        /// <summary>
        /// Checks if the current user is an Admin
        /// </summary>
        /// <returns>True if admin is logged in, otherwise false</returns>
        public bool isAdmin()
        {
            var account = getAccount();
            if (account != null)
            {
                switch (account.AccountType)
                {
                    case AccountType.Admin:
                        return true;
                    case AccountType.Author:
                    case AccountType.Distributee:
                    default:
                        return false;
                }

            }
            else
            {

                return false;
            }

        }

              
        
        /// <summary>
        /// Checks if the current user is a Distributee
        /// </summary>
        /// <returns>True if distributee is logged in, otherwise false</returns>
        public bool isDistributee()
        {
            var account = getAccount();
            if (account != null)
            {
                switch (account.AccountType)
                {
                    case AccountType.Distributee:
                        return true;
                    case AccountType.Author:
                    case AccountType.Admin:
                    default:
                        return false;
                }

            }
            else
            {

                return false;
            }
        }


        /// <summary>
        /// Checks if the current user is an Author
        /// </summary>
        /// <returns>True if author is logged in, otherwise false</returns>
        public bool isAuthor()
        {
            var account = getAccount();
            if (account != null)
            {
                switch (account.AccountType)
                {
                    case AccountType.Author:
                        return true;
                    case AccountType.Admin:
                    case AccountType.Distributee:
                    default:
                        return false;
                }

            }
            else
            {

                return false;
            }

        }

    }
}