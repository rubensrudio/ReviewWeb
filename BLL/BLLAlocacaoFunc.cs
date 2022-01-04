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
    public class BLLAlocacaoFunc
    {
        private DALConexao conexao;
        public BLLAlocacaoFunc(DALConexao cx)
        {
            this.conexao = cx;
        }
        public void Alterar(ModeloAlocacaoFunc modelo)
        {
            DALAlocacaoFunc DALobj1 = new DALAlocacaoFunc(conexao);
            DALobj1.Alterar(modelo);
        }
        public void Excluir(int codigo)
        {
            DALAlocacaoFunc DALobj = new DALAlocacaoFunc(conexao);
            DALobj.Excluir(codigo);
        }
        public DataTable Localizar(String valor, String buscapor, int idempresas, int pageNumber, int RowsPage, string ordenapor)
        {
            DALAlocacaoFunc DALobj = new DALAlocacaoFunc(conexao);
            return DALobj.Localizar(valor, buscapor, idempresas, pageNumber, RowsPage, ordenapor);
        }

        public DataTable UltimaFolga(int idfuncionarios)
        {
            DALAlocacaoFunc DALobj = new DALAlocacaoFunc(conexao);
            return DALobj.UltimaFolga(idfuncionarios);
        }

        public DataTable UltimaLocal(int idfuncionarios)
        {
            DALAlocacaoFunc DALobj = new DALAlocacaoFunc(conexao);
            return DALobj.UltimaLocal(idfuncionarios);
        }

        public ModeloAlocacaoFunc CarregaAlocacaoFunc(int codigo)
        {
            DALAlocacaoFunc DALobj = new DALAlocacaoFunc(conexao);
            return DALobj.CarregaAlocacaoFunc(codigo);
        }

        public int TotalAlocacaoFunc(String valor, String buscapor, int idempresas)
        {
            DALAlocacaoFunc DALobj = new DALAlocacaoFunc(conexao);
            return DALobj.TotalAlocacaoFunc(valor, buscapor, idempresas);
        }
    }
}
