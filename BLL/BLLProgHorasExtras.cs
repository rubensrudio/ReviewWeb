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
    public class BLLProgHorasExtras
    {
        private DALConexao conexao;
        public BLLProgHorasExtras(DALConexao cx)
        {
            this.conexao = cx;
        }
        public void Incluir(ModeloProgHorasExtras modelo)
        {
            DALProgHorasExtras DALobj1 = new DALProgHorasExtras(conexao);
            DALobj1.Incluir(modelo);
        }

        public void IncluirItens(int item, ModeloProgHorasExtras modelo)
        {
            DALProgHorasExtras DALobj1 = new DALProgHorasExtras(conexao);
            DALobj1.IncluirItens(item, modelo);
        }
        public void Alterar(ModeloProgHorasExtras modelo)
        {
            DALProgHorasExtras DALobj1 = new DALProgHorasExtras(conexao);
            DALobj1.Alterar(modelo);
        }
        public void Excluir(int codigo)
        {
            DALProgHorasExtras DALobj = new DALProgHorasExtras(conexao);
            DALobj.Excluir(codigo);
        }
        public DataTable Localizar(String valor, String buscapor, int idempresas, int idusuarios, int pageNumber, int RowsPage, string ordenapor)
        {
            DALProgHorasExtras DALobj = new DALProgHorasExtras(conexao);
            return DALobj.Localizar(valor, buscapor, idempresas, idusuarios, pageNumber, RowsPage, ordenapor);
        }

        public ModeloProgHorasExtras CarregaProgHorasExtras(int codigo)
        {
            DALProgHorasExtras DALobj = new DALProgHorasExtras(conexao);
            return DALobj.CarregaProgHorasExtras(codigo);
        }

        public int TotalProgramas()
        {
            DALProgHorasExtras DALobj = new DALProgHorasExtras(conexao);
            return DALobj.TotalProgramas();
        }
    }
}
