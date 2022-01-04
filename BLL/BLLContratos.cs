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
    public class BLLContratos
    {
        private DALConexao conexao;
        public BLLContratos(DALConexao cx)
        {
            this.conexao = cx;
        }
        public void Incluir(ModeloContratos modelo)
        {
            DALContratos DALobj1 = new DALContratos(conexao);
            DALobj1.Incluir(modelo);
        }
        public void Alterar(ModeloContratos modelo)
        {
            DALContratos DALobj1 = new DALContratos(conexao);
            DALobj1.Alterar(modelo);
        }
        public void Excluir(int codigo)
        {
            DALContratos DALobj = new DALContratos(conexao);
            DALobj.Excluir(codigo);
        }
        public DataTable Localizar(String valor, String buscapor)
        {
            DALContratos DALobj = new DALContratos(conexao);
            return DALobj.Localizar(valor, buscapor);
        }

        public DataTable LocalizarContratos(int idempresas, int idusuarios)
        {
            DALContratos DALobj = new DALContratos(conexao);
            return DALobj.LocalizarContratos(idempresas, idusuarios);
        }

        public DataTable Localizar(String valor, String buscapor, int idempresas, int pageNumber, int RowsPage, string ordenapor)
        {
            DALContratos DALobj = new DALContratos(conexao);
            return DALobj.Localizar(valor, buscapor, idempresas, pageNumber, RowsPage, ordenapor);
        }

        public ModeloContratos CarregaContratos(int codigo)
        {
            DALContratos DALobj = new DALContratos(conexao);
            return DALobj.CarregaContratos(codigo);
        }
        public int TotalContratos(int idempresas)
        {
            DALContratos DALobj = new DALContratos(conexao);
            return DALobj.TotalContratos(idempresas);
        }

        public int VerificaFuncionarios(int idcontratos, int idfuncionarios)
        {
            DALContratos DALobj = new DALContratos(conexao);
            return DALobj.VerificaFuncionarios(idcontratos, idfuncionarios);
        }

        public DataTable CarregaContratosEmpresa(int codigo)
        {
            DALContratos DALobj = new DALContratos(conexao);
            return DALobj.CarregaContratosEmpresa(codigo);
        }

        public DataTable CarregaContratosCliente(int codigo)
        {
            DALContratos DALobj = new DALContratos(conexao);
            return DALobj.CarregaContratosCliente(codigo);
        }

        public DataTable CarregaContratosEmpresa(int codigo, string dataInicial, string dataFinal)
        {
            DALContratos DALobj = new DALContratos(conexao);
            return DALobj.CarregaContratosEmpresa(codigo, dataInicial, dataFinal);
        }

        

        public DataTable CarregaFuncionarios(int idempresas)
        {
            DALContratos DALobj = new DALContratos(conexao);
            return DALobj.CarregaFuncionarios(idempresas);
        }
    }
}
