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
    public class DALFolhaPagamentos
    {
        private DALConexao conexao;
        public DALFolhaPagamentos(DALConexao cx)
        {
            this.conexao = cx;
        }

        public void Incluir(ModeloFolhaPagamentos modelo)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conexao.ObjetoConexao;
            cmd.CommandText = "insert into folha_pagamento (idempresas,idfuncionarios,mes_base,salario_base,salario_liquido,inss,irrf,plano_saude,outros_descontos,horas_extras) " +
                "values (@idempresas,@idfuncionarios,@mes_base,@salario_liquido,@inss,@irrf,@plano_saude,@outros_descontos,@horas_extras); select @@IDENTITY;";
            cmd.Parameters.AddWithValue("@idempresas", ConverteReader.ConverteInt(modelo.IdEmpresas));
            cmd.Parameters.AddWithValue("@idfuncionarios", ConverteReader.ConverteInt(modelo.IdFuncionarios));
            if ((modelo.Mes_Base != null) && (modelo.Mes_Base.ToString() != "01/01/0001 00:00:00"))
            {
                cmd.Parameters.Add("@mes_base", System.Data.SqlDbType.DateTime);
                cmd.Parameters["@mes_base"].Value = modelo.Mes_Base;
            }
            else
            {
                cmd.Parameters.Add("@mes_base", System.Data.SqlDbType.DateTime);
                cmd.Parameters["@mes_base"].Value = SqlDateTime.Null;
            }
            cmd.Parameters.AddWithValue("@salario_base", ConverteReader.ConverteDouble(modelo.Salario_Base));
            cmd.Parameters.AddWithValue("@salario_liquido", ConverteReader.ConverteDouble(modelo.Salario_Liquido));
            cmd.Parameters.AddWithValue("@inss", ConverteReader.ConverteDouble(modelo.INSS));
            cmd.Parameters.AddWithValue("@irrf", ConverteReader.ConverteDouble(modelo.IRRF));
            cmd.Parameters.AddWithValue("@plano_saude", ConverteReader.ConverteDouble(modelo.Plano_Saude));
            cmd.Parameters.AddWithValue("@outros_descontos", ConverteReader.ConverteDouble(modelo.Outros_Descontos));
            cmd.Parameters.AddWithValue("@horas_extras", ConverteReader.ConverteDouble(modelo.Horas_Extras));

            conexao.Conectar();
            modelo.IdFolha_Pagamento = Convert.ToInt32(cmd.ExecuteScalar());
            conexao.Desconectar();
        }
       
        public void Excluir(string mes_base)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conexao.ObjetoConexao;
            cmd.CommandText = "delete from folha_pagamento where mes_base=@mes_base;";
            cmd.Parameters.AddWithValue("@mes_base", mes_base);

            conexao.Conectar();
            cmd.ExecuteNonQuery();
            conexao.Desconectar();
        }

        public DataTable Localizar(String valor, String buscapor, int idempresas, int pageNumber, int RowsPage, string ordenapor)
        {
            String where = "f.nome";
            String where2 = "nome";
            
            String order = "fp.mes_base desc,f.nome,f.sobrenome";
            String order2 = "mes_base,nome,sobrenome";
            
            DataTable tabela = new DataTable();

            string sql = "SELECT * FROM ( " +
                            "SELECT ROW_NUMBER() OVER(ORDER BY " + where + ") as number, fp.idfolha_pagamento,f.nome,f.sobrenome,CONVERT(VARCHAR(10), fp.mes_base,103) as mes_base " +
                            "from folha_pagamento fp join funcionarios f on f.idfuncionarios=fp.idfuncionarios where " + where + " like '%" + valor + "%'" +
                            ") as tbl " +
                          "where " + where2 + " like '%" + valor + "%' and number between((" + pageNumber + " - 1) * " + RowsPage + " + 1) and(" + pageNumber + " * " + RowsPage + ") " +
                          "order by " + order2;
            SqlDataAdapter da = new SqlDataAdapter(sql, conexao.StringConexao);
            da.Fill(tabela);
            return tabela;
        }

        public int TotalFolha_Pagamento()
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conexao.ObjetoConexao;
            cmd.CommandText = "select count(idfolha_pagamento) as quant from folha_pagamento";
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
        
        public ModeloFolhaPagamentos CarregaFolha_Pagamento(string mes_base)
        {
            ModeloFolhaPagamentos modelo = new ModeloFolhaPagamentos();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conexao.ObjetoConexao;
            cmd.CommandText = "select f.nome,f.sobrenome,fp.* from folha_pagamento fp join funcionarios f on f.idfuncionarios=fp.idfuncionarios where fp.mes_base='" + mes_base + "' order by f.nome,f.sobrenome";
            conexao.Conectar();
            SqlDataReader registro = cmd.ExecuteReader();

            if (registro.HasRows)
            {
                registro.Read();
                modelo.IdFolha_Pagamento = ConverteReader.ConverteInt(registro["idfolha_pagamento"]);
                modelo.IdFuncionarios = ConverteReader.ConverteInt(registro["idfuncionarios"]);
                modelo.IdEmpresas = ConverteReader.ConverteInt(registro["idempresas"]);
                modelo.Mes_Base = ConverteReader.ConverteDateTime(registro["mes_base"]);
                modelo.Salario_Base = ConverteReader.ConverteDecimal(registro["salario_base"]);
                modelo.Salario_Liquido = ConverteReader.ConverteDecimal(registro["salario_liquido"]);
                modelo.INSS = ConverteReader.ConverteDecimal(registro["inss"]);
                modelo.IRRF = ConverteReader.ConverteDecimal(registro["irrf"]);
                modelo.Plano_Saude = ConverteReader.ConverteDecimal(registro["plano_saude"]);
                modelo.Outros_Descontos = ConverteReader.ConverteDecimal(registro["outros_descontos"]);
                modelo.Horas_Extras = ConverteReader.ConverteDecimal(registro["horas_extras"]);

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
