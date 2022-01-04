using DAL;
using Modelo;
using System;
using System.Data;

namespace BLL
{
    public class BLLModulos
    {
        private DALConexao conexao;
        public BLLModulos(DALConexao cx)
        {
            this.conexao = cx;
        }
        public void Incluir(ModeloModulo modelo)
        {

            if (modelo.Modulo.Trim().Length == 0)
            {
                throw new Exception("Módulo é obrigatório!");
            }
           
            DALModulo DALobj = new DALModulo(conexao);
            DALobj.Incluir(modelo);
        }
        public void Alterar(ModeloModulo modelo)
        {
            if (modelo.IdModulos <= 0)
            {
                throw new Exception("Código é obrigatório!");
            }
            if (modelo.Modulo.Trim().Length == 0)
            {
                throw new Exception("Módulo é obrigatório!");
            }
            
            DALModulo DALobj = new DALModulo(conexao);
            DALobj.Alterar(modelo);
        }
        public void Excluir(int codigo)
        {
            DALModulo DALobj = new DALModulo(conexao);
            DALobj.Excluir(codigo);
        }
        public DataTable Localizar(int idempresas, int idusuarios)
        {
            DALModulo DALobj = new DALModulo(conexao);
            return DALobj.Localizar(idempresas, idusuarios);
        }

        public bool LocalizarModuloUsuario(int idmodulos, int idusuarios)
        {
            DALModulo DALobj = new DALModulo(conexao);
            return DALobj.LocalizarModuloUsuario(idmodulos, idusuarios);
        }

        public DataTable CarregarModulosUsuario(int idusuarios)
        {
            DALModulo DALobj = new DALModulo(conexao);
            return DALobj.CarregarModulosUsuario(idusuarios);
        }
    }
}
