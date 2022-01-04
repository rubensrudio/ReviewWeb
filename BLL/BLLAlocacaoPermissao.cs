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
    public class BLLAlocacaoPermissao
    {
        private DALConexao conexao;
        public BLLAlocacaoPermissao(DALConexao cx)
        {
            this.conexao = cx;
        }
        public void Incluir(ModeloAlocacaoPermissao modelo)
        {
            DALAlocacaoPermissao DALobj1 = new DALAlocacaoPermissao(conexao);
            DALobj1.Incluir(modelo);
        }
        public void Alterar(ModeloAlocacaoPermissao modelo)
        {
            DALAlocacaoPermissao DALobj1 = new DALAlocacaoPermissao(conexao);
            DALobj1.Alterar(modelo);
        }
        public void Excluir(int codigo)
        {
            DALAlocacaoPermissao DALobj = new DALAlocacaoPermissao(conexao);
            DALobj.Excluir(codigo);
        }
        public DataTable Localizar(String valor, String buscapor, int idempresas, int pageNumber, int RowsPage, string ordenapor)
        {
            DALAlocacaoPermissao DALobj = new DALAlocacaoPermissao(conexao);
            return DALobj.Localizar(valor, buscapor, idempresas, pageNumber, RowsPage, ordenapor);
        }

        public ModeloAlocacaoPermissao CarregaPermissoes(int codigo)
        {
            DALAlocacaoPermissao DALobj = new DALAlocacaoPermissao(conexao);
            return DALobj.CarregaPermissoes(codigo);
        }

        public int TotalPermissao()
        {
            DALAlocacaoPermissao DALobj = new DALAlocacaoPermissao(conexao);
            return DALobj.TotalPermissoes();
        }
    }
}
