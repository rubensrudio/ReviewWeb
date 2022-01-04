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
    public class BLLDocumentos
    {
        private DALConexao conexao;
        public BLLDocumentos(DALConexao cx)
        {
            this.conexao = cx;
        }
        public void Incluir(ModeloDocumentos modelo)
        {
            DALDocumentos DALobj1 = new DALDocumentos(conexao);
            DALobj1.Incluir(modelo);
        }
        public void Alterar(ModeloDocumentos modelo)
        {
            DALDocumentos DALobj1 = new DALDocumentos(conexao);
            DALobj1.Alterar(modelo);
        }
        public void Excluir(int codigo)
        {
            DALDocumentos DALobj = new DALDocumentos(conexao);
            DALobj.Excluir(codigo);
        }
        public DataTable Localizar(String valor, String buscapor)
        {
            DALDocumentos DALobj = new DALDocumentos(conexao);
            return DALobj.Localizar(valor, buscapor);
        }

        public DataTable Localizar(String valor, String buscapor, int idempresas, int pageNumber, int RowsPage, string ordenapor)
        {
            DALDocumentos DALobj = new DALDocumentos(conexao);
            return DALobj.Localizar(valor, buscapor, idempresas, pageNumber, RowsPage, ordenapor);
        }

        public int VencimentoDocumento(int codigo)
        {
            DALDocumentos DALobj = new DALDocumentos(conexao);
            return DALobj.VencimentoDocumento(codigo);
        }

        public ModeloDocumentos CarregaDocumentos(int codigo)
        {
            DALDocumentos DALobj = new DALDocumentos(conexao);
            return DALobj.CarregaDocumentos(codigo);
        }
        public int TotalDocumentos()
        {
            DALDocumentos DALobj = new DALDocumentos(conexao);
            return DALobj.TotalDocumentos();
        }
    }
}
