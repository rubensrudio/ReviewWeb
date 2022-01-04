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
    public class DALCargos
    {
        private DALConexao conexao;
        public DALCargos(DALConexao cx)
        {
            this.conexao = cx;
        }

        public void Incluir(ModeloCargos modelo)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conexao.ObjetoConexao;
            cmd.CommandText = "insert into cargos (idempresas,descricao,salario_medio) " +
                "values (@idempresas,@descricao,@salario_medio); select @@IDENTITY;";
            cmd.Parameters.AddWithValue("@idempresas", ConverteReader.ConverteInt(modelo.IdEmpresas));
            cmd.Parameters.AddWithValue("@descricao", ConverteReader.ConverteString(modelo.Descricao));
            cmd.Parameters.AddWithValue("@salario_medio", ConverteReader.ConverteDouble(modelo.Salario_Medio));
            
            conexao.Conectar();
            modelo.IdCargos = Convert.ToInt32(cmd.ExecuteScalar());
            conexao.Desconectar();
        }

        public void Alterar(ModeloCargos modelo)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conexao.ObjetoConexao;
            cmd.CommandText = "update cargos set idempresas=@idempresas,descricao=@descricao,salario_medio=@salario_medio " +
                "where idcargos=@idcargos;";
            cmd.Parameters.AddWithValue("@idcargos", ConverteReader.ConverteInt(modelo.IdCargos));
            cmd.Parameters.AddWithValue("@idempresas", ConverteReader.ConverteInt(modelo.IdEmpresas));
            cmd.Parameters.AddWithValue("@descricao", ConverteReader.ConverteString(modelo.Descricao));
            cmd.Parameters.AddWithValue("@salario_medio", ConverteReader.ConverteDouble(modelo.Salario_Medio));
            
            conexao.Conectar();
            cmd.ExecuteNonQuery();
            conexao.Desconectar();
        }

        public void Excluir(int codigo)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conexao.ObjetoConexao;
            cmd.CommandText = "delete from cargos where idcargos=@idcargos;";
            cmd.Parameters.AddWithValue("@idcargos", codigo);

            conexao.Conectar();
            cmd.ExecuteNonQuery();
            conexao.Desconectar();
        }

        public DataTable Localizar(String valor, String buscapor)
        {
            String where = "descricao";
            
            DataTable tabela = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter("select idcargos,descricao from cargos where " + where + " like '%" + valor + "%' order by " + where, conexao.StringConexao);
            da.Fill(tabela);
            return tabela;
        }

        public DataTable Localizar(String valor, String buscapor, int idempresas, int pageNumber, int RowsPage, string ordenapor)
        {
            String where = "descricao";
            String where2 = "descricao";
            
            //String order = "descricao";
            String order2 = "descricao";
           
            DataTable tabela = new DataTable();

            string sql = "SELECT * FROM ( " +
                            "SELECT ROW_NUMBER() OVER(ORDER BY " + where + ") as number, idcargos,descricao " +
                            "from cargos where " + where + " like '%" + valor + "%'" +
                            ") as tbl " +
                          "where " + where2 + " like '%" + valor + "%' and number between((" + pageNumber + " - 1) * " + RowsPage + " + 1) and(" + pageNumber + " * " + RowsPage + ") " +
                          "order by " + order2;
            SqlDataAdapter da = new SqlDataAdapter(sql, conexao.StringConexao);
            da.Fill(tabela);
            return tabela;
        }

        public int TotalCargos()
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conexao.ObjetoConexao;
            cmd.CommandText = "select count(idcargos) as quant from cargos";
            conexao.Conectar();
            SqlDataReader registro = cmd.ExecuteReader();
            int quant = 0;
            if (registro.HasRows)
            {
                registro.Read();
                quant = Convert.ToInt32(registro["quant"]);
            }
            conexao.Desconectar();
            return quant;
        }

        
        public ModeloCargos CarregaCargos(int codigo)
        {
            ModeloCargos modelo = new ModeloCargos();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conexao.ObjetoConexao;
            cmd.CommandText = "select * from cargos where idcargos=" + codigo.ToString();
            conexao.Conectar();
            SqlDataReader registro = cmd.ExecuteReader();

            if (registro.HasRows)
            {
                registro.Read();
                modelo.IdCargos = ConverteReader.ConverteInt(registro["idcargos"]);
                modelo.IdEmpresas = ConverteReader.ConverteInt(registro["idempresas"]);
                modelo.Descricao = ConverteReader.ConverteString(registro["descricao"]);
                modelo.Salario_Medio = ConverteReader.ConverteDouble(registro["salario_medio"]);

                conexao.Desconectar();
            }
            else
            {
                conexao.Desconectar();
            }

            return modelo;
        }
    }
}
