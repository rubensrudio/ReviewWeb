using Modelo;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class DALDocumentos
    {
        private DALConexao conexao;
        public DALDocumentos(DALConexao cx)
        {
            this.conexao = cx;
        }

        public void Incluir(ModeloDocumentos modelo)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conexao.ObjetoConexao;
            cmd.CommandText = "insert into documentos (idempresas,titulo,descricao,dt_vencimento) " +
                "values (@idempresas,@titulo,@descricao,@dt_vencimento); select @@IDENTITY;";
            cmd.Parameters.AddWithValue("@idempresas", ConverteReader.ConverteInt(modelo.IdEmpresas));
            cmd.Parameters.AddWithValue("@titulo", ConverteReader.ConverteString(modelo.Titulo));
            cmd.Parameters.AddWithValue("@descricao", ConverteReader.ConverteString(modelo.Descricao));
            if ((modelo.Dt_Vencimento != null) && (modelo.Dt_Vencimento.ToString() != "01/01/0001 00:00:00"))
            {
                cmd.Parameters.Add("@dt_vencimento", System.Data.SqlDbType.DateTime);
                cmd.Parameters["@dt_vencimento"].Value = modelo.Dt_Vencimento;
            }
            else
            {
                cmd.Parameters.Add("@dt_vencimento", System.Data.SqlDbType.DateTime);
                cmd.Parameters["@dt_vencimento"].Value = SqlDateTime.Null;
            }

            conexao.Conectar();
            modelo.IdDocumentos = Convert.ToInt32(cmd.ExecuteScalar());
            conexao.Desconectar();
        }

        public void Alterar(ModeloDocumentos modelo)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conexao.ObjetoConexao;
            cmd.CommandText = "update documentos set titulo=@titulo,descricao=@descricao,dt_vencimento=@dt_vencimento " +
                "where iddocumentos=@iddocumentos;";
            cmd.Parameters.AddWithValue("@iddocumentos", ConverteReader.ConverteInt(modelo.IdDocumentos));
            cmd.Parameters.AddWithValue("@titulo", ConverteReader.ConverteString(modelo.Titulo));
            cmd.Parameters.AddWithValue("@descricao", ConverteReader.ConverteString(modelo.Descricao));
            if ((modelo.Dt_Vencimento != null) && (modelo.Dt_Vencimento.ToString() != "01/01/0001 00:00:00"))
            {
                cmd.Parameters.Add("@dt_vencimento", System.Data.SqlDbType.DateTime);
                cmd.Parameters["@dt_vencimento"].Value = modelo.Dt_Vencimento;
            }
            else
            {
                cmd.Parameters.Add("@dt_vencimento", System.Data.SqlDbType.DateTime);
                cmd.Parameters["@dt_vencimento"].Value = SqlDateTime.Null;
            }

            conexao.Conectar();
            cmd.ExecuteNonQuery();
            conexao.Desconectar();
        }

        public void Excluir(int codigo)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conexao.ObjetoConexao;
            cmd.CommandText = "delete from documentos where iddocumentos=@iddocumentos;";
            cmd.Parameters.AddWithValue("@iddocumentos", codigo);

            conexao.Conectar();
            cmd.ExecuteNonQuery();
            conexao.Desconectar();
        }

        public DataTable Localizar(String valor, String buscapor)
        {
            String where = "descricao";
            if (buscapor == "Título")
            {
                where = "titulo";
            }
            else
            {
                where = "descricao";
            }
            DataTable tabela = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter("select iddocumentos,titulo,descricao from documentos where " + where + " like '%" + valor + "%' order by " + where, conexao.StringConexao);
            da.Fill(tabela);
            return tabela;
        }

        public DataTable Localizar(String valor, String buscapor, int idempresas, int pageNumber, int RowsPage, string ordenapor)
        {
            String where = "titulo";
            if (buscapor == "Título")
            {
                where = "titulo";
            }
            else if (buscapor == "Descrição")
            {
                where = "descricao";
            }
            else
            {
                where = "titulo";
            }

            String order = "dt_vencimento desc,titulo";
            if (ordenapor == "Código")
            {
                order = "iddocumentos";
            }
            else if (ordenapor == "Título")
            {
                order = "titulo";
            }
            else
            {
                order = "dt_vencimento desc,titulo";
            }
            DataTable tabela = new DataTable();

            string sql = "SELECT * FROM ( " +
                            "SELECT ROW_NUMBER() OVER(ORDER BY " + where + ") as number, iddocumentos,titulo,descricao,CONVERT(VARCHAR(10), dt_vencimento,103) as dt_vencimento " +
                            "from documentos where " + where + " like '%" + valor + "%'" +
                            ") as tbl " +
                          "where " + where + " like '%" + valor + "%' and number between((" + pageNumber + " - 1) * " + RowsPage + " + 1) and(" + pageNumber + " * " + RowsPage + ") " +
                          "order by " + order;
            SqlDataAdapter da = new SqlDataAdapter(sql, conexao.StringConexao);
            da.Fill(tabela);
            return tabela;
        }

        public int TotalDocumentos()
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conexao.ObjetoConexao;
            cmd.CommandText = "select count(iddocumentos) as quant from documentos";
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

        public int VencimentoDocumento(int idempresas)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conexao.ObjetoConexao;
            cmd.CommandText = "select datediff(day, cast(getdate() as date), dt_vencimento) as dif from documentos where idempresas=" + idempresas + " and dateadd(day, -30, dt_vencimento) <= cast(getdate() as date)";
            conexao.Conectar();
            SqlDataReader registro = cmd.ExecuteReader();
            int result = -999999;
            if (registro.HasRows)
            {
                registro.Read();
                //Quantidade de dias pra vencer
                result = Convert.ToInt32(registro["dif"]);
            }
            conexao.Desconectar();
            return result;
        }

        public ModeloDocumentos CarregaDocumentos(int codigo)
        {
            ModeloDocumentos modelo = new ModeloDocumentos();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conexao.ObjetoConexao;
            cmd.CommandText = "select * from documentos where iddocumentos=" + codigo.ToString();
            conexao.Conectar();
            SqlDataReader registro = cmd.ExecuteReader();

            if (registro.HasRows)
            {
                registro.Read();
                modelo.IdDocumentos = ConverteReader.ConverteInt(registro["iddocumentos"]);
                modelo.IdEmpresas = ConverteReader.ConverteInt(registro["idempresas"]);
                modelo.Titulo = ConverteReader.ConverteString(registro["titulo"]);
                modelo.Descricao = ConverteReader.ConverteString(registro["descricao"]);
                modelo.Dt_Vencimento = ConverteReader.ConverteDateTime(registro["dt_vencimento"]);

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
