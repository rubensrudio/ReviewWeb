using System;
using Modelo;
using System.Data.SqlClient;
using System.Data;
using System.IO;

namespace DAL
{
    public class DALEmpresa
    {
        private DALConexao conexao;
        public DALEmpresa(DALConexao cx)
        {
            this.conexao = cx;
        }

        public void Incluir(ModeloEmpresa modelo,ModeloEndereco modeloEnd)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conexao.ObjetoConexao;
            cmd.CommandText = "insert into empresas (idenderecos,razao_social,nome_fantasia,cnpj,insc_est,email,responsavel,ramo_atividade,telefone1,telefone2,logo,nome_arquivo) " +
                "values (@idenderecos,@razao_social,@nome_fantasia,@cnpj,@insc_est,@email,@responsavel,@ramo_atividade,@telefone1,@telefone2,@logo,@nome_arquivo); select @@IDENTITY;";
            cmd.Parameters.AddWithValue("@idenderecos", ConverteReader.ConverteInt(modeloEnd.IdEnderecos));
            cmd.Parameters.AddWithValue("@razao_social", ConverteReader.ConverteString(modelo.Razao_Social));
            cmd.Parameters.AddWithValue("@nome_fantasia", ConverteReader.ConverteString(modelo.Nome_Fantasia));
            cmd.Parameters.AddWithValue("@cnpj", ConverteReader.ConverteString(modelo.CNPJ));
            cmd.Parameters.AddWithValue("@insc_est", ConverteReader.ConverteString(modelo.Insc_Est));
            cmd.Parameters.AddWithValue("@email", ConverteReader.ConverteString(modelo.Email));
            cmd.Parameters.AddWithValue("@responsavel", ConverteReader.ConverteString(modelo.Responsavel));
            cmd.Parameters.AddWithValue("@ramo_atividade", ConverteReader.ConverteString(modelo.Ramo_Atividade));
            cmd.Parameters.AddWithValue("@telefone1", ConverteReader.ConverteString(modelo.Telefone1));
            cmd.Parameters.AddWithValue("@telefone2", ConverteReader.ConverteString(modelo.Telefone2));
            cmd.Parameters.AddWithValue("@logo", modelo.Logo);
            cmd.Parameters.AddWithValue("@nome_arquivo", ConverteReader.ConverteString(modelo.Nome_Arquivo));

            conexao.Conectar();
            modelo.IdEmpresas = Convert.ToInt32(cmd.ExecuteScalar());
            conexao.Desconectar();
        }

        public void Alterar(ModeloEmpresa modelo)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conexao.ObjetoConexao;
            cmd.CommandText = "update empresas set razao_social=@razao_social,nome_fantasia=@nome_fantasia,cnpj=@cnpj,insc_est=@insc_est,email=@email,responsavel=@responsavel,ramo_atividade=@ramo_atividade,telefone1=@telefone1,telefone2=@telefone2,logo=@logo,nome_arquivo=@nome_arquivo " +
                "where idempresas=@idempresas;";
            cmd.Parameters.AddWithValue("@idempresas", ConverteReader.ConverteInt(modelo.IdEmpresas));
            cmd.Parameters.AddWithValue("@razao_social", ConverteReader.ConverteString(modelo.Razao_Social));
            cmd.Parameters.AddWithValue("@nome_fantasia", ConverteReader.ConverteString(modelo.Nome_Fantasia));
            cmd.Parameters.AddWithValue("@cnpj", ConverteReader.ConverteString(modelo.CNPJ));
            cmd.Parameters.AddWithValue("@insc_est", ConverteReader.ConverteString(modelo.Insc_Est));
            cmd.Parameters.AddWithValue("@email", ConverteReader.ConverteString(modelo.Email));
            cmd.Parameters.AddWithValue("@responsavel", ConverteReader.ConverteString(modelo.Responsavel));
            cmd.Parameters.AddWithValue("@ramo_atividade", ConverteReader.ConverteString(modelo.Ramo_Atividade));
            cmd.Parameters.AddWithValue("@telefone1", ConverteReader.ConverteString(modelo.Telefone1));
            cmd.Parameters.AddWithValue("@telefone2", ConverteReader.ConverteString(modelo.Telefone2));
            cmd.Parameters.AddWithValue("@logo", modelo.Logo);
            cmd.Parameters.AddWithValue("@nome_arquivo", ConverteReader.ConverteString(modelo.Nome_Arquivo));

