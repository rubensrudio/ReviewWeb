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
    public class BLLFuncionarios
    {
        private DALConexao conexao;
        public BLLFuncionarios(DALConexao cx)
        {
            this.conexao = cx;
        }
        public void Incluir(ModeloFuncionarios modelo)
        {
            DALFuncionarios DALobj1 = new DALFuncionarios(conexao);

            DALEndereco DALobj = new DALEndereco(conexao);
            DALobj.Incluir(modelo.Endereco);

            DALobj1.Incluir(modelo, modelo.Endereco);
        }
        public void Alterar(ModeloFuncionarios modelo)
        {
            DALFuncionarios DALobj1 = new DALFuncionarios(conexao);
            DALEndereco DALobj = new DALEndereco(conexao);
            DALobj.Alterar(modelo.Endereco);

            DALobj1.Alterar(modelo);
        }
        public void Excluir(int codigo)
        {
            DALFuncionarios DALobj = new DALFuncionarios(conexao);
            DALobj.Excluir(codigo);
        }
        public DataTable Localizar(String valor, String buscapor)
        {
            DALFuncionarios DALobj = new DALFuncionarios(conexao);
            return DALobj.Localizar(valor, buscapor);
        }

        public DataTable LocalizarFuncionarios(int idempresas)
        {
            DALFuncionarios DALobj = new DALFuncionarios(conexao);
            return DALobj.LocalizarFuncionarios(idempresas);
        }

        public DataTable Localizar(String valor, String buscapor, int idempresas, int pageNumber, int RowsPage, string ordenapor)
        {
            DALFuncionarios DALobj = new DALFuncionarios(conexao);
            return DALobj.Localizar(valor, buscapor, idempresas, pageNumber, RowsPage, ordenapor);
        }

        public int LocalizarEnd(int codigo)
        {
            DALFuncionarios DALobj = new DALFuncionarios(conexao);
            return DALobj.LocalizarEnd(codigo);
        }
        public ModeloFuncionarios CarregaFuncionarios(int codigo)
        {
            DALFuncionarios DALobj = new DALFuncionarios(conexao);
            return DALobj.CarregaFuncionarios(codigo);
        }
        public ModeloEndereco CarregaEndereco(int codigo)
        {
            DALEndereco DALobj = new DALEndereco(conexao);
            return DALobj.CarregaEndereco(codigo);
        }

        public int VerificaCPF(String valor, int codigo)
        {
            DALFuncionarios DALobj = new DALFuncionarios(conexao);
            return DALobj.VerificaCPF(valor, codigo);
        }

        public int TotalFuncionarios()
        {
            DALFuncionarios DALobj = new DALFuncionarios(conexao);
            return DALobj.TotalFuncionarios();
        }

        public DataTable CarregaAlocacaoFuncionarios(string inicio, string fim)
        {
            DALFuncionarios DALobj = new DALFuncionarios(conexao);
            return DALobj.CarregaAlocacaoFuncionarios(inicio, fim);
        }
    }
}
