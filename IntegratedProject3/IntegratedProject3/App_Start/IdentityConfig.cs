using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using IntegratedProject3.Models;

using System.Net.Http;
using System.Net.Mail;
using SendGrid;
using System.Net;
using System.Configuration;
using Twilio;
using SendGrid.Helpers.Mail;

namespace IntegratedProject3
{
    /// <summary>
    /// Handles the distribution of emails
    /// </summary>
      public class EmailService : IIdentityMessageService
    {
        /// <summary>
        /// Handles the distribution of emails 
        /// </summary>
        /// <param name="message">The message to be sent to the specified recepient</param>
        /// <returns>The Status of the taske being sent</returns>
      public Task SendAsync(IdentityMessage message)
        {
            
            return configSendGridAsync(message);
        }

        /// <summary>
        /// Conducts the sending of emails to a single recipeient 
        /// </summary>
        /// <param name="message">The Message details to send</param>
        /// <returns>The Status of the email sent</returns>
        public Task configSendGridAsync(IdentityMessage message)
        {
            
            var client = new SendGridClient("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");
            var SGMessage = new SendGrid.Helpers.Mail.SendGridMessage()
            {
                From = new EmailAddress(
                                "DocuMate@DocuMate.com", "DocuMate Team"),
                Subject = message.Subject,
                PlainTextContent = message.Body,
                HtmlContent = message.Body

            };
            SGMessage.AddTo(new EmailAddress(message.Destination));

            var response = client.SendEmailAsync(SGMessage);


            return response;

        }
    }
   
    /// <summary>
    /// Service handles the send of and 
    /// </summary>
    public class SmsService : IIdentityMessageService
    {

        /// <summary>
        /// Conducts the sending of SMS Messages to a number
        /// </summary>
        /// <param name="message">The Message to send</param>
        /// <returns>THe status of the message sent</returns>
        public Task SendAsync(IdentityMessage message)
        {
            ///Creates the service and its credentials that are required
            var Twilio = new TwilioRestClient(
               ConfigurationManager.AppSettings["SMSAccountIdentification"],
               ConfigurationManager.AppSettings["SMSAccountPassword"]);

            ///Sends the message
            var res = Twilio.SendMessage(
                ConfigurationManager.AppSettings["SMSFrom"], message.Destination, message.Body);

            return Task.FromResult(0);
        }
    }

    // Configure the application user manager used in this application. UserManager is defined in ASP.NET Identity and is used by the application.
    public class ApplicationUserManager : UserManager<ApplicationUser>
    {
        public ApplicationUserManager(IUserStore<ApplicationUser> store)
            : base(store)
        {
        }

        public static ApplicationUserManager Create(IdentityFactoryOptions<ApplicationUserManager> options, IOwinContext context) 
        {
            var manager = new ApplicationUserManager(new UserStore<ApplicationUser>(context.Get<ApplicationDbContext>()));
            // Configure validation logic for usernames
            manager.UserValidator = new UserValidator<ApplicationUser>(manager)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };

            // Configure validation logic for passwords
            manager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 6,
                RequireNonLetterOrDigit = false,
                RequireDigit = true,
                RequireLowercase = true,
                RequireUppercase = true,
            };

            // Configure user lockout defaults
            manager.UserLockoutEnabledByDefault = true;
            manager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
            manager.MaxFailedAccessAttemptsBeforeLockout = 5;

            // Register two factor authentication providers. This application uses Phone and Emails as a step of receiving a code for verifying the user
            // You can write your own provider and plug it in here.
            manager.RegisterTwoFactorProvider("Phone Code", new PhoneNumberTokenProvider<ApplicationUser>
            {
                MessageFormat = "Your security code is {0}"
            });
            manager.RegisterTwoFactorProvider("Email Code", new EmailTokenProvider<ApplicationUser>
            {
                Subject = "Security Code",
                BodyFormat = "Your security code is {0}"
            });
           // manager.EmailService = new EmailService();
            manager.SmsService = new SmsService();
            var dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                manager.UserTokenProvider = 
                    new DataProtectorTokenProvider<ApplicationUser>(dataProtectionProvider.Create("ASP.NET Identity"));
            }
            return manager;
        }
    }

    // Configure the application sign-in manager which is used in this application.
    public class ApplicationSignInManager : SignInManager<ApplicationUser, string>
    {
        public ApplicationSignInManager(ApplicationUserManager userManager, IAuthenticationManager authenticationManager)
            : base(userManager, authenticationManager)
        {
        }

        public override Task<ClaimsIdentity> CreateUserIdentityAsync(ApplicationUser user)
        {
            return user.GenerateUserIdentityAsync((ApplicationUserManager)UserManager);
        }

        public static ApplicationSignInManager Create(IdentityFactoryOptions<ApplicationSignInManager> options, IOwinContext context)
        {
            return new ApplicationSignInManager(context.GetUserManager<ApplicationUserManager>(), context.Authentication);
        }
    }
}
