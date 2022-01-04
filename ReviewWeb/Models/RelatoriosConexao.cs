using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Modelo;

namespace ReviewWeb.Models
{
    public class RelatoriosConexao: DbContext
    {
        static RelatoriosConexao()
        {
            Database.SetInitializer<RelatoriosConexao>(null);
        }

        public RelatoriosConexao(): base("Name=SqlServer")
        {

        }

        public DbSet<ModeloCliente> cliente { get; set; }
        public DbSet<HorasContrato> horascontrato { get; set; }
        public DbSet<Alocacao_Func> alocacao_func { get; set; }
        public DbSet<ModeloFuncionarios> funcionarios { get; set; }
        public DbSet<ModeloContratos> contratos { get; set; }
        
    }
}