using IntegratedProject3.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IntegratedProject3.Controllers
{
    /// <summary>
    /// Email Messaging Service, handles all interactions and processes that occur with the email service.
    /// </summary>
    public class EmailService
    {
    
            /// <summary>
            /// Handles the distribution of a message to all applicable users
            /// </summary>
            /// <param name="distributees">the users whom will be sent the message</param>
            /// <param name="message">the message they wish to send</param>
            public void distributeMessage(ICollection<Account> distributees, string message)
            {
                foreach (var distributee in distributees)
                {
                    IdentityMessage im = new IdentityMessage();
                    im.Body = message;
                    im.Destination = distributee.Email;
                    im.Subject = "DocuMate Notification";

                IntegratedProject3.EmailService es = new IntegratedProject3.EmailService();

                    es.SendAsync(im);
                }
            }

            /// <summary>
            /// Provides some commonly used notifiation messages to be mass distributed. 
            /// Current options are: 
            /// 1. Document access permitted message. 
            /// 2. Document no longer available message.
            /// </summary>
            /// <param name="choice">The generic messages choice</param>
            public void massMessageDistribution(ICollection<Account> accounts, int choice)
            {
                string message;
                switch (choice)
                {
                    case 1:
                        message = "Hi there! You have been added to a distribution list for a document. You can now view it on the DocuMate website!";
                        distributeMessage(accounts, message);
                        break;

                    case 2:
                        message = "Hi there! A document you were permitted to access is no longer available!";
                        distributeMessage(accounts, message);
                        break;

                    default:
                        throw new Exception("Err: You have not selected a viable choice, read the method summary for all choices! ");
                }
            }


        }
    }
