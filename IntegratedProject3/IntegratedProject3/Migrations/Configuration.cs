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
                        Email = "acmeAdmin@acme.com",
                        UserName = "acmeAdmin@acme.com",
                        FirstName = "Nikki",
                        Surname = "Clelland",
                        PhoneNumber = "07507984652",
                        EmailConfirmed = true

                    };

                    Account admin2 = new Account
                    {
                        Email = "james.craig@doc.com",
                        UserName = "james.craig@doc.com",
                        FirstName = "James",
                        Surname = "Craig",
                        PhoneNumber = "07507984652",
                        EmailConfirmed = true

                    };

                    Account admin3 = new Account
                    {
                        Email = "ross.mcarthur@doc.com",
                        UserName = "ross.mcarthur@doc.com",
                        FirstName = "Nikki",
                        Surname = "Clelland",
                        PhoneNumber = "07507984652",
                        EmailConfirmed = true

                    };

                    Account author = new Account
                    {
                        Email = "john@acme.com",
                        UserName = "john@acme.com",
                        FirstName = "John",
                        Surname = "Johnstone",
                        PhoneNumber = "07507984652",
                        EmailConfirmed = true
                    };

                    Account distributee = new Account
                    {
                        Email = "jcraig203@caledonian.ac.uk",
                        UserName = "jcraig203@caledonian.ac",
                        FirstName = "James",
                        Surname = "Craig",
                        PhoneNumber = "07507984652",
                        EmailConfirmed = true
                    };

                    Account distributee2 = new Account
                    {
                        Email = "ral@acme.com",
                        UserName = "ral@acme.com",
                        FirstName = "Ramba",
                        Surname = "Ral",
                        PhoneNumber = "07507984652",
                        EmailConfirmed = true
                    };

                    Account distributee3 = new Account
                    {
                        Email = "jackson@acme.com",
                        UserName = "jackson@acme.com",
                        FirstName = "Jackson",
                        Surname = "King",
                        PhoneNumber = "07507984652",
                        EmailConfirmed = true
                    };

                    Account distributee4 = new Account
                    {
                        Email = "steve@acme.com",
                        UserName = "steve@acme.com",
                        FirstName = "Steve",
                        Surname = "Smith",
                        PhoneNumber = "07507984652",
                        EmailConfirmed = true
                    };

                    Account distributee5 = new Account
                    {
                        Email = "mark@acme.com",
                        UserName = "mark@acme.com",
                        FirstName = "Mark",
                        Surname = "King",
                        PhoneNumber = "07507984652",
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
                    string distributee3Password = distributee3.Email;
                    string distributee4Password = distributee4.Email;
                    string distributee5Password = distributee5.Email;

                    //Creating admins 
                    CreateAdmin(context, admin1, admin1Password);
                    CreateAdmin(context, admin2, admin2Password);
                    CreateAdmin(context, admin3, admin3Password);

                    //Creating Author
                    CreateAuthor(context, author, authorPassword);

                    //Creating Distributees
                    CreateDistributee(context, distributee, distributee1Password);
                    CreateDistributee(context, distributee2, distributee2Password);
                    CreateDistributee(context, distributee3, distributee3Password);
                    CreateDistributee(context, distributee4, distributee4Password);
                    CreateDistributee(context, distributee5, distributee5Password);

                    //Constructing 
                    Document doc = new Document
                    {
                        id = Guid.NewGuid().ToString(),
                        Author = author
                    };

                    Document doc2 = new Document
                    {
                        id = Guid.NewGuid().ToString(),
                        Author = author
                    };

                    Document doc3 = new Document
                    {
                        id = Guid.NewGuid().ToString(),
                        Author = author
                    };
					
					Document doc4 = new Document
                    {
                        id = Guid.NewGuid().ToString(),
                        Author = author
                    };
					
					Document doc5 = new Document
                    {
                        id = Guid.NewGuid().ToString(),
                        Author = author
                    };
					
					Document doc6 = new Document
                    {
                        id = Guid.NewGuid().ToString(),
                        Author = author
                    };
					
					Document doc7 = new Document
                    {
                        id = Guid.NewGuid().ToString(),
                        Author = author
                    };


                    Models.Revision v1 = new Models.Revision
                    {
                        id = Guid.NewGuid().ToString(),
                        DocumentTitle = "Acme Health and Saftey Guidelines DRAFT.docx",
                        RevisionNum = 1.1,
                        document = doc,
                        State = DocumentState.Archived,
                        DocCreationDate = DateTime.Today.AddDays(-3).Date,
                        ActivationDate = DateTime.Today.AddDays(-1).Date,
                        Distributees = new Collection<Account>()
                    };

                    Models.Revision v2 = new Models.Revision
                    {
                        id = Guid.NewGuid().ToString(),
                        DocumentTitle = "Acme Health and Safety Guidelines FINAL.docx",
                        RevisionNum = 2.1,
                        document = doc,
                        State = DocumentState.Active,
                        DocCreationDate = DateTime.Today.Date,
                        ActivationDate = DateTime.Today.Date,
                        Distributees = new Collection<Account>()
                    };

                    Models.Revision v3 = new Models.Revision
                    {
                        id = Guid.NewGuid().ToString(),
                        DocumentTitle = "Acme Devlelopment Guidelines V1",
                        RevisionNum = 1.1,
                        document = doc2,
                        State = DocumentState.Archived,
                        DocCreationDate = DateTime.Today.Date,
                        ActivationDate = DateTime.Today.Date,
                        Distributees = new Collection<Account>()
                    };

                    Models.Revision v4 = new Models.Revision
                    {
                        id = Guid.NewGuid().ToString(),
                        DocumentTitle = "Acme Development Guidelines V2",
                        RevisionNum = 2.1,
                        document = doc2,
                        State = DocumentState.Active,
                        DocCreationDate = DateTime.Today.Date,
                        ActivationDate = DateTime.Today.Date,
                        Distributees = new Collection<Account>()
                    };

                    Models.Revision v5 = new Models.Revision
                    {
                        id = Guid.NewGuid().ToString(),
                        DocumentTitle = "Acme Document 3 DRAFT",
                        RevisionNum = 1.1,
                        document = doc3,
                        State = DocumentState.Archived,
                        DocCreationDate = DateTime.Today.Date,
                        ActivationDate = DateTime.Today.Date,
                        Distributees = new Collection<Account>()
                    };

                    Models.Revision v6 = new Models.Revision
                    {
                        id = Guid.NewGuid().ToString(),
                        DocumentTitle = "Acme Document 3",
                        RevisionNum = 2.1,
                        document = doc3,
                        State = DocumentState.Active,
                        DocCreationDate = DateTime.Today.Date,
                        ActivationDate = DateTime.Today.Date,
                        Distributees = new Collection<Account>()
                    };

                    Models.Revision v7 = new Models.Revision
                    {
                        id = Guid.NewGuid().ToString(),
                        DocumentTitle = "Acme Document 3 V2",
                        RevisionNum = 3.1,
                        document = doc3,
                        State = DocumentState.Draft,
                        DocCreationDate = DateTime.Today.Date,
                        ActivationDate = DateTime.Today.Date,
                        Distributees = new Collection<Account>()
                    };
					
					Models.Revision v8 = new Models.Revision
                    {
                        id = Guid.NewGuid().ToString(),
                        DocumentTitle = "Acme Document 4",
                        RevisionNum = 1.1,
                        document = doc4,
                        State = DocumentState.Draft,
                        DocCreationDate = DateTime.Today.AddDays(-3).Date,
                        ActivationDate = DateTime.Today.AddDays(-1).Date,
                        Distributees = new Collection<Account>()
                    };
					
					Models.Revision v9 = new Models.Revision
                    {
                        id = Guid.NewGuid().ToString(),
                        DocumentTitle = "Acme Document 5 (Archived)",
                        RevisionNum = 1.1,
                        document = doc5,
                        State = DocumentState.Archived,
                        DocCreationDate = DateTime.Today.AddDays(-3).Date,
                        ActivationDate = DateTime.Today.AddDays(-1).Date,
                        Distributees = new Collection<Account>()
                    };
					
					Models.Revision v10 = new Models.Revision
                    {
                        id = Guid.NewGuid().ToString(),
                        DocumentTitle = "Acme Document 6",
                        RevisionNum = 1.1,
                        document = doc6,
                        State = DocumentState.Active,
                        DocCreationDate = DateTime.Today.AddDays(-3).Date,
                        ActivationDate = DateTime.Today.AddDays(-1).Date,
                        Distributees = new Collection<Account>()
                    };
					
					Models.Revision v11 = new Models.Revision
                    {
                        id = Guid.NewGuid().ToString(),
                        DocumentTitle = "Acme Document 7",
                        RevisionNum = 1.1,
                        document = doc7,
                        State = DocumentState.Active,
                        DocCreationDate = DateTime.Today.AddDays(-3).Date,
                        ActivationDate = DateTime.Today.AddDays(-1).Date,
                        Distributees = new Collection<Account>()
                    };

                    context.Documents.Add(doc);
                    context.SaveChanges();
                    v1.Distributees.Add(distributee);

                    v2.Distributees.Add(distributee);
                    v2.Distributees.Add(distributee2);
                    v2.Distributees.Add(distributee3);
                    v2.Distributees.Add(distributee4);
                    v2.Distributees.Add(distributee5);

                    v3.Distributees.Add(distributee);
                    v3.Distributees.Add(distributee2);
                    v3.Distributees.Add(distributee3);

                    v4.Distributees.Add(distributee);
                    v4.Distributees.Add(distributee2);
                    v4.Distributees.Add(distributee3);
                    v4.Distributees.Add(distributee5);

                    v5.Distributees.Add(distributee);
                    v5.Distributees.Add(distributee2);
                    v5.Distributees.Add(distributee3);
                    v5.Distributees.Add(distributee4);

                    v6.Distributees.Add(distributee);
                    v6.Distributees.Add(distributee2);
                    v6.Distributees.Add(distributee3);
                    v6.Distributees.Add(distributee4);
                    v6.Distributees.Add(distributee5);

                    v7.Distributees.Add(distributee);
                    v7.Distributees.Add(distributee2);
                    v7.Distributees.Add(distributee3);
                    v7.Distributees.Add(distributee4);
                    v7.Distributees.Add(distributee5);
					
					v8.Distributees.Add(distributee);
                    v8.Distributees.Add(distributee2);
                    v8.Distributees.Add(distributee3);
                    v8.Distributees.Add(distributee4);
                    v8.Distributees.Add(distributee5);
					
					v9.Distributees.Add(distributee);
                    v9.Distributees.Add(distributee2);
                    v9.Distributees.Add(distributee3);
                    v9.Distributees.Add(distributee4);
                    v9.Distributees.Add(distributee5);
					
					v10.Distributees.Add(distributee);
                    v10.Distributees.Add(distributee2);
                    v10.Distributees.Add(distributee3);
                    v10.Distributees.Add(distributee4);
                    v10.Distributees.Add(distributee5);
					
					v11.Distributees.Add(distributee);
                    v11.Distributees.Add(distributee2);
                    v11.Distributees.Add(distributee3);
                    v11.Distributees.Add(distributee4);
                    v11.Distributees.Add(distributee5);

                    context.Revisions.Add(v1);
                    context.Revisions.Add(v2);
                    context.Revisions.Add(v3);
                    context.Revisions.Add(v4);
                    context.Revisions.Add(v5);
                    context.Revisions.Add(v6);
                    context.Revisions.Add(v7);
                    context.Revisions.Add(v8);
                    context.Revisions.Add(v9);
                    context.Revisions.Add(v10);
                    context.Revisions.Add(v11);


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
