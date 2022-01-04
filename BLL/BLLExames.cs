using DAL;
using Modelo;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class BLLExames
    {
        private DALConexao conexao;
        public BLLExames(DALConexao cx)
        {
            this.conexao = cx;
        }
        public void Incluir(ModeloExames modelo)
        {
            DALExames DALobj1 = new DALExames(conexao);
            DALobj1.Incluir(modelo);
        }
        public void Alterar(ModeloExames modelo)
        {
            DALExames DALobj1 = new DALExames(conexao);
            DALobj1.Alterar(modelo);
        }
        public void Excluir(int codigo)
        {
            DALExames DALobj = new DALExames(conexao);
            DALobj.Excluir(codigo);
        }
        public DataTable Localizar(String valor, String buscapor)
        {
            DALExames DALobj = new DALExames(conexao);
            return DALobj.Localizar(valor, buscapor);
        }

        public DataTable Localizar(String valor, String buscapor, int idempresas, int pageNumber, int RowsPage, string ordenapor)
        {
            DALExames DALobj = new DALExames(conexao);
            return DALobj.Localizar(valor, buscapor, idempresas, pageNumber, RowsPage, ordenapor);
        }

        public int VencimentoExame(int codigo)
        {
            DALExames DALobj = new DALExames(conexao);
            return DALobj.VencimentoExame(codigo);
        }

        public ModeloExames CarregaExames(int codigo)
        {
            DALExames DALobj = new DALExames(conexao);
            return DALobj.CarregaExames(codigo);
        }
        public int TotalExames()
        {
            DALExames DALobj = new DALExames(conexao);
            return DALobj.TotalExames();
        }
    }
}
