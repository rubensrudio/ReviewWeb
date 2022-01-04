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
    public class DALAlocacaoFunc
    {
        private DALConexao conexao;
        public DALAlocacaoFunc(DALConexao cx)
        {
            this.conexao = cx;
        }
        
        public void Alterar(ModeloAlocacaoFunc modelo)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conexao.ObjetoConexao;
            cmd.CommandText = "update alocacao_func set horario=@horario, horario_fim=@horario_fim " +
                "where idalocacao_func=@idalocacao_func;";
            cmd.Parameters.AddWithValue("@idalocacao_func", ConverteReader.ConverteInt(modelo.IdAlocacao_Func));
            if ((modelo.Horario != null) && (modelo.Horario.ToString() != "01/01/0001 00:00:00"))
            {
                cmd.Parameters.Add("@horario", System.Data.SqlDbType.DateTime);
                cmd.Parameters["@horario"].Value = modelo.Horario;
            }
            else
            {
                cmd.Parameters.Add("@horario", System.Data.SqlDbType.DateTime);
                cmd.Parameters["@horario"].Value = SqlDateTime.Null;
            }
            if ((modelo.Horario_Fim != null) && (modelo.Horario_Fim.ToString() != "01/01/0001 00:00:00"))
            {
                cmd.Parameters.Add("@horario_fim", System.Data.SqlDbType.DateTime);
                cmd.Parameters["@horario_fim"].Value = modelo.Horario_Fim;
            }
            else
            {
                cmd.Parameters.Add("@horario_fim", System.Data.SqlDbType.DateTime);
                cmd.Parameters["@horario_fim"].Value = SqlDateTime.Null;
            }

            conexao.Conectar();
            cmd.ExecuteNonQuery();
            conexao.Desconectar();
        }

        public void Excluir(int codigo)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conexao.ObjetoConexao;
            cmd.CommandText = "delete from alocacao_func where idalocacao_func=@idalocacao_func;";
            cmd.Parameters.AddWithValue("@idalocacao_func", codigo);

            conexao.Conectar();
            cmd.ExecuteNonQuery();
            conexao.Desconectar();
        }

        public DataTable Localizar(String valor, String buscapor, int idempresas, int pageNumber, int RowsPage, string ordenapor)
        {
            String where = "f.nome";
            String where2 = "nome";
            if (buscapor == "Sobrenome")
            {
                where = "f.sobrenome";
                where2 = "sobrenome";
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

            String order = "f.nome,f.sobrenome,af.idalocacao_func desc";
            String order2 = "nome,sobrenome,idalocacao_func desc";

            DataTable tabela = new DataTable();

            string sql = "SELECT * FROM ( " +
                            "SELECT ROW_NUMBER() OVER(ORDER BY " + order + ") as number, af.idalocacao_func,f.nome,f.sobrenome,c.contrato,cl.nome_fantasia as cliente,af.horario,af.aloc_uf,CONVERT(VARCHAR(10), af.horario,103) as datainicio,CONVERT(VARCHAR(8), af.horario,8) as horainicio,CONVERT(VARCHAR(10), af.horario_fim,103) as datafim,CONVERT(VARCHAR(8), af.horario_fim,8) as horafim " +
                            "from alocacao_func af left join funcionarios f on f.idfuncionarios=af.idfuncionarios " +
                            "left join contratos c on c.idcontratos=af.aloc_contrato " +
                            "left join clientes cl on cl.idclientes=af.aloc_cliente " +
                            "where " + where + " like '%" + valor + "%'" +
                            ") as tbl " +
                          "where " + where2 + " like '%" + valor + "%' and number between((" + pageNumber + " - 1) * " + RowsPage + " + 1) and(" + pageNumber + " * " + RowsPage + ") " +
                          "order by " + order2;
            SqlDataAdapter da = new SqlDataAdapter(sql, conexao.StringConexao);
            da.Fill(tabela);
            return tabela;
        }

        public DataTable UltimaFolga(int idfuncionarios)
        {
            DataTable tabela = new DataTable();

            string sql = "select top 1 af.idalocacao_func, f.nome, f.sobrenome, af.horario, af.horario_fim, af.aloc_uf from alocacao_func af " +
                "left join funcionarios f on f.idfuncionarios = af.idfuncionarios " +
                "where af.idfuncionarios="+ idfuncionarios + " and af.aloc_uf='FO' " +
                "order by f.nome,f.sobrenome,af.idalocacao_func desc";
            SqlDataAdapter da = new SqlDataAdapter(sql, conexao.StringConexao);
            da.Fill(tabela);

            return tabela;
        }

        public DataTable UltimaLocal(int idfuncionarios)
        {
            DataTable tabela = new DataTable();

            string sql = "select top 1 af.idalocacao_func, f.nome, f.sobrenome, af.horario, af.horario_fim, af.aloc_uf from alocacao_func af " +
                "left join funcionarios f on f.idfuncionarios = af.idfuncionarios " +
                "where af.idfuncionarios=" + idfuncionarios + " and (af.aloc_uf=f.uf_contratacao or (af.aloc_uf='' and f.uf_contratacao='ES')) " +
                "order by f.nome,f.sobrenome,af.idalocacao_func desc";
            SqlDataAdapter da = new SqlDataAdapter(sql, conexao.StringConexao);
            da.Fill(tabela);

            return tabela;
        }
        

        public int TotalAlocacaoFunc(String valor, String buscapor, int idempresas)
        {
            String where = "where f.nome like '%"+valor+"%' and af.idempresas="+ idempresas;
            if (buscapor == "Sobrenome")
            {
                where = "where f.sobrenome like '%" + valor + "%' and af.idempresas=" + idempresas;
            }
            else if (buscapor == "Nome")
            {
                where = "where f.nome like '%" + valor + "%' and af.idempresas=" + idempresas;
            }
            else
            {
                where = "where f.nome like '%" + valor + "%' and af.idempresas=" + idempresas;
            }
            

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conexao.ObjetoConexao;
            cmd.CommandText = "select count(af.idalocacao_func) as quant from alocacao_func af left join funcionarios f on f.idfuncionarios=af.idfuncionarios "+ where;
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

        
        public ModeloAlocacaoFunc CarregaAlocacaoFunc(int codigo)
        {
            ModeloAlocacaoFunc modelo = new ModeloAlocacaoFunc();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conexao.ObjetoConexao;
            cmd.CommandText = "select af.idalocacao_func, format(af.horario, 'dd/MM/yyyy HH:mm:ss') as horario, format(af.horario_fim, 'dd/MM/yyyy HH:mm:ss') as horario_fim,f.nome,f.sobrenome,c.contrato,cl.nome_fantasia from alocacao_func af " +
                "left join funcionarios f on f.idfuncionarios=af.idfuncionarios " +
                "left join clientes cl on cl.idclientes=af.aloc_cliente " +
                "left join contratos c on c.idcontratos=af.aloc_contrato " +
                "where af.idalocacao_func=" + codigo.ToString();
            conexao.Conectar();
            SqlDataReader registro = cmd.ExecuteReader();

            if (registro.HasRows)
            {
                registro.Read();
                modelo.IdAlocacao_Func = ConverteReader.ConverteInt(registro["idalocacao_func"]);
                if ((registro["horario"] != null) && (registro["horario"].ToString() != "") && (registro["horario"].ToString() != "01/01/0001 00:00:00"))
                {
                    modelo.Horario = ConverteReader.ConverteDateTime(registro["horario"]);
                }
                else
                {
                    modelo.Horario = null;
                }
                if ((registro["horario_fim"] != null) && (registro["horario_fim"].ToString() != "") && (registro["horario_fim"].ToString() != "01/01/0001 00:00:00"))
                {
                    string str = registro["horario_fim"].ToString();
                    modelo.Horario_Fim = ConverteReader.ConverteDateTime(registro["horario_fim"]);
                }
                else
                {
                    modelo.Horario_Fim = null;
                }
                modelo.Funcionario = registro["nome"].ToString() + " " + registro["sobrenome"].ToString();
                modelo.Contrato = registro["contrato"].ToString();
                modelo.Cliente = registro["nome_fantasia"].ToString();

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
