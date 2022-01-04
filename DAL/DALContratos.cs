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
    public class DALContratos
    {
        private DALConexao conexao;
        public DALContratos(DALConexao cx)
        {
            this.conexao = cx;
        }

        public void Incluir(ModeloContratos modelo)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conexao.ObjetoConexao;
            cmd.CommandText = "insert into contratos (idcontratos_clientes_localidades,contrato,descricao,vl_contrato,dt_inicio,dt_termino,responsavel,e_mail,observacoes,ativo) " +
                "values (@idcontratos_clientes_localidades,@contrato,@descricao,@vl_contrato,@dt_inicio,@dt_termino,@responsavel,@e_mail,@observacoes,@ativo); select @@IDENTITY;";
            cmd.Parameters.AddWithValue("@idcontratos_clientes_localidades", ConverteReader.ConverteInt(modelo.IdContratos_Clientes_Localidades));
            cmd.Parameters.AddWithValue("@contrato", ConverteReader.ConverteString(modelo.Contrato));
            cmd.Parameters.AddWithValue("@descricao", ConverteReader.ConverteString(modelo.Descricao));
            cmd.Parameters.AddWithValue("@vl_contrato", ConverteReader.ConverteDouble(modelo.Vl_Contrato));
            if ((modelo.Dt_Inicio != null) && (modelo.Dt_Inicio.ToString() != "01/01/0001 00:00:00"))
            {
                cmd.Parameters.Add("@dt_inicio", System.Data.SqlDbType.DateTime);
                cmd.Parameters["@dt_inicio"].Value = modelo.Dt_Inicio;
            }
            else
            {
                cmd.Parameters.Add("@dt_inicio", System.Data.SqlDbType.DateTime);
                cmd.Parameters["@dt_inicio"].Value = SqlDateTime.Null;
            }
            if ((modelo.Dt_Termino != null) && (modelo.Dt_Termino.ToString() != "01/01/0001 00:00:00"))
            {
                cmd.Parameters.Add("@dt_termino", System.Data.SqlDbType.DateTime);
                cmd.Parameters["@dt_termino"].Value = modelo.Dt_Termino;
            }
            else
            {
                cmd.Parameters.Add("@dt_termino", System.Data.SqlDbType.DateTime);
                cmd.Parameters["@dt_termino"].Value = SqlDateTime.Null;
            }
            cmd.Parameters.AddWithValue("@responsavel", ConverteReader.ConverteString(modelo.Responsavel));
            cmd.Parameters.AddWithValue("@e_mail", ConverteReader.ConverteString(modelo.E_Mail));
            cmd.Parameters.AddWithValue("@observacoes", ConverteReader.ConverteString(modelo.Observacoes));
            cmd.Parameters.AddWithValue("@ativo", ConverteReader.ConverteInt(modelo.Ativo));


            conexao.Conectar();
            modelo.IdContratos = Convert.ToInt32(cmd.ExecuteScalar());
            conexao.Desconectar();
        }

        public void Alterar(ModeloContratos modelo)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conexao.ObjetoConexao;
            cmd.CommandText = "update contratos set idcontratos_clientes_localidades=@idcontratos_clientes_localidades,contrato=@contrato,descricao=@descricao,vl_contrato=@vl_contrato,dt_inicio=@dt_inicio,dt_termino=@dt_termino,responsavel=@responsavel,e_mail=@e_mail,observacoes=@observacoes,ativo=@ativo " +
                "where idcontratos=@idcontratos;";
            cmd.Parameters.AddWithValue("@idcontratos", ConverteReader.ConverteInt(modelo.IdContratos));
            cmd.Parameters.AddWithValue("@idcontratos_clientes_localidades", ConverteReader.ConverteInt(modelo.IdContratos_Clientes_Localidades));
            cmd.Parameters.AddWithValue("@contrato", ConverteReader.ConverteString(modelo.Contrato));
            cmd.Parameters.AddWithValue("@descricao", ConverteReader.ConverteString(modelo.Descricao));
            cmd.Parameters.AddWithValue("@vl_contrato", ConverteReader.ConverteDouble(modelo.Vl_Contrato));
            if ((modelo.Dt_Inicio != null) && (modelo.Dt_Inicio.ToString() != "01/01/0001 00:00:00"))
            {
                cmd.Parameters.Add("@dt_inicio", System.Data.SqlDbType.DateTime);
                cmd.Parameters["@dt_inicio"].Value = modelo.Dt_Inicio;
            }
            else
            {
                cmd.Parameters.Add("@dt_inicio", System.Data.SqlDbType.DateTime);
                cmd.Parameters["@dt_inicio"].Value = SqlDateTime.Null;
            }
            if ((modelo.Dt_Termino != null) && (modelo.Dt_Termino.ToString() != "01/01/0001 00:00:00"))
            {
                cmd.Parameters.Add("@dt_termino", System.Data.SqlDbType.DateTime);
                cmd.Parameters["@dt_termino"].Value = modelo.Dt_Termino;
            }
            else
            {
                cmd.Parameters.Add("@dt_termino", System.Data.SqlDbType.DateTime);
                cmd.Parameters["@dt_termino"].Value = SqlDateTime.Null;
            }
            cmd.Parameters.AddWithValue("@responsavel", ConverteReader.ConverteString(modelo.Responsavel));
            cmd.Parameters.AddWithValue("@e_mail", ConverteReader.ConverteString(modelo.E_Mail));
            cmd.Parameters.AddWithValue("@observacoes", ConverteReader.ConverteString(modelo.Observacoes));
            cmd.Parameters.AddWithValue("@ativo", ConverteReader.ConverteInt(modelo.Ativo));

            conexao.Conectar();
            cmd.ExecuteNonQuery();
            conexao.Desconectar();
        }

        public void Excluir(int codigo)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conexao.ObjetoConexao;
            cmd.CommandText = "delete from contratos where idcontratos=@idcontratos;";
            cmd.Parameters.AddWithValue("@idcontratos", codigo);

            conexao.Conectar();
            cmd.ExecuteNonQuery();
            conexao.Desconectar();
        }

        public DataTable Localizar(String valor, String buscapor)
        {
            String where = "descricao";
            if (buscapor == "Contrato")
            {
                where = "contrato";
            }
            else
            {
                where = "descricao";
            }
            DataTable tabela = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter("select idcontratos,contrato,descricao from contratos where " + where + " like '%" + valor + "%' order by " + where, conexao.StringConexao);
            da.Fill(tabela);
            return tabela;
        }

        public DataTable LocalizarContratos(int idempresas, int idusuarios)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conexao.ObjetoConexao;
            cmd.CommandText = "select idusuarios from alocacao_permissao where idusuarios="+idusuarios + " and permissao='Todos'";
            conexao.Conectar();
            SqlDataReader registro = cmd.ExecuteReader();
            string where = "join alocacao_permissao ap on cl.uf = ap.permissao where ap.idusuarios = " + idusuarios;
            if (registro.HasRows)
            {
                where = " where c.idempresas=" + idempresas;
            }
            conexao.Desconectar();

            DataTable tabela = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter("select ct.idcontratos,c.nome_fantasia, cl.uf, ct.contrato, ct.descricao "+
                "from contratos ct " +
                "join clientes_localidades cl on cl.idclientes_localidades = ct.idcontratos_clientes_localidades "+
                "join clientes c on c.idclientes = cl.idclientes "+
                where, conexao.StringConexao);
            da.Fill(tabela);
            return tabela;
        }

        public DataTable Localizar(String valor, String buscapor, int idempresas, int pageNumber, int RowsPage, string ordenapor)
        {
            String where = "descricao";
            if (buscapor == "Contrato")
            {
                where = "contrato";
            }
            else
            {
                where = "descricao";
            }

            String order = "descricao";
            if (ordenapor == "Código")
            {
                order = "idcontratos";
            }
            else if (ordenapor == "Contrato")
            {
                order = "contrato";
            }
            else
            {
                order = "descricao";
            }
            DataTable tabela = new DataTable();

            string sql = "SELECT * FROM ( " +
                            "SELECT ROW_NUMBER() OVER(ORDER BY " + where + ") as number, idcontratos,contrato,descricao " +
                            "from contratos where " + where + " like '%" + valor + "%'" +
                            ") as tbl " +
                          "where " + where + " like '%" + valor + "%' and number between((" + pageNumber + " - 1) * " + RowsPage + " + 1) and(" + pageNumber + " * " + RowsPage + ") " +
                          "order by " + order;
            SqlDataAdapter da = new SqlDataAdapter(sql, conexao.StringConexao);
            da.Fill(tabela);
            return tabela;
        }

        public int TotalContratos(int idempresas)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conexao.ObjetoConexao;
            cmd.CommandText = "select count(ct.idcontratos) as quant from contratos ct left join clientes_localidades cl on cl.idclientes_localidades=ct.idcontratos_clientes_localidades left join clientes c on c.idclientes=cl.idclientes where c.idempresas="+idempresas;
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

        public int VerificaFuncionarios(int idcontratos, int idfuncionarios)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conexao.ObjetoConexao;
            string sql = "select top 1 af.idalocacao_func, f.nome, f.sobrenome, af.horario, af.horario_fim, af.aloc_uf, ct.idcontratos from alocacao_func af " +
                "left join funcionarios f on f.idfuncionarios = af.idfuncionarios " +
                "left join contratos ct on ct.idcontratos = af.aloc_contrato " +
                "where af.idfuncionarios=" + idfuncionarios +
                " order by f.nome,f.sobrenome,af.idalocacao_func desc";
            cmd.CommandText = sql;
            conexao.Conectar();
            SqlDataReader registro = cmd.ExecuteReader();
            int quant = 0;
            if (registro.HasRows)
            {
                registro.Read();
                if(registro["idcontratos"].ToString() != "")
                    if(Convert.ToInt32(registro["idcontratos"]) == idcontratos)
                        quant = 1;
            }
            conexao.Desconectar();
            return quant;
        }

        public ModeloContratos CarregaContratos(int codigo)
        {
            ModeloContratos modelo = new ModeloContratos();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conexao.ObjetoConexao;
            cmd.CommandText = "select * from contratos where idcontratos=" + codigo.ToString();
            conexao.Conectar();
            SqlDataReader registro = cmd.ExecuteReader();

            if (registro.HasRows)
            {
                registro.Read();
                modelo.IdContratos = ConverteReader.ConverteInt(registro["idcontratos"]);
                modelo.IdContratos_Clientes_Localidades = ConverteReader.ConverteInt(registro["idcontratos_clientes_localidades"]);
                modelo.Contrato = ConverteReader.ConverteString(registro["contrato"]);
                modelo.Descricao = ConverteReader.ConverteString(registro["descricao"]);
                modelo.Vl_Contrato = ConverteReader.ConverteDouble(registro["vl_contrato"]);
                modelo.Dt_Inicio = ConverteReader.ConverteDateTime(registro["dt_inicio"]);
                modelo.Dt_Termino = ConverteReader.ConverteDateTime(registro["dt_termino"]);
                modelo.Responsavel = ConverteReader.ConverteString(registro["responsavel"]);
                modelo.E_Mail = ConverteReader.ConverteString(registro["e_mail"]);
                modelo.Observacoes = ConverteReader.ConverteString(registro["observacoes"]);
                modelo.Ativo = ConverteReader.ConverteInt(registro["ativo"]);

                conexao.Desconectar();
            }
            else
            {
                conexao.Desconectar();
            }

            return modelo;
        }

        public DataTable CarregaContratosEmpresa(int idempresas)
        {
            DataTable tabela = new DataTable();

            string sql = "select ct.contrato,ct.idcontratos_clientes_localidades,ct.idcontratos, ct.descricao " +
                "from contratos ct left join clientes_localidades cl on ct.idcontratos_clientes_localidades=cl.idclientes_localidades " +
                " left join clientes c on c.idclientes=cl.idclientes where c.idempresas=" + idempresas + " and ct.ativo=1 order by ct.contrato";
            SqlDataAdapter da = new SqlDataAdapter(sql, conexao.StringConexao);
            da.Fill(tabela);
            return tabela;
        }

        public DataTable CarregaContratosEmpresa(int idempresas, string dataInicial, string dataFinal)
        {
            DataTable tabela = new DataTable();

            string sql = "select ct.contrato,ct.idcontratos_clientes_localidades,ct.idcontratos, ct.descricao, ct.dt_termino " +
                "from contratos ct left join clientes_localidades cl on ct.idcontratos_clientes_localidades=cl.idclientes_localidades " +
                " left join clientes c on c.idclientes=cl.idclientes where c.idempresas="+idempresas + " and ct.ativo=1 " +
                //" and ((ct.dt_inicio <= '" + dataFinal + "' and ct.dt_termino is null) or (ct.dt_inicio >= '" + dataInicial + "' and ct.dt_inicio <= '"+ dataFinal + "') or (ct.dt_termino >= '" + dataInicial + "' and ct.dt_termino <= '" + dataFinal + "')) " +
                " order by ct.contrato";
            SqlDataAdapter da = new SqlDataAdapter(sql, conexao.StringConexao);
            da.Fill(tabela);
            return tabela;
        }

        public DataTable CarregaContratosCliente(int idempresas)
        {
            DataTable tabela = new DataTable();

            string sql = "select ct.contrato,ct.idcontratos_clientes_localidades,ct.idcontratos, ct.descricao, c.nome_fantasia " +
                "from contratos ct left join clientes_localidades cl on ct.idcontratos_clientes_localidades=cl.idclientes_localidades " +
                " left join clientes c on c.idclientes=cl.idclientes where c.idempresas=" + idempresas + " order by c.nome_fantasia,ct.contrato";
            SqlDataAdapter da = new SqlDataAdapter(sql, conexao.StringConexao);
            da.Fill(tabela);
            return tabela;
        }

        public DataTable CarregaFuncionarios(int idcontratos)
        {
            DataTable tabela = new DataTable();

            string sql = "select f.idfuncionarios, f.nome, f.sobrenome," +
                            "(select top 1 af.aloc_contrato from alocacao_func af where af.idfuncionarios = f.idfuncionarios order by af.horario desc) as idcontratos," +
                            "(select top 1 af.horario from alocacao_func af where af.idfuncionarios = f.idfuncionarios order by af.horario desc) as horario " +
                        "from funcionarios f " +
                        "order by f.nome,f.sobrenome";
            SqlDataAdapter da = new SqlDataAdapter(sql, conexao.StringConexao);
            da.Fill(tabela);
            return tabela;
        }


    }
}
