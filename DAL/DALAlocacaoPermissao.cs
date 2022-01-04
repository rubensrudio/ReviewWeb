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
    public class DALAlocacaoPermissao
    {
        private DALConexao conexao;
        public DALAlocacaoPermissao(DALConexao cx)
        {
            this.conexao = cx;
        }

        public void Incluir(ModeloAlocacaoPermissao modelo)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conexao.ObjetoConexao;
            cmd.CommandText = "insert into alocacao_permissao (idusuarios,permissao) " +
                "values (@idusuarios,@permissao); select @@IDENTITY;";
            cmd.Parameters.AddWithValue("@idusuarios", ConverteReader.ConverteInt(modelo.IdUsuarios));
            cmd.Parameters.AddWithValue("@permissao", ConverteReader.ConverteString(modelo.Permissao));
            
            conexao.Conectar();
            modelo.IdAlocacaoPermissao = Convert.ToInt32(cmd.ExecuteScalar());
            conexao.Desconectar();
        }

        public void Alterar(ModeloAlocacaoPermissao modelo)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conexao.ObjetoConexao;
            cmd.CommandText = "update alocacao_permissao set idusuarios=@idusuarios,permissao=@permissao " +
                "where idalocacao_permissao=@idalocacao_permissao;";
            cmd.Parameters.AddWithValue("@idalocacao_permissao", ConverteReader.ConverteInt(modelo.IdAlocacaoPermissao));
            cmd.Parameters.AddWithValue("@idusuarios", ConverteReader.ConverteInt(modelo.IdUsuarios));
            cmd.Parameters.AddWithValue("@permissao", ConverteReader.ConverteString(modelo.Permissao));
            
            conexao.Conectar();
            cmd.ExecuteNonQuery();
            conexao.Desconectar();
        }

        public void Excluir(int codigo)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conexao.ObjetoConexao;
            cmd.CommandText = "delete from alocacao_permissao where idalocacao_permissao=@idalocacao_permissao;";
            cmd.Parameters.AddWithValue("@idalocacao_permissao", codigo);

            conexao.Conectar();
            cmd.ExecuteNonQuery();
            conexao.Desconectar();
        }

        public DataTable Localizar(String valor, String buscapor, int idempresas, int pageNumber, int RowsPage, string ordenapor)
        {
            String where = "f.nome";
            String where2 = "nome";
            
            String order = "nome";
            
            DataTable tabela = new DataTable();

            string sql = "SELECT * FROM ( " +
                            "SELECT ROW_NUMBER() OVER(ORDER BY " + where + ") as number, e.idalocacao_permissao,f.nome,e.permissao " +
                            "from alocacao_permissao e join usuarios f on f.idusuarios=e.idusuarios where " + where + " like '%" + valor + "%'" +
                            ") as tbl " +
                          "where " + where2 + " like '%" + valor + "%' and number between((" + pageNumber + " - 1) * " + RowsPage + " + 1) and(" + pageNumber + " * " + RowsPage + ") " +
                          "order by " + order;
            SqlDataAdapter da = new SqlDataAdapter(sql, conexao.StringConexao);
            da.Fill(tabela);
            return tabela;
        }

        public int TotalPermissoes()
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conexao.ObjetoConexao;
            cmd.CommandText = "select count(idalocacao_permissao) as quant from alocacao_permissao";
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

        public ModeloAlocacaoPermissao CarregaPermissoes(int codigo)
        {
            ModeloAlocacaoPermissao modelo = new ModeloAlocacaoPermissao();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conexao.ObjetoConexao;
            cmd.CommandText = "select * from alocacao_permissao where idalocacao_permissao=" + codigo.ToString();
            conexao.Conectar();
            SqlDataReader registro = cmd.ExecuteReader();

            if (registro.HasRows)
            {
                registro.Read();
                modelo.IdAlocacaoPermissao = ConverteReader.ConverteInt(registro["idalocacao_permissao"]);
                modelo.IdUsuarios = ConverteReader.ConverteInt(registro["idusuarios"]);
                modelo.Permissao = ConverteReader.ConverteString(registro["permissao"]);

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
