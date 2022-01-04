using DAL;
using Modelo;

namespace BLL
{
    public class BLLLogin
    {
        private DALConexao conexao;
        public BLLLogin(DALConexao cx)
        {
            this.conexao = cx;
        }
       
        public ModeloUsuario Login(string usuario, string senha)
        {
            DALUsuario DALobj = new DALUsuario(conexao);
            return DALobj.CarregaUsuario(usuario, senha);
        }
    }
}
