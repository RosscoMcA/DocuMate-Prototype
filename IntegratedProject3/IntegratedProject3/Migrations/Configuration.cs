namespace IntegratedProject3.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Models;
    using System;
    using System.Collections.ObjectModel;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<IntegratedProject3.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(IntegratedProject3.Models.ApplicationDbContext context)
        {
            if (System.Diagnostics.Debugger.IsAttached == false)
                System.Diagnostics.Debugger.Launch();
            try
            {
                //Execute the following when there are no users in the database
                if (!context.Users.Any())
                {


                    Account admin1 = new Account
                    {
                        Email = "nikki.clelland@doc.com",
                        UserName = "nikki.clelland@doc.com",
                        FirstName = "Nikki",
                        Surname = "Clelland",
                        EmailConfirmed = true

                    };

                    Account admin2 = new Account
                    {
                        Email = "james.craig@doc.com",
                        UserName = "james.craig@doc.com",
                        FirstName = "James",
                        Surname = "Craig",
                        EmailConfirmed = true

                    };

                    Account admin3 = new Account
                    {
                        Email = "ross.mcarthur@doc.com",
                        UserName = "ross.mcarthur@doc.com",
                        FirstName = "Nikki",
                        Surname = "Clelland",
                        EmailConfirmed = true

                    };

                    Account author = new Account
                    {
                        Email = "kappa@kappa.com",
                        UserName = "kappa@kappa.com",
                        FirstName = "Kappa",
                        Surname = "Kappa",
                        EmailConfirmed = true
                    };

                    Account distributee = new Account
                    {
                        Email = "distributee@d.com",
                        UserName = "distributee@d.com",
                        FirstName = "Distributee",
                        Surname = "1",
                        EmailConfirmed = true
                    };

                    Account distributee2 = new Account
                    {
                        Email = "distributee2@d.com",
                        UserName = "distributee2@d.com",
                        FirstName = "Distributee",
                        Surname = "2",
                        EmailConfirmed = true
                    };


                    //<summary>
                    //Password details of the accounts (Seeded data has email as password)
                    //</summary> 
                    string admin1Password = admin1.Email;
                    string admin2Password = admin2.Email;
                    string admin3Password = admin3.Email;
                    string authorPassword = author.Email;
                    string distributee1Password = distributee.Email;
                    string distributee2Password = distributee2.Email;

                    //Creating admins 
                    CreateAdmin(context, admin1, admin1Password);
                    CreateAdmin(context, admin2, admin2Password);
                    CreateAdmin(context, admin3, admin3Password);

                    //Creating Author
                    CreateAuthor(context, author, authorPassword);

                    //Creating Distributees
                    CreateDistributee(context, distributee, distributee1Password);
                    CreateDistributee(context, distributee2, distributee2Password);

                    //Constructing 
                    Document doc = new Document
                    {
                        id = Guid.NewGuid().ToString(),
                        Author = author
                    };

                    Models.Revisions v1 = new Models.Revisions
                    {
                        id = Guid.NewGuid().ToString(),
                        DocumentTitle = "Test Document",
                        RevisionNum = 1.1,
                        document = doc,
                        State = DocumentState.Archived,
                        DocCreationDate = DateTime.Today.AddDays(-3).Date,
                        ActivationDate = DateTime.Today.AddDays(-1).Date, 
                        Distributees = new Collection<Account>()
                    };

                    Models.Revisions v2 = new Models.Revisions
                    {
                        id = Guid.NewGuid().ToString(),
                        DocumentTitle = "New Test Document",
                        RevisionNum = 2.1,
                        document = doc,
                        State = DocumentState.Active,
                        DocCreationDate = DateTime.Today.Date,
                        ActivationDate = DateTime.Today.Date, 
                        Distributees = new Collection<Account>()
                    };

                    context.Documents.Add(doc);
                    context.SaveChanges();
                    v1.Distributees.Add(distributee);

                    v2.Distributees.Add(distributee);
                    v2.Distributees.Add(distributee2);

                    context.Versions.Add(v1);

                    context.Versions.Add(v2);

                                     

                    context.SaveChanges();
                }
            }
            catch (Exception e) { }
        }

        private void CreateAdmin(ApplicationDbContext con, Account admin, string password)
        {
            //Creates and intiailises the componients of adding the admin
            var userStore = new UserStore<Account>(con);
            var userManager = new UserManager<Account>(userStore);
            userManager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 1,
                RequireNonLetterOrDigit = false,
                RequireLowercase = false,
                RequireUppercase = false,
                RequireDigit = false
            };
            //Enum asignment for the account type
            admin.AccountType = AccountType.Admin;
            //Adds the admin to the database 
            var userCreateResult = userManager.Create(admin, password);

            //If the creation of the Staff has failed throw exception
            if (!userCreateResult.Succeeded)
            {
                throw new Exception(string.Join(";", userCreateResult.Errors));
            }
        }

        private void CreateAuthor(ApplicationDbContext con, Account author, string password)
        {
            //Creates and intiailises the componients of adding the admin
            var userStore = new UserStore<Account>(con);
            var userManager = new UserManager<Account>(userStore);
            userManager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 1,
                RequireNonLetterOrDigit = false,
                RequireLowercase = false,
                RequireUppercase = false,
                RequireDigit = false
            };
            //Enum asignment for the account type
            author.AccountType = AccountType.Author;
            //Adds the admin to the database 
            var userCreateResult = userManager.Create(author, password);

            //If the creation of the Staff has failed throw exception
            if (!userCreateResult.Succeeded)
            {
                throw new Exception(string.Join(";", userCreateResult.Errors));
            }
        }

        private void CreateDistributee(ApplicationDbContext con, Account distributee, string password)
        {
            //Creates and intiailises the componients of adding the admin
            var userStore = new UserStore<Account>(con);
            var userManager = new UserManager<Account>(userStore);
            userManager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 1,
                RequireNonLetterOrDigit = false,
                RequireLowercase = false,
                RequireUppercase = false,
                RequireDigit = false
            };
            //Enum asignment for the account type
            distributee.AccountType = AccountType.Distributee;
            //Adds the admin to the database 
            var userCreateResult = userManager.Create(distributee, password);

            //If the creation of the Staff has failed throw exception
            if (!userCreateResult.Succeeded)
            {
                throw new Exception(string.Join(";", userCreateResult.Errors));
            }
        }

    }
}

