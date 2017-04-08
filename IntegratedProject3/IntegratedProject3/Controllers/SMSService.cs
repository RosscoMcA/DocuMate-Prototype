using IntegratedProject3.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IntegratedProject3.Controllers
{
    public class SMSService
    {
        /// <summary>
        /// Sends a preset message to all relative accounts
        /// </summary>
        /// <param name="accounts">The Accounts to distribute messages to</param>
        /// <param name="message">The message to be sent</param>
        private void sendMassSMS(ICollection<Account>accounts, string message)
        {
            //For Every account, send a message
            foreach (var account in accounts)
            {
                IdentityMessage im = new IdentityMessage();

                im.Body = message;
                im.Destination = account.PhoneNumber;
                im.Subject = "DocuMate Notification";

                SmsService sms = new SmsService();

                sms.SendAsync(im);
            }
        }

        /// <summary>
        /// Determines what mass distributed message to distributees is sent
        /// </summary>
        /// <param name="accounts"></param>
        /// <param name="options">1) new Document accessable, 2) document has been archived</param>
        public void DetermineSMSMessage(ICollection<Account>accounts, int options)
        {
            string message;

            switch(options){
                case 1:
                    message = "You have been added to a new distributee list";

                    sendMassSMS(accounts, message); 

                    break;

                case 2:
                    message = "A Document you are attached to has been archived";

                    sendMassSMS(accounts, message); 

                    break;


                default:
                    throw new Exception("Error, invalid choice");
                    
            }

        }
    }
}