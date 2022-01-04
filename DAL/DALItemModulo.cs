using System;
using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    public class DALItemModulo
    {
        private DALConexao conexao;
        public DALItemModulo(DALConexao cx)
        {
            this.conexao = cx;
        }
        public DataTable Localizar(int idmodulo, int idusuarios)
        {
            DataTable tabela = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter("select im.iditens_modulos, im.item, (select 1 from itens_modulos_usuarios imu where imu.idusuarios=" + idusuarios + " and imu.iditens_modulos=im.iditens_modulos) as ch from itens_modulos im where im.idmodulos=" + idmodulo + " order by im.item", conexao.StringConexao);
            da.Fill(tabela);
            return tabela;
        }

        public bool LocalizarItemModuloUsuario(int iditensmodulos, int idusuarios)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conexao.ObjetoConexao;
            cmd.CommandText = "select * from itens_modulos_usuarios where iditens_modulos=" + iditensmodulos.ToString() + " and idusuarios=" + idusuarios.ToString();
            conexao.Conectar();
            SqlDataReader registro = cmd.ExecuteReader();
            bool resultado = false;
            if (registro.HasRows)
            {
                resultado = true;
            }
            conexao.Desconectar();
            return resultado;
        }

        public bool LocalizarModuloUsuario(int idmodulos, int idusuarios)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conexao.ObjetoConexao;
            cmd.CommandText = "select imu.* from itens_modulos_usuarios imu join itens_modulos im on im.iditens_modulos=imu.iditens_modulos where idmodulos=" + idmodulos.ToString() + " and idusuarios=" + idusuarios.ToString();
            conexao.Conectar();
            SqlDataReader registro = cmd.ExecuteReader();
            bool resultado = false;
            if (registro.HasRows)
            {
                resultado = true;
            }
            conexao.Desconectar();
            return resultado;
        }

        public int LocalizarModulo(int iditensmodulos)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conexao.ObjetoConexao;
            cmd.CommandText = "select idmodulos from itens_modulos where iditens_modulos=" + iditensmodulos.ToString();
            conexao.Conectar();
            SqlDataReader registro = cmd.ExecuteReader();
            int resultado = 0;
            if (registro.HasRows)
            {
                registro.Read();
                resultado = Convert.ToInt32(registro["idmodulos"]);
            }
            conexao.Desconectar();
            return resultado;
        }

        public DataTable CarregarItensModulosUsuario(int idusuarios, int idmodulos)
        {
            DataTable tabela = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter("select im.iditens_modulos,im.item,im.controller,im.action from itens_modulos im join itens_modulos_usuarios imu on imu.iditens_modulos=im.iditens_modulos where imu.idusuarios=" + idusuarios + " and im.idmodulos=" + idmodulos + " order by ordem asc", DadosConexao.StringConexao);
            da.Fill(tabela);
            return tabela;
        }
    }
}
