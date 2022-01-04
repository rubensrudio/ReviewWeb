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
    public class DALCliente
    {
        private DALConexao conexao;
        public DALCliente(DALConexao cx)
        {
            this.conexao = cx;
        }

        public void Incluir(ModeloCliente modelo, ModeloEndereco modeloEnd)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conexao.ObjetoConexao;
            cmd.CommandText = "insert into clientes (idempresas,idenderecos,razao_social,nome_fantasia,cnpj,insc_est,email,responsavel,ramo_atividade,telefone1,telefone2,clientefornecedor) " +
                "values (@idempresas,@idenderecos,@razao_social,@nome_fantasia,@cnpj,@insc_est,@email,@responsavel,@ramo_atividade,@telefone1,@telefone2,@clientefornecedor); select @@IDENTITY;";
            cmd.Parameters.AddWithValue("@idempresas", ConverteReader.ConverteInt(modelo.IdEmpresas));
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
            cmd.Parameters.AddWithValue("@clientefornecedor", ConverteReader.ConverteString(modelo.ClienteFornecedor));

            conexao.Conectar();
            modelo.IdClientes = Convert.ToInt32(cmd.ExecuteScalar());
            conexao.Desconectar();
        }

        public void Alterar(ModeloCliente modelo)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conexao.ObjetoConexao;
            cmd.CommandText = "update clientes set razao_social=@razao_social,nome_fantasia=@nome_fantasia,cnpj=@cnpj,insc_est=@insc_est,email=@email,responsavel=@responsavel,ramo_atividade=@ramo_atividade,telefone1=@telefone1,telefone2=@telefone2,clientefornecedor=@clientefornecedor " +
                "where idclientes=@idclientes;";
            cmd.Parameters.AddWithValue("@idclientes", modelo.IdClientes);
            cmd.Parameters.AddWithValue("@razao_social", ConverteReader.ConverteString(modelo.Razao_Social));
            cmd.Parameters.AddWithValue("@nome_fantasia", ConverteReader.ConverteString(modelo.Nome_Fantasia));
            cmd.Parameters.AddWithValue("@cnpj", ConverteReader.ConverteString(modelo.CNPJ));
            cmd.Parameters.AddWithValue("@insc_est", ConverteReader.ConverteString(modelo.Insc_Est));
            cmd.Parameters.AddWithValue("@email", ConverteReader.ConverteString(modelo.Email));
            cmd.Parameters.AddWithValue("@responsavel", ConverteReader.ConverteString(modelo.Responsavel));
            cmd.Parameters.AddWithValue("@ramo_atividade", ConverteReader.ConverteString(modelo.Ramo_Atividade));
            cmd.Parameters.AddWithValue("@telefone1", ConverteReader.ConverteString(modelo.Telefone1));
            cmd.Parameters.AddWithValue("@telefone2", ConverteReader.ConverteString(modelo.Telefone2));
            cmd.Parameters.AddWithValue("@clientefornecedor", ConverteReader.ConverteString(modelo.ClienteFornecedor));

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
            cmd.CommandText = "delete from clientes where idclientes=@idclientes;";
            cmd.Parameters.AddWithValue("@idclientes", codigo);

            conexao.Conectar();
            cmd.ExecuteNonQuery();
            conexao.Desconectar();

            //Excluir Endereço
            DALEndereco DALObj = new DALEndereco(this.conexao);
            DALObj.Excluir(idenderecos);
        }

        public DataTable Localizar(String valor, String buscapor, int idempresas)
        {
            String where = "nome_fantasia";
            if (buscapor == "Razão Social")
            {
                where = "razao_social";
            }
            else if (buscapor == "Nome Fantasia")
            {
                where = "nome_fantasia";
            }
            else
            {
                where = "CNPJ";
            }
            DataTable tabela = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter("select idclientes,razao_social,nome_fantasia,cnpj from clientes where idempresas=" + idempresas + " and " + where + " like '%" + valor + "%' order by " + where, conexao.StringConexao);
            da.Fill(tabela);
            return tabela;
        }

        public DataTable Localizar(String valor, String buscapor, int idempresas, String clifor, int pageNumber, int RowsPage, string ordenapor)
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

            String where1;
            if (clifor == "cl") {
                where1 = " and (clientefornecedor='cl' or clientefornecedor='cf') and ";
            }
            else
            {
                where1 = " and (clientefornecedor='fo' or clientefornecedor='cf') and ";
            }
            DataTable tabela = new DataTable();
            //String sql = "select idclientes,idenderecos,razao_social,nome_fantasia,cnpj from clientes where idempresas=" + idempresas.ToString() + where1 + where + " like '%" + valor + "%' order by " + where;
            string sql = "SELECT * FROM ( " +
                            "SELECT ROW_NUMBER() OVER(ORDER BY " + where + ") as number, idclientes,idempresas,razao_social,nome_fantasia,cnpj,clientefornecedor " +
                            "from clientes where idempresas=" + idempresas.ToString() + where1 + where + " like '%" + valor + "%'" +
                            ") as tbl " +
                          "where idempresas=" + idempresas.ToString() + where1 + where + " like '%" + valor + "%' and number between((" + pageNumber + " - 1) * " + RowsPage + " + 1) and(" + pageNumber + " * " + RowsPage + ") " +
                          "order by " + order;
            SqlDataAdapter da = new SqlDataAdapter(sql, conexao.StringConexao);
            da.Fill(tabela);
            return tabela;
        }

        public int LocalizarEnd(int codigo)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conexao.ObjetoConexao;
            cmd.CommandText = "select idenderecos from clientes where idclientes=" + codigo.ToString();
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

        public int TotalClientes()
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conexao.ObjetoConexao;
            cmd.CommandText = "select count(idclientes) as quant from clientes";
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

        public int VerificaCNPJ(String valor, int idempresas)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conexao.ObjetoConexao;
            cmd.CommandText = "select cnpj from clientes where cnpj like '" + valor.ToString() + "' and idempresas="+idempresas;
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

        public ModeloCliente CarregaCliente(int codigo)
        {
            ModeloCliente modelo = new ModeloCliente();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conexao.ObjetoConexao;
            cmd.CommandText = "select * from clientes where idclientes=" + codigo.ToString();
            conexao.Conectar();
            SqlDataReader registro = cmd.ExecuteReader();
            if (registro.HasRows)
            {
                registro.Read();
                modelo.IdClientes = ConverteReader.ConverteInt(registro["idclientes"]);
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
                modelo.ClienteFornecedor = ConverteReader.ConverteString(registro["clientefornecedor"]);

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
