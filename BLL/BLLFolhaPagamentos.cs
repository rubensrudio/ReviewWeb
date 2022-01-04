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
    public class BLLFolhaPagamentos
    {
        private DALConexao conexao;
        public BLLFolhaPagamentos(DALConexao cx)
        {
            this.conexao = cx;
        }
        public void Incluir(ModeloFolhaPagamentos modelo)
        {
            DALFolhaPagamentos DALobj1 = new DALFolhaPagamentos(conexao);
            DALobj1.Incluir(modelo);
        }
        public void Excluir(string mes_base)
        {
            DALFolhaPagamentos DALobj = new DALFolhaPagamentos(conexao);
            DALobj.Excluir(mes_base);
        }
        public DataTable Localizar(String valor, String buscapor, int idempresas, int pageNumber, int RowsPage, string ordenapor)
        {
            DALFolhaPagamentos DALobj = new DALFolhaPagamentos(conexao);
            return DALobj.Localizar(valor, buscapor, idempresas, pageNumber, RowsPage, ordenapor);
        }

        public ModeloFolhaPagamentos CarregaFolhaPagamentos(string mes_base)
        {
            DALFolhaPagamentos DALobj = new DALFolhaPagamentos(conexao);
            return DALobj.CarregaFolha_Pagamento(mes_base);
        }
        public int TotalFolhaPagamento()
        {
            DALFolhaPagamentos DALobj = new DALFolhaPagamentos(conexao);
            return DALobj.TotalFolha_Pagamento();
        }
    }
}
