using ControlaAcesso.DAL;
using ControlaAcesso.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

[assembly: OwinStartupAttribute(typeof(ControlaAcesso.Startup))]
namespace ControlaAcesso
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);

            createRolesandUsers();
            CarregaVisitante();
        }
        // In this method we will create default User roles and Admin user for login  
        private void createRolesandUsers()
        {
            ApplicationDbContext context = new ApplicationDbContext();
            //ControlaAcessoContext context = new ControlaAcessoContext();

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            //var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new ApplicationDbContext()));
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));


            // In Startup iam creating first Admin Role and creating a default Admin User   
            if (!roleManager.RoleExists("Admin"))
            {
                // first we create Admin rool  
                var role = new IdentityRole();
                role.Name = "Admin";
                roleManager.Create(role);

                //Here we create a Admin super user who will maintain the website                 

                var user = new ApplicationUser();
                user.UserName = "devteam";
                user.Email = "raphael.malman@first-tech.com";

                string userPWD = "Ftech@123";

                var chkUser = UserManager.Create(user, userPWD);

                //Add default User to Role Admin  
                if (chkUser.Succeeded)
                {
                    var result1 = UserManager.AddToRole(user.Id, "Admin");

                }

            }




            // creating Creating Diretoria role   
            if (!roleManager.RoleExists("Diretoria"))
            {
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "Diretoria";
                roleManager.Create(role);

            }


            // creating Creating Manager role   
            if (!roleManager.RoleExists("Recursos Humanos"))
            {
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "Recursos Humanos";
                roleManager.Create(role);

            }


            // creating Creating Employee role   
            if (!roleManager.RoleExists("Recepção"))
            {
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "Recepção";
                roleManager.Create(role);

            }

        }

        public static void CarregaVisitante()
        {
            ControlaAcessoContext context = new ControlaAcessoContext();

            var CadVisitante = new List<Visitante>();

            List<Visitante> visitante = new List<Visitante>();
            visitante = context.Visitantes.OrderBy(v => v.VisitanteID).ToList();

            if (visitante.Count(a => a.CPF == 11194112706) == 0)
            {
                CadVisitante.Add(
                    new Visitante
                    {
                        Nome = UpperCase("Raphael Malman"),
                        DataNascimento = DateTime.ParseExact("17/12/1979", "dd/MM/yyyy", CultureInfo.InvariantCulture),
                        CPF = 11194112706
                    }
                );
            }
            if (visitante.Count(a => a.CPF == 01268668729) == 0)
            {
                CadVisitante.Add(new Visitante
                {
                    Nome = UpperCase("FELIPE GOMES"),
                    DataNascimento = DateTime.ParseExact("07/02/1973", "dd/MM/yyyy", CultureInfo.InvariantCulture),
                    CPF = 01268668729
                });
            }
            if (visitante.Count(a => a.CPF == 02943126764) == 0)
            {
                CadVisitante.Add(new Visitante
                {
                    Nome = UpperCase("LEANDRO MARCONDES"),
                    DataNascimento = DateTime.ParseExact("25/09/1971", "dd/MM/yyyy", CultureInfo.InvariantCulture),
                    CPF = 02943126764
                });
            }
            if (visitante.Count(a => a.CPF == 10061281743) == 0)
            {
                CadVisitante.Add(new Visitante
                {
                    Nome = UpperCase("Vitor Hugo"),
                    DataNascimento = DateTime.ParseExact("14/11/1982", "dd/MM/yyyy", CultureInfo.InvariantCulture),
                    CPF = 10061281743
                });
            }




            CadVisitante.ForEach(s => context.Visitantes.Add(s));
            context.SaveChanges();
        }

        public static string UpperCase(String s)
        {
            // Check for empty string.
            if (string.IsNullOrEmpty(s))
            {
                return string.Empty;
            }
            // Return char and concat substring.
            return s.ToUpper();
        }


    }
}
