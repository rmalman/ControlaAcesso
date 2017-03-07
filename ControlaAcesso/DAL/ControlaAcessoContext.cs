using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ControlaAcesso.Models;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using Microsoft.AspNet.Identity.EntityFramework;

namespace ControlaAcesso.DAL
{
    public class ControlaAcessoContext : DbContext
    {
        public ControlaAcessoContext() : base("ControlaAcessoContext")
        {
            
            
        }
        public DbSet<Visita> Visitas { get; set; }
        public DbSet<Visitante> Visitantes { get; set; }
        
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}