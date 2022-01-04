using DAL;
using Modelo;
using System;
using System.Data;

namespace BLL
{
    public class BLLEmpresas
    {
        private DALConexao conexao;
        public BLLEmpresas(DALConexao cx)
        {
            this.conexao = cx;
        }
        public void Incluir(ModeloEmpresa modelo)
        {
            DALEmpresa DALobj1 = new DALEmpresa(conexao);
            
            DALEndereco DALobj = new DALEndereco(conexao);
            DALobj.Incluir(modelo.Endereco);

            DALobj1.Incluir(modelo, modelo.Endereco);
        }
        public void Alterar(ModeloEmpresa modelo)
        {
            DALEmpresa DALobj1 = new DALEmpresa(conexao);
            DALEndereco DALobj = new DALEndereco(conexao);
            DALobj.Alterar(modelo.Endereco);
            
            DALobj1.Alterar(modelo);
        }
        public void Excluir(int codigo)
        {
            DALEmpresa DALobj = new DALEmpresa(conexao);
            DALobj.Excluir(codigo);
        }
        public DataTable Localizar(String valor, String buscapor)
        {
            DALEmpresa DALobj = new DALEmpresa(conexao);
            return DALobj.Localizar(valor, buscapor);
        }

        public DataTable Localizar(String valor, String buscapor, int pageNumber, int RowsPage, string ordenapor)
        {
            DALEmpresa DALobj = new DALEmpresa(conexao);
            return DALobj.Localizar(valor, buscapor, pageNumber, RowsPage, ordenapor);
        }

        public int LocalizarEnd(int codigo)
        {
            DALEmpresa DALobj = new DALEmpresa(conexao);
            return DALobj.LocalizarEnd(codigo);
        }
        public ModeloEmpresa CarregaEmpresa(int codigo)
        {
            DALEmpresa DALobj = new DALEmpresa(conexao);
            return DALobj.CarregaEmpresa(codigo);
        }
        public ModeloEndereco CarregaEndereco(int codigo)
        {
            DALEndereco DALobj = new DALEndereco(conexao);
            return DALobj.CarregaEndereco(codigo);
        }

        public int VerificaCNPJ(String valor, int codigo)
        {
            DALEmpresa DALobj = new DALEmpresa(conexao);
            return DALobj.VerificaCNPJ(valor,codigo);
        }

        public int TotalEmpresas()
        {
            DALEmpresa DALobj = new DALEmpresa(conexao);
            return DALobj.TotalEmpresas();
        }
    }
}
