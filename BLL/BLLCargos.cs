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
    public class BLLCargos
    {
        private DALConexao conexao;
        public BLLCargos(DALConexao cx)
        {
            this.conexao = cx;
        }
        public void Incluir(ModeloCargos modelo)
        {
            DALCargos DALobj1 = new DALCargos(conexao);
            DALobj1.Incluir(modelo);
        }
        public void Alterar(ModeloCargos modelo)
        {
            DALCargos DALobj1 = new DALCargos(conexao);
            DALobj1.Alterar(modelo);
        }
        public void Excluir(int codigo)
        {
            DALCargos DALobj = new DALCargos(conexao);
            DALobj.Excluir(codigo);
        }
        public DataTable Localizar(String valor, String buscapor)
        {
            DALCargos DALobj = new DALCargos(conexao);
            return DALobj.Localizar(valor, buscapor);
        }

        public DataTable Localizar(String valor, String buscapor, int idempresas, int pageNumber, int RowsPage, string ordenapor)
        {
            DALCargos DALobj = new DALCargos(conexao);
            return DALobj.Localizar(valor, buscapor, idempresas, pageNumber, RowsPage, ordenapor);
        }

        public ModeloCargos CarregaCargos(int codigo)
        {
            DALCargos DALobj = new DALCargos(conexao);
            return DALobj.CarregaCargos(codigo);
        }

        public int TotalCargos()
        {
            DALCargos DALobj = new DALCargos(conexao);
            return DALobj.TotalCargos();
        }
    }
}
