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
    public class DALProgHorasExtras
    {
        private DALConexao conexao;
        public DALProgHorasExtras(DALConexao cx)
        {
            this.conexao = cx;
        }

        public void Incluir(ModeloProgHorasExtras modelo)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conexao.ObjetoConexao;
            cmd.CommandText = "insert into prog_horas_extras (idcontratos,idfuncionarios,idusuarios,tipo,quant_horas,dia_todo,dt_prog,liberado) " +
                "values (@idcontratos,@idfuncionarios,@idusuarios,@tipo,@quant_horas,@dia_todo,@dt_prog,@liberado); select @@IDENTITY;";
            cmd.Parameters.AddWithValue("@idfuncionarios", ConverteReader.ConverteInt(modelo.IdFuncionarios));
            cmd.Parameters.AddWithValue("@idcontratos", ConverteReader.ConverteInt(modelo.IdContratos));
            cmd.Parameters.AddWithValue("@idusuarios", ConverteReader.ConverteInt(modelo.IdUsuarios));
            cmd.Parameters.AddWithValue("@tipo", ConverteReader.ConverteString(modelo.Tipo));
            cmd.Parameters.AddWithValue("@quant_horas", ConverteReader.ConverteInt(modelo.Quant_Horas));
            cmd.Parameters.AddWithValue("@dia_todo", ConverteReader.ConverteInt(modelo.Dia_Todo));
            if ((modelo.Dt_Prog != null) && (modelo.Dt_Prog.ToString() != "01/01/0001 00:00:00"))
            {
                cmd.Parameters.Add("@dt_prog", System.Data.SqlDbType.DateTime);
                cmd.Parameters["@dt_prog"].Value = modelo.Dt_Prog;
            }
            else
            {
                cmd.Parameters.Add("@dt_prog", System.Data.SqlDbType.DateTime);
                cmd.Parameters["@dt_prog"].Value = SqlDateTime.Null;
            }
            cmd.Parameters.AddWithValue("@liberado", ConverteReader.ConverteInt(modelo.Liberado));

            conexao.Conectar();
            modelo.IdProgHorasExtras = Convert.ToInt32(cmd.ExecuteScalar());
            conexao.Desconectar();
        }

        public void IncluirItens(int item, ModeloProgHorasExtras modelo)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conexao.ObjetoConexao;
            cmd.CommandText = "insert into prog_horas_extras (idcontratos,idfuncionarios,idusuarios,tipo,quant_horas,dia_todo,dt_prog,liberado) " +
                "values (@idcontratos,@idfuncionarios,@idusuarios,@tipo,@quant_horas,@dia_todo,@dt_prog,@liberado); select @@IDENTITY;";
            cmd.Parameters.AddWithValue("@idfuncionarios", ConverteReader.ConverteInt(item));
            cmd.Parameters.AddWithValue("@idcontratos", ConverteReader.ConverteInt(modelo.IdContratos));
            cmd.Parameters.AddWithValue("@idusuarios", ConverteReader.ConverteInt(modelo.IdUsuarios));
            cmd.Parameters.AddWithValue("@tipo", ConverteReader.ConverteString(modelo.Tipo));
            cmd.Parameters.AddWithValue("@quant_horas", ConverteReader.ConverteInt(modelo.Quant_Horas));
            cmd.Parameters.AddWithValue("@dia_todo", ConverteReader.ConverteInt(modelo.Dia_Todo));
            if ((modelo.Dt_Prog != null) && (modelo.Dt_Prog.ToString() != "01/01/0001 00:00:00"))
            {
                cmd.Parameters.Add("@dt_prog", System.Data.SqlDbType.DateTime);
                cmd.Parameters["@dt_prog"].Value = modelo.Dt_Prog;
            }
            else
            {
                cmd.Parameters.Add("@dt_prog", System.Data.SqlDbType.DateTime);
                cmd.Parameters["@dt_prog"].Value = SqlDateTime.Null;
            }
            cmd.Parameters.AddWithValue("@liberado", ConverteReader.ConverteInt(modelo.Liberado));

            conexao.Conectar();
            modelo.IdProgHorasExtras = Convert.ToInt32(cmd.ExecuteScalar());
            conexao.Desconectar();
        }

        public void Alterar(ModeloProgHorasExtras modelo)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conexao.ObjetoConexao;
            cmd.CommandText = "update prog_horas_extras set idfuncionarios=@idfuncionarios,idcontratos=@idcontratos,idusuarios=@idusuarios,tipo=@tipo,quant_horas=@quant_horas,dia_todo=@dia_todo,dt_prog=@dt_prog,liberado=@liberado " +
                "where idprog_horas_extras=@idprog_horas_extras;";
            cmd.Parameters.AddWithValue("@idprog_horas_extras", ConverteReader.ConverteInt(modelo.IdProgHorasExtras));
            cmd.Parameters.AddWithValue("@idfuncionarios", ConverteReader.ConverteInt(modelo.IdFuncionarios));
            cmd.Parameters.AddWithValue("@idcontratos", ConverteReader.ConverteInt(modelo.IdContratos));
            cmd.Parameters.AddWithValue("@idusuarios", ConverteReader.ConverteInt(modelo.IdUsuarios));
            cmd.Parameters.AddWithValue("@tipo", ConverteReader.ConverteString(modelo.Tipo));
            cmd.Parameters.AddWithValue("@quant_horas", ConverteReader.ConverteInt(modelo.Quant_Horas));
            cmd.Parameters.AddWithValue("@dia_todo", ConverteReader.ConverteInt(modelo.Dia_Todo));
            if ((modelo.Dt_Prog != null) && (modelo.Dt_Prog.ToString() != "01/01/0001 00:00:00"))
            {
                cmd.Parameters.Add("@dt_prog", System.Data.SqlDbType.DateTime);
                cmd.Parameters["@dt_prog"].Value = modelo.Dt_Prog;
            }
            else
            {
                cmd.Parameters.Add("@dt_prog", System.Data.SqlDbType.DateTime);
                cmd.Parameters["@dt_prog"].Value = SqlDateTime.Null;
            }
            cmd.Parameters.AddWithValue("@liberado", ConverteReader.ConverteInt(modelo.Liberado));

            conexao.Conectar();
            cmd.ExecuteNonQuery();
            conexao.Desconectar();
        }

        public void Excluir(int codigo)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conexao.ObjetoConexao;
            cmd.CommandText = "delete from prog_horas_extras where idprog_horas_extras=@idprog_horas_extras;";
            cmd.Parameters.AddWithValue("@idprog_horas_extras", codigo);

            conexao.Conectar();
            cmd.ExecuteNonQuery();
            conexao.Desconectar();
        }

        public DataTable Localizar(String valor, String buscapor, int idempresas, int idusuarios, int pageNumber, int RowsPage, string ordenapor)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conexao.ObjetoConexao;
            cmd.CommandText = "select idalocacao_permissao from alocacao_permissao where idusuarios=" + idusuarios + " and permissao='Todos'";
            conexao.Conectar();
            SqlDataReader registro = cmd.ExecuteReader();
            string todos = " and idusuarios=" + idusuarios;
            if (registro.HasRows)
            {
                todos = "";
            }
            conexao.Desconectar();

            String where = "c.contrato";
            String where2 = "contrato";
            if (buscapor == "Dt. Programação")
            {
                where = "p.dt_prog";
                where2 = "dt_prog";
            }
            else
            {
                where = "c.contrato";
                where2 = "contrato";
            }

            String order = "dt_prog,contrato";
            if (ordenapor == "Código")
            {
                order = "idprog_horas_extras";
            }
            else if (ordenapor == "Contrato")
            {
                order = "contrato";
            }
            else
            {
                order = "dt_prog desc,idprog_horas_extras desc,contrato";
            }
            DataTable tabela = new DataTable();

            string sql = "SELECT * FROM ( " +
                            "SELECT ROW_NUMBER() OVER(ORDER BY " + where + ") as number, p.idprog_horas_extras,c.contrato,c.descricao,CONVERT(VARCHAR(10), p.dt_prog,103) as dt_prog, p.liberado, f.nome, f.sobrenome, p.tipo, p.quant_horas " +
                            "from prog_horas_extras p join contratos c on c.idcontratos=p.idcontratos join funcionarios f on f.idfuncionarios=p.idfuncionarios where " + where + " like '%" + valor + "%'" + todos +
                            ") as tbl " +
                          "where " + where2 + " like '%" + valor + "%' and number between((" + pageNumber + " - 1) * " + RowsPage + " + 1) and(" + pageNumber + " * " + RowsPage + ") " +
                          "order by " + order;
            SqlDataAdapter da = new SqlDataAdapter(sql, conexao.StringConexao);
            da.Fill(tabela);
            return tabela;
        }

        public int TotalProgramas()
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conexao.ObjetoConexao;
            cmd.CommandText = "select count(idprog_horas_extras) as quant from prog_horas_extras";
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

        public ModeloProgHorasExtras CarregaProgHorasExtras(int codigo)
        {
            ModeloProgHorasExtras modelo = new ModeloProgHorasExtras();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conexao.ObjetoConexao;
            cmd.CommandText = "select * from prog_horas_extras where idprog_horas_extras=" + codigo.ToString();
            conexao.Conectar();
            SqlDataReader registro = cmd.ExecuteReader();

            if (registro.HasRows)
            {
                registro.Read();
                modelo.IdProgHorasExtras = ConverteReader.ConverteInt(registro["idprog_horas_extras"]);
                modelo.IdFuncionarios = ConverteReader.ConverteInt(registro["idfuncionarios"]);
                modelo.IdContratos = ConverteReader.ConverteInt(registro["idcontratos"]);
                modelo.IdUsuarios = ConverteReader.ConverteInt(registro["idusuarios"]);
                modelo.Tipo = ConverteReader.ConverteString(registro["tipo"]);
                modelo.Quant_Horas = ConverteReader.ConverteInt(registro["quant_horas"]);
                modelo.Dia_Todo = ConverteReader.ConverteInt(registro["dia_todo"]);
                modelo.Dt_Prog = ConverteReader.ConverteDateTime(registro["dt_prog"]);
                modelo.Liberado = ConverteReader.ConverteInt(registro["liberado"]);

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
