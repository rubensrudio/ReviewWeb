using Modelo;
using System;
using System.Data.SqlClient;

namespace DAL
{
    public class DALEndereco
    {
        private DALConexao conexao;
        public DALEndereco(DALConexao cx)
        {
            this.conexao = cx;
        }

        public void Incluir(ModeloEndereco modelo)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conexao.ObjetoConexao;
            cmd.CommandText = "insert into enderecos (logradouro, bairro, cidade, uf, cep) " +
                "values (@logradouro, @bairro, @cidade, @uf, @cep); select @@IDENTITY;";
            cmd.Parameters.AddWithValue("@logradouro", modelo.Logradouro);
            cmd.Parameters.AddWithValue("@bairro", modelo.Bairro);
            cmd.Parameters.AddWithValue("@cidade", modelo.Cidade);
            cmd.Parameters.AddWithValue("@uf", modelo.UF);
            cmd.Parameters.AddWithValue("@cep", modelo.CEP);
            
            conexao.Conectar();
            modelo.IdEnderecos = Convert.ToInt32(cmd.ExecuteScalar());
            conexao.Desconectar();
        }

        public void Alterar(ModeloEndereco modelo)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conexao.ObjetoConexao;
            cmd.CommandText = "update enderecos set logradouro=@logradouro, bairro=@bairro, cidade=@cidade, uf=@uf, cep=@cep " +
                "where idenderecos=@idenderecos";
            cmd.Parameters.AddWithValue("@idenderecos", modelo.IdEnderecos);
            cmd.Parameters.AddWithValue("@logradouro", modelo.Logradouro);
            cmd.Parameters.AddWithValue("@bairro", modelo.Bairro);
            cmd.Parameters.AddWithValue("@cidade", modelo.Cidade);
            cmd.Parameters.AddWithValue("@uf", modelo.UF);
            cmd.Parameters.AddWithValue("@cep", modelo.CEP);

            conexao.Conectar();
            cmd.ExecuteNonQuery();
            conexao.Desconectar();
        }

        public void Excluir(int codigo)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conexao.ObjetoConexao;
            cmd.CommandText = "delete from enderecos where idenderecos="+codigo.ToString();
            
            conexao.Conectar();
            cmd.ExecuteNonQuery();
            conexao.Desconectar();
        }
        
        public ModeloEndereco CarregaEndereco(int codigo)
        {
            ModeloEndereco modelo = new ModeloEndereco();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conexao.ObjetoConexao;
            cmd.CommandText = "select * from enderecos where idenderecos=" + codigo.ToString();
            conexao.Conectar();
            SqlDataReader registro = cmd.ExecuteReader();
            if (registro.HasRows)
            {
                registro.Read();
                modelo.IdEnderecos = ConverteReader.ConverteInt(registro["idenderecos"]);
                modelo.Logradouro = ConverteReader.ConverteString(registro["logradouro"]);
                modelo.Bairro = ConverteReader.ConverteString(registro["bairro"]);
                modelo.Cidade = ConverteReader.ConverteString(registro["cidade"]);
                modelo.UF = ConverteReader.ConverteString(registro["uf"]);
                modelo.CEP = ConverteReader.ConverteString(registro["cep"]);
            }
            conexao.Desconectar();
            return modelo;
        }
    }
}
