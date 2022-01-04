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
    public class BLLTreinamentos
    {
        private DALConexao conexao;
        public BLLTreinamentos(DALConexao cx)
        {
            this.conexao = cx;
        }
        public void Incluir(ModeloTreinamentos modelo)
        {
            DALTreinamentos DALobj1 = new DALTreinamentos(conexao);
            DALobj1.Incluir(modelo);
        }
        public void Alterar(ModeloTreinamentos modelo)
        {
            DALTreinamentos DALobj1 = new DALTreinamentos(conexao);
            DALobj1.Alterar(modelo);
        }
        public void Excluir(int codigo)
        {
            DALTreinamentos DALobj = new DALTreinamentos(conexao);
            DALobj.Excluir(codigo);
        }
        public DataTable Localizar(String valor, String buscapor)
        {
            DALTreinamentos DALobj = new DALTreinamentos(conexao);
            return DALobj.Localizar(valor, buscapor);
        }

        public DataTable Localizar(String valor, String buscapor, int idempresas, int pageNumber, int RowsPage, string ordenapor)
        {
            DALTreinamentos DALobj = new DALTreinamentos(conexao);
            return DALobj.Localizar(valor, buscapor, idempresas, pageNumber, RowsPage, ordenapor);
        }

        public ModeloTreinamentos CarregaTreinamentos(int codigo)
        {
            DALTreinamentos DALobj = new DALTreinamentos(conexao);
            return DALobj.CarregaTreinamentos(codigo);
        }

        public int VencimentoTreinamento(int codigo)
        {
            DALTreinamentos DALobj = new DALTreinamentos(conexao);
            return DALobj.VencimentoTreinamento(codigo);
        }
        public int TotalTreinamentos()
        {
            DALTreinamentos DALobj = new DALTreinamentos(conexao);
            return DALobj.TotalTreinamentos();
        }
    }
}
