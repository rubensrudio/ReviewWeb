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
    public class BLLLocaisAtuacao
    {
        private DALConexao conexao;

        public BLLLocaisAtuacao(DALConexao cx)
        {
            this.conexao = cx;
        }

        public void Incluir(ModeloLocaisAtuacao modelo)
        {
            DALLocaisAtuacao DALobj1 = new DALLocaisAtuacao(conexao);
            DALobj1.Incluir(modelo);
        }

        public void Excluir(int codigo)
        {
            DALLocaisAtuacao DALobj = new DALLocaisAtuacao(conexao);
            DALobj.Excluir(codigo);
        }

        public DataTable CarregarLocaisAtuacao(int idclientes)
        {
            DALLocaisAtuacao DALobj = new DALLocaisAtuacao(conexao);
            return DALobj.CarregarLocaisAtuacao(idclientes);
        }

        public ModeloLocaisAtuacao LocalizarLocalAtuacao(int codigo)
        {
            DALLocaisAtuacao DALobj = new DALLocaisAtuacao(conexao);
            return DALobj.LocalizarLocalAtuacao(codigo);
        }

        public DataTable CarregarUF(int idempresas, int idusuarios)
        {
            DALLocaisAtuacao DALobj = new DALLocaisAtuacao(conexao);
            return DALobj.CarregarUF(idempresas,idusuarios);
        }

        public DataTable CarregarClientes(int idempresas)
        {
            DALLocaisAtuacao DALobj = new DALLocaisAtuacao(conexao);
            return DALobj.CarregarClientes(idempresas);
        }
    }
}