            conexao.Conectar();
            cmd.ExecuteNonQuery();
            conexao.Desconectar();
        }

        public void Excluir(int codigo)
        {
            int idenderecos = LocalizarEnd(codigo);

            //Excluir Empresa
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conexao.ObjetoConexao;
            cmd.CommandText = "delete from empresas where idempresas=@idempresas;";
            cmd.Parameters.AddWithValue("@idempresas", codigo);

            conexao.Conectar();
            cmd.ExecuteNonQuery();
            conexao.Desconectar();

            //Excluir Endereço
            DALEndereco DALObj = new DALEndereco(this.conexao);
            DALObj.Excluir(idenderecos);
        }

        public DataTable Localizar(String valor, String buscapor)
        {
            String where = "nome_fantasia";
            if(buscapor == "Razão Social")
            {
                where = "razao_social";
            }
            else if(buscapor == "Nome Fantasia")
            {
                where = "nome_fantasia";
            }
            else
            {
                where = "CNPJ";
            }
            DataTable tabela = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter("select idempresas,razao_social,nome_fantasia,cnpj from empresas where "+where+" like '%" + valor + "%' order by " + where, conexao.StringConexao);
            da.Fill(tabela);
            return tabela;
        }

        public DataTable Localizar(String valor, String buscapor, int pageNumber, int RowsPage, string ordenapor)
        {
            String where = "nome_fantasia";
            if (buscapor == "Razão Social")
            {
                where = "razao_social";
            }
            else if (buscapor == "CNPJ")
            {
                where = "cnpj";
            }
            else
            {
                where = "nome_fantasia";
            }

            String order = "nome_fantasia";
            if (ordenapor == "Código")
            {
                order = "idempresas";
            }
            else if (ordenapor == "Razão Social")
            {
                order = "razao_social";
            }
            else if (ordenapor == "CNPJ")
            {
                order = "cnpj";
            }
            else
            {
                order = "nome_fantasia";
            }
            DataTable tabela = new DataTable();

            string sql = "SELECT * FROM ( " +
                            "SELECT ROW_NUMBER() OVER(ORDER BY "+where+") as number, idempresas,razao_social,nome_fantasia,cnpj " +
                            "from empresas where " + where + " like '%" + valor + "%'" +
	                        ") as tbl " +
                          "where " + where + " like '%" + valor + "%' and number between(("+pageNumber+" - 1) * "+RowsPage+" + 1) and("+pageNumber+" * "+RowsPage+") " +
                          "order by " + order;
            SqlDataAdapter da = new SqlDataAdapter(sql, conexao.StringConexao);
            da.Fill(tabela);
            return tabela;
        }

        public int LocalizarEnd(int codigo)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conexao.ObjetoConexao;
            cmd.CommandText = "select idenderecos from empresas where idempresas=" + codigo.ToString();
            conexao.Conectar();
            SqlDataReader registro = cmd.ExecuteReader();
            int idenderecos = 0;
            if (registro.HasRows)
            {
                registro.Read();
                idenderecos = Convert.ToInt32(registro["idenderecos"]);
            }
            conexao.Desconectar();
            return idenderecos;
        }

        public int TotalEmpresas()
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conexao.ObjetoConexao;
            cmd.CommandText = "select count(idempresas) as quant from empresas";
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

        public int VerificaCNPJ(String valor, int id)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conexao.ObjetoConexao;
            cmd.CommandText = "select cnpj from empresas where cnpj like '" + valor.ToString() + "' and idempresas<>" + id.ToString();
            conexao.Conectar();
            SqlDataReader registro = cmd.ExecuteReader();
            int cnpj = 0;
            if (registro.HasRows)
            {
                registro.Read();
                cnpj = 1;
            }
            conexao.Desconectar();
            return cnpj;
        }

        public ModeloEmpresa CarregaEmpresa(int codigo)
        {
            ModeloEmpresa modelo = new ModeloEmpresa();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conexao.ObjetoConexao;
            cmd.CommandText = "select * from empresas where idempresas=" + codigo.ToString();
            conexao.Conectar();
            SqlDataReader registro = cmd.ExecuteReader();

            if (registro.HasRows)
            {
                registro.Read();
                modelo.IdEmpresas = ConverteReader.ConverteInt(registro["idempresas"]);
                modelo.IdEnderecos = ConverteReader.ConverteInt(registro["idenderecos"]);
                modelo.Razao_Social = ConverteReader.ConverteString(registro["razao_social"]);
                modelo.Nome_Fantasia = ConverteReader.ConverteString(registro["nome_fantasia"]);
                modelo.CNPJ = ConverteReader.ConverteString(registro["cnpj"]);
                modelo.Insc_Est = ConverteReader.ConverteString(registro["insc_est"]);
                modelo.Email = ConverteReader.ConverteString(registro["email"]);
                modelo.Responsavel = ConverteReader.ConverteString(registro["responsavel"]);
                modelo.Ramo_Atividade = ConverteReader.ConverteString(registro["ramo_atividade"]);
                modelo.Telefone1 = ConverteReader.ConverteString(registro["telefone1"]);
                modelo.Telefone2 = ConverteReader.ConverteString(registro["telefone2"]);
                if (registro["logo"].ToString() != "")
                {
                    modelo.Logo = (Byte[])registro["logo"];
                    modelo.Nome_Arquivo = ConverteReader.ConverteString(registro["nome_arquivo"]);
                }
                
                conexao.Desconectar();

                DALEndereco DALObj = new DALEndereco(conexao);
                modelo.Endereco = DALObj.CarregaEndereco(modelo.IdEnderecos);

            }
            else
            {
                conexao.Desconectar();
            }

            return modelo;
        }

        
    }
}
