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
    public class DALTreinamentos
    {
        private DALConexao conexao;
        public DALTreinamentos(DALConexao cx)
        {
            this.conexao = cx;
        }

        public void Incluir(ModeloTreinamentos modelo)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conexao.ObjetoConexao;
            cmd.CommandText = "insert into treinamentos (idfuncionarios,treinamento,descricao,dt_treinamento,dt_vencimento) " +
                "values (@idfuncionarios,@treinamento,@descricao,@dt_treinamento,@dt_vencimento); select @@IDENTITY;";
            cmd.Parameters.AddWithValue("@idfuncionarios", ConverteReader.ConverteInt(modelo.IdFuncionarios));
            cmd.Parameters.AddWithValue("@treinamento", ConverteReader.ConverteString(modelo.Treinamento));
            cmd.Parameters.AddWithValue("@descricao", ConverteReader.ConverteString(modelo.Descricao));
            if ((modelo.Dt_Treinamento != null) && (modelo.Dt_Treinamento.ToString() != "01/01/0001 00:00:00"))
            {
                cmd.Parameters.Add("@dt_treinamento", System.Data.SqlDbType.DateTime);
                cmd.Parameters["@dt_treinamento"].Value = modelo.Dt_Treinamento;
            }
            else
            {
                cmd.Parameters.Add("@dt_treinamento", System.Data.SqlDbType.DateTime);
                cmd.Parameters["@dt_treinamento"].Value = SqlDateTime.Null;
            }
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
            modelo.IdTreinamentos = Convert.ToInt32(cmd.ExecuteScalar());
            conexao.Desconectar();
        }

        public void Alterar(ModeloTreinamentos modelo)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conexao.ObjetoConexao;
            cmd.CommandText = "update treinamentos set idfuncionarios=@idfuncionarios,treinamento=@treinamento,descricao=@descricao,dt_treinamento=@dt_treinamento,dt_vencimento=@dt_vencimento " +
                "where idtreinamentos=@idtreinamentos;";
            cmd.Parameters.AddWithValue("@idtreinamentos", ConverteReader.ConverteInt(modelo.IdTreinamentos));
            cmd.Parameters.AddWithValue("@idfuncionarios", ConverteReader.ConverteInt(modelo.IdFuncionarios));
            cmd.Parameters.AddWithValue("@treinamento", ConverteReader.ConverteString(modelo.Treinamento));
            cmd.Parameters.AddWithValue("@descricao", ConverteReader.ConverteString(modelo.Descricao));
            if ((modelo.Dt_Treinamento != null) && (modelo.Dt_Treinamento.ToString() != "01/01/0001 00:00:00"))
            {
                cmd.Parameters.Add("@dt_treinamento", System.Data.SqlDbType.DateTime);
                cmd.Parameters["@dt_treinamento"].Value = modelo.Dt_Treinamento;
            }
            else
            {
                cmd.Parameters.Add("@dt_treinamento", System.Data.SqlDbType.DateTime);
                cmd.Parameters["@dt_treinamento"].Value = SqlDateTime.Null;
            }
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
            cmd.CommandText = "delete from treinamentos where idtreinamentos=@idtreinamentos;";
            cmd.Parameters.AddWithValue("@idtreinamentos", codigo);

            conexao.Conectar();
            cmd.ExecuteNonQuery();
            conexao.Desconectar();
        }

        public DataTable Localizar(String valor, String buscapor)
        {
            String where = "descricao";
            if (buscapor == "Treinamento")
            {
                where = "treinamento";
            }
            else
            {
                where = "descricao";
            }
            DataTable tabela = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter("select idtreinamentos,treinamento,descricao from treinamentos where " + where + " like '%" + valor + "%' order by " + where, conexao.StringConexao);
            da.Fill(tabela);
            return tabela;
        }

        public DataTable Localizar(String valor, String buscapor, int idempresas, int pageNumber, int RowsPage, string ordenapor)
        {
            String where = "f.nome";
            String where2 = "nome";
            if (buscapor == "Treinamento")
            {
                where = "e.treinamento";
                where2 = "treinamento";
            }
            else if (buscapor == "Nome")
            {
                where = "f.nome";
                where2 = "nome";
            }
            else
            {
                where = "f.nome";
                where2 = "nome";
            }

            String order = "e.dt_treinamento,f.nome,f.sobrenome";
            String order2 = "dt_treinamento,nome,sobrenome";
            if (ordenapor == "Código")
            {
                order = "e.idtreinamentos";
                order2 = "idtreinamentos";
            }
            else if (ordenapor == "Treinamento")
            {
                order = "e.treinamento";
                order2 = "treinamento";
            }
            else
            {
                order = "e.dt_treinamento,f.nome,f.sobrenome";
                order2 = "dt_treinamento,nome,sobrenome";
            }
            DataTable tabela = new DataTable();

            string sql = "SELECT * FROM ( " +
                            "SELECT ROW_NUMBER() OVER(ORDER BY " + where + ") as number, e.idtreinamentos,f.nome,f.sobrenome,e.treinamento,e.descricao,CONVERT(VARCHAR(10), e.dt_treinamento,103) as dt_treinamento,CONVERT(VARCHAR(10), e.dt_vencimento,103) as dt_vencimento " +
                            "from treinamentos e join funcionarios f on f.idfuncionarios=e.idfuncionarios where " + where + " like '%" + valor + "%'" +
                            ") as tbl " +
                          "where " + where2 + " like '%" + valor + "%' and number between((" + pageNumber + " - 1) * " + RowsPage + " + 1) and(" + pageNumber + " * " + RowsPage + ") " +
                          "order by " + order2;
            SqlDataAdapter da = new SqlDataAdapter(sql, conexao.StringConexao);
            da.Fill(tabela);
            return tabela;
        }

        public int TotalTreinamentos()
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conexao.ObjetoConexao;
            cmd.CommandText = "select count(idtreinamentos) as quant from treinamentos";
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

        public int VencimentoTreinamento(int idfuncionarios)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conexao.ObjetoConexao;
            cmd.CommandText = "select datediff(day, cast(getdate() as date), dt_vencimento) as dif from treinamentos where idfuncionarios=" + idfuncionarios + " and dateadd(day, -30, dt_vencimento) <= cast(getdate() as date)";
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

        public ModeloTreinamentos CarregaTreinamentos(int codigo)
        {
            ModeloTreinamentos modelo = new ModeloTreinamentos();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conexao.ObjetoConexao;
            cmd.CommandText = "select * from treinamentos where idtreinamentos=" + codigo.ToString();
            conexao.Conectar();
            SqlDataReader registro = cmd.ExecuteReader();

            if (registro.HasRows)
            {
                registro.Read();
                modelo.IdTreinamentos = ConverteReader.ConverteInt(registro["idtreinamentos"]);
                modelo.IdFuncionarios = ConverteReader.ConverteInt(registro["idfuncionarios"]);
                modelo.Treinamento = ConverteReader.ConverteString(registro["treinamento"]);
                modelo.Descricao = ConverteReader.ConverteString(registro["descricao"]);
                modelo.Dt_Treinamento = ConverteReader.ConverteDateTime(registro["dt_treinamento"]);
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
