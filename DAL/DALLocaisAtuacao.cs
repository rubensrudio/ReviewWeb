using Modelo;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class DALLocaisAtuacao
    {
        private DALConexao conexao;
        public DALLocaisAtuacao(DALConexao cx)
        {
            this.conexao = cx;
        }

        public void Incluir(ModeloLocaisAtuacao modelo)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conexao.ObjetoConexao;
            cmd.CommandText = "insert into clientes_localidades (idclientes, cidade, uf) " +
                "values (@idclientes, @cidade, @uf); select @@IDENTITY;";
            cmd.Parameters.AddWithValue("@idclientes", modelo.IdClientes);
            cmd.Parameters.AddWithValue("@cidade", modelo.Cidade);
            cmd.Parameters.AddWithValue("@uf", modelo.UF);
            
            conexao.Conectar();
            modelo.IdClientes_Localidades = Convert.ToInt32(cmd.ExecuteScalar());
            conexao.Desconectar();
        }

        public void Excluir(int codigo)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conexao.ObjetoConexao;
            cmd.CommandText = "delete from contratos where idcontratos_clientes_localidades=@idclientes_localidades;";
            cmd.Parameters.AddWithValue("@idclientes_localidades", codigo);

            conexao.Conectar();
            cmd.ExecuteNonQuery();
            conexao.Desconectar();

            SqlCommand cmd2 = new SqlCommand();
            cmd2.Connection = conexao.ObjetoConexao;
            cmd2.CommandText = "delete from clientes_localidades where idclientes_localidades=@idclientes_localidades;";
            cmd2.Parameters.AddWithValue("@idclientes_localidades", codigo);

            conexao.Conectar();
            cmd2.ExecuteNonQuery();
            conexao.Desconectar();
        }

        public DataTable CarregarLocaisAtuacao(int idclientes)
        {
            DataTable tabela = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter("select idclientes_localidades, idclientes, cidade, uf from clientes_localidades where idclientes=" + idclientes + " order by cidade", conexao.StringConexao);
            da.Fill(tabela);
            return tabela;
        }

        public ModeloLocaisAtuacao LocalizarLocalAtuacao(int codigo)
        {
            ModeloLocaisAtuacao modelo = new ModeloLocaisAtuacao();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conexao.ObjetoConexao;
            cmd.CommandText = "select idclientes_localidades,idclientes,cidade,uf from clientes_localidades where idclientes_localidades=" + codigo.ToString();
            conexao.Conectar();
            SqlDataReader registro = cmd.ExecuteReader();
            if (registro.HasRows)
            {
                registro.Read();
                modelo.IdClientes_Localidades = Convert.ToInt32(registro["idclientes_localidades"]);
                modelo.IdClientes = Convert.ToInt32(registro["idclientes"]);
                modelo.Cidade = registro["idclientes_localidades"].ToString();
                modelo.UF = registro["uf"].ToString();
            }
            conexao.Desconectar();
            return modelo;
        }

        public DataTable CarregarUF(int idempresas, int idusuarios)
        {
            DataTable tabela = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter("select cl.uf, count(ct.idcontratos) as quant_contratos, (select top 1 '1' from alocacao_permissao ap where ap.permissao like cl.uf and ap.idusuarios="+idusuarios+ ") as perm from clientes_localidades cl join clientes c on c.idclientes = cl.idclientes left join contratos ct on ct.idcontratos_clientes_localidades=cl.idclientes_localidades where c.idempresas = " + idempresas + " group by cl.uf order by cl.uf", conexao.StringConexao);
            da.Fill(tabela);
            return tabela;
        }

        public DataTable CarregarClientes(int idempresas)
        {
            DataTable tabela = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter("select c.nome_fantasia, cl.idclientes_localidades, cl.uf, cl.idclientes, count(ct.idcontratos) as quant_contratos from clientes c left join clientes_localidades cl on cl.idclientes = c.idclientes left join contratos ct on ct.idcontratos_clientes_localidades=cl.idclientes_localidades where c.idempresas = " + idempresas + "  and cl.uf is not null group by c.nome_fantasia, cl.idclientes_localidades, cl.uf, cl.idclientes order by c.nome_fantasia, cl.uf", conexao.StringConexao);
            da.Fill(tabela);
            return tabela;
        }
    }
}
