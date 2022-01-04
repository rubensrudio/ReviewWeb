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
    public class DALExames
    {
        private DALConexao conexao;
        public DALExames(DALConexao cx)
        {
            this.conexao = cx;
        }

        public void Incluir(ModeloExames modelo)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conexao.ObjetoConexao;
            cmd.CommandText = "insert into exames (idfuncionarios,exame,descricao,dt_exame,dt_vencimento) " +
                "values (@idfuncionarios,@exame,@descricao,@dt_exame,@dt_vencimento); select @@IDENTITY;";
            cmd.Parameters.AddWithValue("@idfuncionarios", ConverteReader.ConverteInt(modelo.IdFuncionarios));
            cmd.Parameters.AddWithValue("@exame", ConverteReader.ConverteString(modelo.Exame));
            cmd.Parameters.AddWithValue("@descricao", ConverteReader.ConverteString(modelo.Descricao));
            if ((modelo.Dt_Exame != null) && (modelo.Dt_Exame.ToString() != "01/01/0001 00:00:00"))
            {
                cmd.Parameters.Add("@dt_exame", System.Data.SqlDbType.DateTime);
                cmd.Parameters["@dt_exame"].Value = modelo.Dt_Exame;
            }
            else
            {
                cmd.Parameters.Add("@dt_exame", System.Data.SqlDbType.DateTime);
                cmd.Parameters["@dt_exame"].Value = SqlDateTime.Null;
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
            modelo.IdExames = Convert.ToInt32(cmd.ExecuteScalar());
            conexao.Desconectar();
        }

        public void Alterar(ModeloExames modelo)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conexao.ObjetoConexao;
            cmd.CommandText = "update exames set idfuncionarios=@idfuncionarios,exame=@exame,descricao=@descricao,dt_exame=@dt_exame,dt_vencimento=@dt_vencimento " +
                "where idexames=@idexames;";
            cmd.Parameters.AddWithValue("@idexames", ConverteReader.ConverteInt(modelo.IdExames));
            cmd.Parameters.AddWithValue("@idfuncionarios", ConverteReader.ConverteInt(modelo.IdFuncionarios));
            cmd.Parameters.AddWithValue("@exame", ConverteReader.ConverteString(modelo.Exame));
            cmd.Parameters.AddWithValue("@descricao", ConverteReader.ConverteString(modelo.Descricao));
            if ((modelo.Dt_Exame != null) && (modelo.Dt_Exame.ToString() != "01/01/0001 00:00:00"))
            {
                cmd.Parameters.Add("@dt_exame", System.Data.SqlDbType.DateTime);
                cmd.Parameters["@dt_exame"].Value = modelo.Dt_Exame;
            }
            else
            {
                cmd.Parameters.Add("@dt_exame", System.Data.SqlDbType.DateTime);
                cmd.Parameters["@dt_exame"].Value = SqlDateTime.Null;
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
            cmd.CommandText = "delete from exames where idexames=@idexames;";
            cmd.Parameters.AddWithValue("@idexames", codigo);

            conexao.Conectar();
            cmd.ExecuteNonQuery();
            conexao.Desconectar();
        }

        public DataTable Localizar(String valor, String buscapor)
        {
            String where = "descricao";
            if (buscapor == "Exame")
            {
                where = "exame";
            }
            else
            {
                where = "descricao";
            }
            DataTable tabela = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter("select idexames,exame,descricao from exames where " + where + " like '%" + valor + "%' order by " + where, conexao.StringConexao);
            da.Fill(tabela);
            return tabela;
        }

        public DataTable Localizar(String valor, String buscapor, int idempresas, int pageNumber, int RowsPage, string ordenapor)
        {
            String where = "f.nome";
            String where2 = "nome";
            if (buscapor == "Exame")
            {
                where = "e.exame";
                where2 = "exame";
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

            String order = "e.dt_exame,f.nome,f.sobrenome";
            String order2 = "dt_exame,nome,sobrenome";
            if (ordenapor == "Código")
            {
                order = "e.idexames";
                order2 = "idexames";
            }
            else if (ordenapor == "Exame")
            {
                order = "e.exame";
                order2 = "exame";
            }
            else
            {
                order = "e.dt_exame,f.nome,f.sobrenome";
                order2 = "dt_exame,nome,sobrenome";
            }
            DataTable tabela = new DataTable();

            string sql = "SELECT * FROM ( " +
                            "SELECT ROW_NUMBER() OVER(ORDER BY " + where + ") as number, e.idexames,f.nome,f.sobrenome,e.exame,e.descricao,CONVERT(VARCHAR(10), e.dt_exame,103) as dt_exame,CONVERT(VARCHAR(10), e.dt_vencimento,103) as dt_vencimento " +
                            "from exames e join funcionarios f on f.idfuncionarios=e.idfuncionarios where " + where + " like '%" + valor + "%'" +
                            ") as tbl " +
                          "where " + where2 + " like '%" + valor + "%' and number between((" + pageNumber + " - 1) * " + RowsPage + " + 1) and(" + pageNumber + " * " + RowsPage + ") " +
                          "order by " + order2;
            SqlDataAdapter da = new SqlDataAdapter(sql, conexao.StringConexao);
            da.Fill(tabela);
            return tabela;
        }

        public int TotalExames()
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conexao.ObjetoConexao;
            cmd.CommandText = "select count(idexames) as quant from exames";
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

        public int VencimentoExame(int idfuncionarios)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conexao.ObjetoConexao;
            cmd.CommandText = "select datediff(day, cast(getdate() as date), dt_vencimento) as dif from exames where idfuncionarios="+idfuncionarios+" and dateadd(day, -30, dt_vencimento) <= cast(getdate() as date)";
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

        public ModeloExames CarregaExames(int codigo)
        {
            ModeloExames modelo = new ModeloExames();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conexao.ObjetoConexao;
            cmd.CommandText = "select * from exames where idexames=" + codigo.ToString();
            conexao.Conectar();
            SqlDataReader registro = cmd.ExecuteReader();

            if (registro.HasRows)
            {
                registro.Read();
                modelo.IdExames = ConverteReader.ConverteInt(registro["idexames"]);
                modelo.IdFuncionarios = ConverteReader.ConverteInt(registro["idfuncionarios"]);
                modelo.Exame = ConverteReader.ConverteString(registro["exame"]);
                modelo.Descricao = ConverteReader.ConverteString(registro["descricao"]);
                modelo.Dt_Exame = ConverteReader.ConverteDateTime(registro["dt_exame"]);
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
