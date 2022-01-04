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
    public class BLLUsuarios
    {
        private DALConexao conexao;
        public BLLUsuarios(DALConexao cx)
        {
            this.conexao = cx;
        }
        public void Incluir(ModeloUsuario modelo)
        {
            if (modelo.Usuario.Trim().Length == 0)
            {
                throw new Exception("Usuário é obrigatório!");
            }
            if (modelo.Senha.Trim().Length == 0)
            {
                throw new Exception("Senha é obrigatória!");
            }
            if (modelo.Nome.Trim().Length == 0)
            {
                throw new Exception("Nome é obrigatório!");
            }

            DALUsuario DALobj = new DALUsuario(conexao);
            DALobj.Incluir(modelo);
        }
        public void Alterar(ModeloUsuario modelo)
        {
            if (modelo.IdUsuarios <= 0)
            {
                throw new Exception("Código é obrigatório!");
            }
            if (modelo.Usuario.Trim().Length == 0)
            {
                throw new Exception("Usuário é obrigatório!");
            }
            if (modelo.Senha.Trim().Length == 0)
            {
                throw new Exception("Senha é obrigatória!");
            }
            if (modelo.Nome.Trim().Length == 0)
            {
                throw new Exception("Nome é obrigatório!");
            }

            DALUsuario DALobj = new DALUsuario(conexao);
            DALobj.Alterar(modelo);
        }
        public void Excluir(int codigo)
        {
            DALUsuario DALobj = new DALUsuario(conexao);
            DALobj.Excluir(codigo);
        }
        public DataTable Localizar(String valor, String buscapor, int idempresa)
        {
            DALUsuario DALobj = new DALUsuario(conexao);
            return DALobj.Localizar(valor, buscapor, idempresa);
        }

        public DataTable Localizar(int idempresa, int idcentrosdecusto)
        {
            DALUsuario DALobj = new DALUsuario(conexao);
            return DALobj.Localizar(idempresa,idcentrosdecusto);
        }

        public int TotalUsuarios()
        {
            DALUsuario DALobj = new DALUsuario(conexao);
            return DALobj.TotalUsuarios();
        }

        public DataTable Localizar(String valor, String buscapor, int idempresas, int pageNumber, int RowsPage, string ordenapor)
        {
            DALUsuario DALobj = new DALUsuario(conexao);
            return DALobj.Localizar(valor, buscapor, idempresas,pageNumber,RowsPage,ordenapor);
        }

        public ModeloUsuario CarregaUsuario(int codigo)
        {
            DALUsuario DALobj = new DALUsuario(conexao);
            return DALobj.CarregaUsuario(codigo);
        }
        
    }
}
