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
    public class BLLCliente
    {
        private DALConexao conexao;
        public BLLCliente(DALConexao cx)
        {
            this.conexao = cx;
        }
        public void Incluir(ModeloCliente modelo)
        {
            DALCliente DALobj1 = new DALCliente(conexao);

            DALEndereco DALobj = new DALEndereco(conexao);
            DALobj.Incluir(modelo.Endereco);

            DALobj1.Incluir(modelo, modelo.Endereco);
        }
        public void Alterar(ModeloCliente modelo)
        {
            DALCliente DALobj1 = new DALCliente(conexao);
            DALEndereco DALobj = new DALEndereco(conexao);
            DALobj.Alterar(modelo.Endereco);

            DALobj1.Alterar(modelo);
        }
        public void Excluir(int codigo)
        {
            DALCliente DALobj = new DALCliente(conexao);
            DALobj.Excluir(codigo);
        }
        public DataTable Localizar(String valor, String buscapor, int idempresas)
        {
            DALCliente DALobj = new DALCliente(conexao);
            return DALobj.Localizar(valor, buscapor, idempresas);
        }

        public DataTable Localizar(String valor, String buscapor, int idempresas, String clifor, int pageNumber, int RowsPage, string ordenapor)
        {
            DALCliente DALobj = new DALCliente(conexao);
            return DALobj.Localizar(valor, buscapor, idempresas, clifor, pageNumber, RowsPage, ordenapor);
        }

        public int LocalizarEnd(int codigo)
        {
            DALCliente DALobj = new DALCliente(conexao);
            return DALobj.LocalizarEnd(codigo);
        }
        public ModeloCliente CarregaCliente(int codigo)
        {
            DALCliente DALobj = new DALCliente(conexao);
            return DALobj.CarregaCliente(codigo);
        }
        public ModeloEndereco CarregaEndereco(int codigo)
        {
            DALEndereco DALobj = new DALEndereco(conexao);
            return DALobj.CarregaEndereco(codigo);
        }

        public int VerificaCNPJ(String valor, int codigo)
        {
            DALCliente DALobj = new DALCliente(conexao);
            return DALobj.VerificaCNPJ(valor, codigo);
        }

        public int TotalClientes()
        {
            DALCliente DALobj = new DALCliente(conexao);
            return DALobj.TotalClientes();
        }
    }
}