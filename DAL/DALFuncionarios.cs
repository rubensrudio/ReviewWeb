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
    public class DALFuncionarios
    {
        private DALConexao conexao;
        public DALFuncionarios(DALConexao cx)
        {
            this.conexao = cx;
        }

        public void Incluir(ModeloFuncionarios modelo, ModeloEndereco modeloEnd)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conexao.ObjetoConexao;
            cmd.CommandText = "insert into funcionarios (idenderecos,idempresas,idcargos,matricula,nome,sobrenome,cpf,cart_trabalho,serie_cart_trab,dt_emissao_cart_trab, " +
                "rg,emissor_rg,dt_emissao_rg,titulo_eleitor,zona_eleitor,secao_eleitor,titulo_reservista,pis,cnh,categoria_cnh,dt_vencimento_cnh,conta_bancaria,agencia_bancaria, " +
                "banco,telefone1,telefone2,e_mail,dt_nascimento,local_nascimento,sexo,estado_civil,tipo_sanguineo,escolaridade,pai,mae,dt_admissao,dt_desligamento,ativo,salario_base,uf_contratacao,viagem_dias_trab,viagem_dias_folga,aloc_uf,aloc_cliente,aloc_contrato) " +
                "values (@idenderecos,@idempresas,@idcargos,@matricula,@nome,@sobrenome,@cpf,@cart_trabalho,@serie_cart_trab,@dt_emissao_cart_trab, " +
                "@rg,@emissor_rg,@dt_emissao_rg,@titulo_eleitor,@zona_eleitor,@secao_eleitor,@titulo_reservista,@pis,@cnh,@categoria_cnh,@dt_vencimento_cnh,@conta_bancaria,@agencia_bancaria, " +
                "@banco,@telefone1,@telefone2,@e_mail,@dt_nascimento,@local_nascimento,@sexo,@estado_civil,@tipo_sanguineo,@escolaridade,@pai,@mae,@dt_admissao,@dt_desligamento,@ativo,@salario_base,@uf_contratacao,@viagem_dias_trab,@viagem_dias_folga,@aloc_uf,@aloc_cliente,@aloc_contrato); select @@IDENTITY;";
            cmd.Parameters.AddWithValue("@idenderecos", ConverteReader.ConverteInt(modeloEnd.IdEnderecos));
            cmd.Parameters.AddWithValue("@idempresas", ConverteReader.ConverteInt(modelo.IdEmpresas));
            cmd.Parameters.AddWithValue("@idcargos", ConverteReader.ConverteInt(modelo.IdCargos));
            cmd.Parameters.AddWithValue("@matricula", ConverteReader.ConverteInt(modelo.Matricula));
            cmd.Parameters.AddWithValue("@nome", ConverteReader.ConverteString(modelo.Nome));
            cmd.Parameters.AddWithValue("@sobrenome", ConverteReader.ConverteString(modelo.Sobrenome));
            //cmd.Parameters.AddWithValue("@foto", modelo.Foto);
            //cmd.Parameters.AddWithValue("@nome_arquivo", ConverteReader.ConverteString(modelo.Nome_Arquivo));
            cmd.Parameters.AddWithValue("@cpf", ConverteReader.ConverteString(modelo.CPF));
            cmd.Parameters.AddWithValue("@cart_trabalho", ConverteReader.ConverteString(modelo.Cart_Trabalho));
            cmd.Parameters.AddWithValue("@serie_cart_trab", ConverteReader.ConverteString(modelo.Serie_Cart_Trab));
            if ((modelo.Dt_Emissao_Cart_Trab != null) && (modelo.Dt_Emissao_Cart_Trab.ToString() != "01/01/0001 00:00:00"))
            {
                cmd.Parameters.Add("@dt_emissao_cart_trab", System.Data.SqlDbType.DateTime);
                cmd.Parameters["@dt_emissao_cart_trab"].Value = modelo.Dt_Emissao_Cart_Trab;
            }
            else
            {
                cmd.Parameters.Add("@dt_emissao_cart_trab", System.Data.SqlDbType.DateTime);
                cmd.Parameters["@dt_emissao_cart_trab"].Value = SqlDateTime.Null;
            }
            cmd.Parameters.AddWithValue("@rg", ConverteReader.ConverteString(modelo.RG));
            cmd.Parameters.AddWithValue("@emissor_rg", ConverteReader.ConverteString(modelo.Emissor_RG));
            if ((modelo.Dt_Emissao_RG != null) && (modelo.Dt_Emissao_RG.ToString() != "01/01/0001 00:00:00"))
            {
                cmd.Parameters.Add("@dt_emissao_rg", System.Data.SqlDbType.DateTime);
                cmd.Parameters["@dt_emissao_rg"].Value = modelo.Dt_Emissao_RG;
            }
            else
            {
                cmd.Parameters.Add("@dt_emissao_rg", System.Data.SqlDbType.DateTime);
                cmd.Parameters["@dt_emissao_rg"].Value = SqlDateTime.Null;
            }
            cmd.Parameters.AddWithValue("@titulo_eleitor", ConverteReader.ConverteString(modelo.Titulo_Eleitor));
            cmd.Parameters.AddWithValue("@zona_eleitor", ConverteReader.ConverteString(modelo.Zona_Eleitor));
            cmd.Parameters.AddWithValue("@secao_eleitor", ConverteReader.ConverteString(modelo.Secao_Eleitor));
            cmd.Parameters.AddWithValue("@titulo_reservista", ConverteReader.ConverteString(modelo.Titulo_Reservista));
            cmd.Parameters.AddWithValue("@pis", ConverteReader.ConverteString(modelo.PIS));
            cmd.Parameters.AddWithValue("@cnh", ConverteReader.ConverteString(modelo.CNH));
            cmd.Parameters.AddWithValue("@categoria_cnh", ConverteReader.ConverteString(modelo.Categoria_CNH));
            if ((modelo.Dt_Vencimento_CNH != null) && (modelo.Dt_Vencimento_CNH.ToString() != "01/01/0001 00:00:00"))
            {
                cmd.Parameters.Add("@dt_vencimento_cnh", System.Data.SqlDbType.DateTime);
                cmd.Parameters["@dt_vencimento_cnh"].Value = modelo.Dt_Vencimento_CNH;
            }
            else
            {
                cmd.Parameters.Add("@dt_vencimento_cnh", System.Data.SqlDbType.DateTime);
                cmd.Parameters["@dt_vencimento_cnh"].Value = SqlDateTime.Null;
            }
            cmd.Parameters.AddWithValue("@conta_bancaria", ConverteReader.ConverteString(modelo.Conta_Bancaria));
            cmd.Parameters.AddWithValue("@agencia_bancaria", ConverteReader.ConverteString(modelo.Agencia_Bancaria));
            cmd.Parameters.AddWithValue("@banco", ConverteReader.ConverteString(modelo.Banco));
            cmd.Parameters.AddWithValue("@telefone1", ConverteReader.ConverteString(modelo.Telefone1));
            cmd.Parameters.AddWithValue("@telefone2", ConverteReader.ConverteString(modelo.Telefone2));
            cmd.Parameters.AddWithValue("@e_mail", ConverteReader.ConverteString(modelo.E_Mail));
            if ((modelo.Dt_Nascimento != null) && (modelo.Dt_Nascimento.ToString() != "01/01/0001 00:00:00"))
            {
                cmd.Parameters.Add("@dt_nascimento", System.Data.SqlDbType.DateTime);
                cmd.Parameters["@dt_nascimento"].Value = modelo.Dt_Nascimento;
            }
            else
            {
                cmd.Parameters.Add("@dt_nascimento", System.Data.SqlDbType.DateTime);
                cmd.Parameters["@dt_nascimento"].Value = SqlDateTime.Null;
            }
            cmd.Parameters.AddWithValue("@local_nascimento", ConverteReader.ConverteString(modelo.Local_Nascimento));
            cmd.Parameters.AddWithValue("@sexo", ConverteReader.ConverteString(modelo.Sexo));
            cmd.Parameters.AddWithValue("@estado_civil", ConverteReader.ConverteString(modelo.Estado_Civil));
            cmd.Parameters.AddWithValue("@tipo_sanguineo", ConverteReader.ConverteString(modelo.Tipo_Sanguineo));
            cmd.Parameters.AddWithValue("@escolaridade", ConverteReader.ConverteString(modelo.Escolaridade));
            cmd.Parameters.AddWithValue("@pai", ConverteReader.ConverteString(modelo.Pai));
            cmd.Parameters.AddWithValue("@mae", ConverteReader.ConverteString(modelo.Mae));
            if ((modelo.Dt_Admissao != null) && (modelo.Dt_Admissao.ToString() != "01/01/0001 00:00:00"))
            {
                cmd.Parameters.Add("@dt_admissao", System.Data.SqlDbType.DateTime);
                cmd.Parameters["@dt_admissao"].Value = modelo.Dt_Admissao;
            }
            else
            {
                cmd.Parameters.Add("@dt_admissao", System.Data.SqlDbType.DateTime);
                cmd.Parameters["@dt_admissao"].Value = SqlDateTime.Null;
            }
            if ((modelo.Dt_Desligamento != null) && (modelo.Dt_Desligamento.ToString() != "01/01/0001 00:00:00"))
            {
                cmd.Parameters.Add("@dt_desligamento", System.Data.SqlDbType.DateTime);
                cmd.Parameters["@dt_desligamento"].Value = modelo.Dt_Desligamento;
            }
            else
            {
                cmd.Parameters.Add("@dt_desligamento", System.Data.SqlDbType.DateTime);
                cmd.Parameters["@dt_desligamento"].Value = SqlDateTime.Null;
            }
            cmd.Parameters.AddWithValue("@ativo", ConverteReader.ConverteInt(modelo.Ativo));
            cmd.Parameters.AddWithValue("@salario_base", ConverteReader.ConverteDouble(modelo.Salario_Base));
            cmd.Parameters.AddWithValue("@uf_contratacao", ConverteReader.ConverteString(modelo.UF_Contratacao));
            cmd.Parameters.AddWithValue("@viagem_dias_trab", ConverteReader.ConverteInt(modelo.Viagem_Dias_Trab));
            cmd.Parameters.AddWithValue("@viagem_dias_folga", ConverteReader.ConverteInt(modelo.Viagem_Dias_Folga));
            cmd.Parameters.AddWithValue("@aloc_uf", ConverteReader.ConverteString(""));
            cmd.Parameters.AddWithValue("@aloc_cliente", ConverteReader.ConverteInt(0));
            cmd.Parameters.AddWithValue("@aloc_contrato", ConverteReader.ConverteInt(0));

            conexao.Conectar();
            modelo.IdFuncionarios = Convert.ToInt32(cmd.ExecuteScalar());
            conexao.Desconectar();
        }

        public void Alterar(ModeloFuncionarios modelo)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conexao.ObjetoConexao;
            cmd.CommandText = "update funcionarios set idcargos=@idcargos,matricula=@matricula,nome=@nome,sobrenome=@sobrenome,cpf=@cpf,cart_trabalho=@cart_trabalho,serie_cart_trab=@serie_cart_trab,dt_emissao_cart_trab=@dt_emissao_cart_trab, " +
                "rg=@rg,emissor_rg=@emissor_rg,dt_emissao_rg=@dt_emissao_rg,titulo_eleitor=@titulo_eleitor,zona_eleitor=@zona_eleitor,secao_eleitor=@secao_eleitor,titulo_reservista=@titulo_reservista,pis=@pis,cnh=@cnh,categoria_cnh=@categoria_cnh,dt_vencimento_cnh=@dt_vencimento_cnh,conta_bancaria=@conta_bancaria,agencia_bancaria=@agencia_bancaria, " +
                "banco=@banco,telefone1=@telefone1,telefone2=@telefone2,e_mail=@e_mail,dt_nascimento=@dt_nascimento,local_nascimento=@local_nascimento,sexo=@sexo,estado_civil=@estado_civil,tipo_sanguineo=@tipo_sanguineo,escolaridade=@escolaridade,pai=@pai,mae=@mae,dt_admissao=@dt_admissao,dt_desligamento=@dt_desligamento,ativo=@ativo,salario_base=@salario_base,uf_contratacao=@uf_contratacao,viagem_dias_trab=@viagem_dias_trab,viagem_dias_folga=@viagem_dias_folga " +
                "where idfuncionarios=@idfuncionarios;";
            cmd.Parameters.AddWithValue("@idfuncionarios", ConverteReader.ConverteInt(modelo.IdFuncionarios));
            cmd.Parameters.AddWithValue("@idcargos", ConverteReader.ConverteInt(modelo.IdCargos));
            cmd.Parameters.AddWithValue("@matricula", ConverteReader.ConverteInt(modelo.Matricula));
            cmd.Parameters.AddWithValue("@nome", ConverteReader.ConverteString(modelo.Nome));
            cmd.Parameters.AddWithValue("@sobrenome", ConverteReader.ConverteString(modelo.Sobrenome));
            //cmd.Parameters.AddWithValue("@foto", modelo.Foto);
            //cmd.Parameters.AddWithValue("@nome_arquivo", ConverteReader.ConverteString(modelo.Nome_Arquivo));
            cmd.Parameters.AddWithValue("@cpf", ConverteReader.ConverteString(modelo.CPF));
            cmd.Parameters.AddWithValue("@cart_trabalho", ConverteReader.ConverteString(modelo.Cart_Trabalho));
            cmd.Parameters.AddWithValue("@serie_cart_trab", ConverteReader.ConverteString(modelo.Serie_Cart_Trab));
            if ((modelo.Dt_Emissao_Cart_Trab != null) && (modelo.Dt_Emissao_Cart_Trab.ToString() != "01/01/0001 00:00:00"))
            {
                cmd.Parameters.Add("@dt_emissao_cart_trab", System.Data.SqlDbType.DateTime);
                cmd.Parameters["@dt_emissao_cart_trab"].Value = modelo.Dt_Emissao_Cart_Trab;
            }
            else
            {
                cmd.Parameters.Add("@dt_emissao_cart_trab", System.Data.SqlDbType.DateTime);
                cmd.Parameters["@dt_emissao_cart_trab"].Value = SqlDateTime.Null;
            }
            cmd.Parameters.AddWithValue("@rg", ConverteReader.ConverteString(modelo.RG));
            cmd.Parameters.AddWithValue("@emissor_rg", ConverteReader.ConverteString(modelo.Emissor_RG));
            if ((modelo.Dt_Emissao_RG != null) && (modelo.Dt_Emissao_RG.ToString() != "01/01/0001 00:00:00"))
            {
                cmd.Parameters.Add("@dt_emissao_rg", System.Data.SqlDbType.DateTime);
                cmd.Parameters["@dt_emissao_rg"].Value = modelo.Dt_Emissao_RG;
            }
            else
            {
                cmd.Parameters.Add("@dt_emissao_rg", System.Data.SqlDbType.DateTime);
                cmd.Parameters["@dt_emissao_rg"].Value = SqlDateTime.Null;
            }
            cmd.Parameters.AddWithValue("@titulo_eleitor", ConverteReader.ConverteString(modelo.Titulo_Eleitor));
            cmd.Parameters.AddWithValue("@zona_eleitor", ConverteReader.ConverteString(modelo.Zona_Eleitor));
            cmd.Parameters.AddWithValue("@secao_eleitor", ConverteReader.ConverteString(modelo.Secao_Eleitor));
            cmd.Parameters.AddWithValue("@titulo_reservista", ConverteReader.ConverteString(modelo.Titulo_Reservista));
            cmd.Parameters.AddWithValue("@pis", ConverteReader.ConverteString(modelo.PIS));
            cmd.Parameters.AddWithValue("@cnh", ConverteReader.ConverteString(modelo.CNH));
            cmd.Parameters.AddWithValue("@categoria_cnh", ConverteReader.ConverteString(modelo.Categoria_CNH));
            if ((modelo.Dt_Vencimento_CNH != null) && (modelo.Dt_Vencimento_CNH.ToString() != "01/01/0001 00:00:00"))
            {
                cmd.Parameters.Add("@dt_vencimento_cnh", System.Data.SqlDbType.DateTime);
                cmd.Parameters["@dt_vencimento_cnh"].Value = modelo.Dt_Vencimento_CNH;
            }
            else
            {
                cmd.Parameters.Add("@dt_vencimento_cnh", System.Data.SqlDbType.DateTime);
                cmd.Parameters["@dt_vencimento_cnh"].Value = SqlDateTime.Null;
            }
            cmd.Parameters.AddWithValue("@conta_bancaria", ConverteReader.ConverteString(modelo.Conta_Bancaria));
            cmd.Parameters.AddWithValue("@agencia_bancaria", ConverteReader.ConverteString(modelo.Agencia_Bancaria));
            cmd.Parameters.AddWithValue("@banco", ConverteReader.ConverteString(modelo.Banco));
            cmd.Parameters.AddWithValue("@telefone1", ConverteReader.ConverteString(modelo.Telefone1));
            cmd.Parameters.AddWithValue("@telefone2", ConverteReader.ConverteString(modelo.Telefone2));
            cmd.Parameters.AddWithValue("@e_mail", ConverteReader.ConverteString(modelo.E_Mail));
            if ((modelo.Dt_Nascimento != null) && (modelo.Dt_Nascimento.ToString() != "01/01/0001 00:00:00"))
            {
                cmd.Parameters.Add("@dt_nascimento", System.Data.SqlDbType.DateTime);
                cmd.Parameters["@dt_nascimento"].Value = modelo.Dt_Nascimento;
            }
            else
            {
                cmd.Parameters.Add("@dt_nascimento", System.Data.SqlDbType.DateTime);
                cmd.Parameters["@dt_nascimento"].Value = SqlDateTime.Null;
            }
            cmd.Parameters.AddWithValue("@local_nascimento", ConverteReader.ConverteString(modelo.Local_Nascimento));
            cmd.Parameters.AddWithValue("@sexo", ConverteReader.ConverteString(modelo.Sexo));
            cmd.Parameters.AddWithValue("@estado_civil", ConverteReader.ConverteString(modelo.Estado_Civil));
            cmd.Parameters.AddWithValue("@tipo_sanguineo", ConverteReader.ConverteString(modelo.Tipo_Sanguineo));
            cmd.Parameters.AddWithValue("@escolaridade", ConverteReader.ConverteString(modelo.Escolaridade));
            cmd.Parameters.AddWithValue("@pai", ConverteReader.ConverteString(modelo.Pai));
            cmd.Parameters.AddWithValue("@mae", ConverteReader.ConverteString(modelo.Mae));
            if ((modelo.Dt_Admissao != null) && (modelo.Dt_Admissao.ToString() != "01/01/0001 00:00:00"))
            {
                cmd.Parameters.Add("@dt_admissao", System.Data.SqlDbType.DateTime);
                cmd.Parameters["@dt_admissao"].Value = modelo.Dt_Admissao;
            }
            else
            {
                cmd.Parameters.Add("@dt_admissao", System.Data.SqlDbType.DateTime);
                cmd.Parameters["@dt_admissao"].Value = SqlDateTime.Null;
            }
            if ((modelo.Dt_Desligamento != null) && (modelo.Dt_Desligamento.ToString() != "01/01/0001 00:00:00"))
            {
                cmd.Parameters.Add("@dt_desligamento", System.Data.SqlDbType.DateTime);
                cmd.Parameters["@dt_desligamento"].Value = modelo.Dt_Desligamento;
            }
            else
            {
                cmd.Parameters.Add("@dt_desligamento", System.Data.SqlDbType.DateTime);
                cmd.Parameters["@dt_desligamento"].Value = SqlDateTime.Null;
            }
            cmd.Parameters.AddWithValue("@ativo", ConverteReader.ConverteInt(modelo.Ativo));
            cmd.Parameters.AddWithValue("@salario_base", ConverteReader.ConverteDouble(modelo.Salario_Base));
            cmd.Parameters.AddWithValue("@uf_contratacao", ConverteReader.ConverteString(modelo.UF_Contratacao));
            cmd.Parameters.AddWithValue("@viagem_dias_trab", ConverteReader.ConverteInt(modelo.Viagem_Dias_Trab));
            cmd.Parameters.AddWithValue("@viagem_dias_folga", ConverteReader.ConverteInt(modelo.Viagem_Dias_Folga));

            conexao.Conectar();
            cmd.ExecuteNonQuery();
            conexao.Desconectar();
        }

        public void Excluir(int codigo)
        {
            int idenderecos = LocalizarEnd(codigo);

            try
            {
                //Excluir Alocações
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conexao.ObjetoConexao;
                cmd.CommandText = "delete from alocacao_func where idfuncionarios=@idfuncionarios;";
                cmd.Parameters.AddWithValue("@idfuncionarios", codigo);

                conexao.Conectar();
                cmd.ExecuteNonQuery();
                conexao.Desconectar();

                //Excluir Exames
                SqlCommand cmd3 = new SqlCommand();
                cmd3.Connection = conexao.ObjetoConexao;
                cmd3.CommandText = "delete from exames where idfuncionarios=@idfuncionarios;";
                cmd3.Parameters.AddWithValue("@idfuncionarios", codigo);

                conexao.Conectar();
                cmd3.ExecuteNonQuery();
                conexao.Desconectar();

                //Excluir Exames
                SqlCommand cmd4 = new SqlCommand();
                cmd4.Connection = conexao.ObjetoConexao;
                cmd4.CommandText = "delete from treinamentos where idfuncionarios=@idfuncionarios;";
                cmd4.Parameters.AddWithValue("@idfuncionarios", codigo);

                conexao.Conectar();
                cmd4.ExecuteNonQuery();
                conexao.Desconectar();

                //Excluir Empresa
                SqlCommand cmd2 = new SqlCommand();
                cmd2.Connection = conexao.ObjetoConexao;
                cmd2.CommandText = "delete from funcionarios where idfuncionarios=@idfuncionarios;";
                cmd2.Parameters.AddWithValue("@idfuncionarios", codigo);

                conexao.Conectar();
                cmd2.ExecuteNonQuery();
                conexao.Desconectar();

                //Excluir Endereço
                DALEndereco DALObj = new DALEndereco(this.conexao);
                DALObj.Excluir(idenderecos);

            }
            catch (Exception erro)
            {

            }
        }

        public DataTable Localizar(String valor, String buscapor)
        {
            String where = "nome";
            if (buscapor == "Matricula")
            {
                where = "matricula";
            }
            else if (buscapor == "CPF")
            {
                where = "cpf";
            }
            else
            {
                where = "nome";
            }
            DataTable tabela = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter("select idfuncionarios,matricula,nome,sobrenome,cpf from funcionarios where " + where + " like '%" + valor + "%' order by " + where, conexao.StringConexao);
            da.Fill(tabela);
            return tabela;
        }

        public DataTable Localizar(String valor, String buscapor, int idempresas, int pageNumber, int RowsPage, string ordenapor)
        {
            String where = "nome";
            if (buscapor == "Matricula")
            {
                where = "matricula";
            }
            else if (buscapor == "CPF")
            {
                where = "cpf";
            }
            else
            {
                where = "nome";
            }

            String order = "nome";
            if (ordenapor == "Código")
            {
                order = "idfuncionarios";
            }
            else if (ordenapor == "Matrícula")
            {
                order = "matricula";
            }
            else if (ordenapor == "Sobrenome")
            {
                order = "sobrenome";
            }
            else if (ordenapor == "CPF")
            {
                order = "cpf";
            }
            else
            {
                order = "nome";
            }
            DataTable tabela = new DataTable();

            string sql = "SELECT * FROM ( " +
                            "SELECT ROW_NUMBER() OVER(ORDER BY " + where + ") as number, idfuncionarios,idempresas,matricula,nome,sobrenome,cpf " +
                            "from funcionarios where idempresas= " + idempresas + " and " + where + " like '%" + valor + "%'" +
                            ") as tbl " +
                          "where idempresas= " + idempresas + " and " + where + " like '%" + valor + "%' and number between((" + pageNumber + " - 1) * " + RowsPage + " + 1) and(" + pageNumber + " * " + RowsPage + ") " +
                          "order by " + order;
            SqlDataAdapter da = new SqlDataAdapter(sql, conexao.StringConexao);
            da.Fill(tabela);
            return tabela;
        }

        public DataTable LocalizarFuncionarios(int idempresas)
        {
            DataTable tabela = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter("select idfuncionarios,matricula,nome,sobrenome,cpf from funcionarios where idempresas="+idempresas+" order by nome,sobrenome", conexao.StringConexao);
            da.Fill(tabela);
            return tabela;
        }

        public int LocalizarEnd(int codigo)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conexao.ObjetoConexao;
            cmd.CommandText = "select idenderecos from funcionarios where idfuncionarios=" + codigo.ToString();
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

        
        public int TotalFuncionarios()
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conexao.ObjetoConexao;
            cmd.CommandText = "select count(idfuncionarios) as quant from funcionarios";
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

        public int VerificaCPF(String valor, int id)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conexao.ObjetoConexao;
            cmd.CommandText = "select cpf from funcionarios where cpf like '" + valor.ToString() + "' and idempresas<>" + id.ToString();
            conexao.Conectar();
            SqlDataReader registro = cmd.ExecuteReader();
            int cpf = 0;
            if (registro.HasRows)
            {
                registro.Read();
                cpf = 1;
            }
            conexao.Desconectar();
            return cpf;
        }

        public ModeloFuncionarios CarregaFuncionarios(int codigo)
        {
            ModeloFuncionarios modelo = new ModeloFuncionarios();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conexao.ObjetoConexao;
            cmd.CommandText = "select * from funcionarios where idfuncionarios=" + codigo.ToString();
            conexao.Conectar();
            SqlDataReader registro = cmd.ExecuteReader();

            if (registro.HasRows)
            {
                registro.Read();
                modelo.IdFuncionarios = ConverteReader.ConverteInt(registro["idfuncionarios"]);
                modelo.IdEnderecos = ConverteReader.ConverteInt(registro["idenderecos"]);
                modelo.IdCargos = ConverteReader.ConverteInt(registro["idcargos"]);
                modelo.IdEmpresas = ConverteReader.ConverteInt(registro["idempresas"]);
                modelo.Matricula = ConverteReader.ConverteInt(registro["matricula"]);
                modelo.Nome = ConverteReader.ConverteString(registro["nome"]);
                modelo.Sobrenome = ConverteReader.ConverteString(registro["sobrenome"]);
                modelo.CPF = ConverteReader.ConverteString(registro["cpf"]);
                modelo.Cart_Trabalho = ConverteReader.ConverteString(registro["cart_trabalho"]);
                modelo.Serie_Cart_Trab = ConverteReader.ConverteString(registro["serie_cart_trab"]);
                modelo.Dt_Emissao_Cart_Trab = ConverteReader.ConverteDateTime(registro["dt_emissao_cart_trab"]);
                modelo.RG = ConverteReader.ConverteString(registro["rg"]);
                modelo.Emissor_RG = ConverteReader.ConverteString(registro["emissor_rg"]);
                modelo.Dt_Emissao_RG = ConverteReader.ConverteDateTime(registro["dt_emissao_rg"]);
                modelo.Titulo_Eleitor = ConverteReader.ConverteString(registro["titulo_eleitor"]);
                modelo.Zona_Eleitor = ConverteReader.ConverteString(registro["zona_eleitor"]);
                modelo.Secao_Eleitor = ConverteReader.ConverteString(registro["secao_eleitor"]);
                modelo.Titulo_Reservista = ConverteReader.ConverteString(registro["titulo_reservista"]);
                modelo.PIS = ConverteReader.ConverteString(registro["pis"]);
                modelo.CNH = ConverteReader.ConverteString(registro["cnh"]);
                modelo.Categoria_CNH = ConverteReader.ConverteString(registro["categoria_cnh"]);
                modelo.Dt_Vencimento_CNH = ConverteReader.ConverteDateTime(registro["dt_vencimento_cnh"]);
                modelo.Conta_Bancaria = ConverteReader.ConverteString(registro["conta_bancaria"]);
                modelo.Agencia_Bancaria = ConverteReader.ConverteString(registro["agencia_bancaria"]);
                modelo.Banco = ConverteReader.ConverteString(registro["banco"]);
                modelo.Telefone1 = ConverteReader.ConverteString(registro["telefone1"]);
                modelo.Telefone2 = ConverteReader.ConverteString(registro["telefone2"]);
                modelo.E_Mail = ConverteReader.ConverteString(registro["e_mail"]);
                modelo.Dt_Nascimento = ConverteReader.ConverteDateTime(registro["dt_nascimento"]);
                modelo.Local_Nascimento = ConverteReader.ConverteString(registro["local_nascimento"]);
                modelo.Sexo = ConverteReader.ConverteString(registro["sexo"]);
                modelo.Estado_Civil = ConverteReader.ConverteString(registro["estado_civil"]);
                modelo.Tipo_Sanguineo = ConverteReader.ConverteString(registro["tipo_sanguineo"]);
                modelo.Escolaridade = ConverteReader.ConverteString(registro["escolaridade"]);
                modelo.Pai = ConverteReader.ConverteString(registro["pai"]);
                modelo.Mae = ConverteReader.ConverteString(registro["mae"]);
                modelo.Dt_Admissao = ConverteReader.ConverteDateTime(registro["dt_admissao"]);
                modelo.Dt_Desligamento = ConverteReader.ConverteDateTime(registro["dt_desligamento"]);
                modelo.Ativo = ConverteReader.ConverteInt(registro["ativo"]);
                modelo.Salario_Base = ConverteReader.ConverteDecimal(registro["salario_base"]);
                modelo.UF_Contratacao = registro["uf_contratacao"].ToString();
                modelo.Viagem_Dias_Trab = ConverteReader.ConverteInt(registro["viagem_dias_trab"]);
                modelo.Viagem_Dias_Folga = ConverteReader.ConverteInt(registro["viagem_dias_folga"]);

                if (registro["foto"].ToString() != "")
                {
                    modelo.Foto = (Byte[])registro["foto"];
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

        public DataTable CarregaAlocacaoFuncionarios(string inicio, string fim)
        {
            DataTable tabela = new DataTable();

            string sql = "select af.idalocacao_func,f.idfuncionarios, f.nome, f.sobrenome, ct.contrato, ct.descricao, af.horario, af.horario_fim " +
                            "from funcionarios f " +
                                "left join alocacao_func af on f.idfuncionarios = af.idfuncionarios " +
                                "left join contratos ct on ct.idcontratos = af.aloc_contrato " +
                            "where ((horario >= '"+ inicio +" 00:00:00' and horario <= '"+ fim +" 23:59:59') " +
                            "order by f.nome, f.sobrenome, af.idalocacao_func desc";
            SqlDataAdapter da = new SqlDataAdapter(sql, conexao.StringConexao);
            da.Fill(tabela);
            return tabela;
        }

        public DataTable CarregaFuncionarios(string ano, string mes)
        {
            DataTable tabela = new DataTable();

            string sql = "select af.idalocacao_func,f.idfuncionarios, f.nome, f.sobrenome, ct.contrato, ct.descricao, af.horario, af.horario_fim " +
                            "from alocacao_func af " +
                                "join funcionarios f on f.idfuncionarios = af.idfuncionarios "+
                                "join contratos ct on ct.idcontratos = af.aloc_contrato "+
                            "where ((horario >= '01/08/2019 00:00:00' and horario <= '31/08/2019 00:00:00') "+
	                            "or(horario_fim >= '01/08/2019 00:00:00' and horario_fim <= '31/08/2019 00:00:00') "+
                                "or(horario <= '01/08/2019 00:00:00' and horario_fim is null)) "+
	                            "and af.aloc_contrato <> 0 "+
                            "order by af.aloc_contrato, f.nome, f.sobrenome";
            SqlDataAdapter da = new SqlDataAdapter(sql, conexao.StringConexao);
            da.Fill(tabela);
            return tabela;
        }
    }
}
