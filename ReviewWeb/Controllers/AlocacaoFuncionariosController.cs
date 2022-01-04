using BLL;
using DAL;
using Modelo;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ReviewWeb.Controllers
{
    public class AlocacaoFuncionariosController : Controller
    {
        private DALConexao cx = new DALConexao(DadosConexao.StringConexao);
        // GET: AlocacaoFuncionarios
        public ActionResult AlocacaoFuncionariosList()
        {
            if (Convert.ToInt32(Session["idempresas"]) <= 0)
            {
                return RedirectToAction("Index","Home");
            }
            BLLLocaisAtuacao bll = new BLLLocaisAtuacao(cx);
            DataTable dt = bll.CarregarUF(Convert.ToInt32(Session["idempresas"]), Convert.ToInt32(Session["idusuarios"]));
            DataTable dt2 = bll.CarregarClientes(Convert.ToInt32(Session["idempresas"]));
            BLLContratos bll2 = new BLLContratos(cx);
            DataTable dt3 = bll2.CarregaContratosEmpresa(Convert.ToInt32(Session["idempresas"]), DateTime.Now.Date.ToString(), DateTime.Now.Date.ToString());

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cx.ObjetoConexao;
            cmd.CommandText = "select top 1 '1' from alocacao_permissao ap where ap.permissao like 'Todos' and ap.idusuarios=" + Convert.ToInt32(Session["idusuarios"]);
            cx.Conectar();
            SqlDataReader registro = cmd.ExecuteReader();
            if (registro.HasRows)
            {
                ViewBag.Todos = 1;
            }
            else
            {
                ViewBag.Todos = 0;
            }
            cx.Desconectar();

            SqlCommand cmd2 = new SqlCommand();
            cmd2.Connection = cx.ObjetoConexao;
            cmd2.CommandText = "select top 1 '1' from alocacao_permissao ap where ap.permissao like 'View' and ap.idusuarios=" + Convert.ToInt32(Session["idusuarios"]);
            cx.Conectar();
            SqlDataReader registro2 = cmd2.ExecuteReader();
            if (registro2.HasRows)
            {
                ViewBag.Visualizacao = 1;
            }
            else
            {
                ViewBag.Visualizacao = 0;
            }
            cx.Desconectar();

            DataTable tabela = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter("select f.idfuncionarios,af.aloc_uf,af.aloc_cliente,af.aloc_contrato,af.horario,af.situacao,af.observacao, f.nome, f.sobrenome, f.uf_contratacao, f.viagem_dias_trab,f.viagem_dias_folga from alocacao_func af right join funcionarios f on f.idfuncionarios=af.idfuncionarios where f.idempresas = " + Convert.ToInt32(Session["idempresas"]) + " and f.ativo=1 order by f.nome asc,f.sobrenome asc,af.horario desc", cx.StringConexao);
            da.Fill(tabela);
            int idfuncionarios = 0;

            DataTable dt4 = new DataTable();
            dt4.Columns.Add("idfuncionarios", typeof(int));
            dt4.Columns.Add("nome", typeof(string));
            dt4.Columns.Add("sobrenome", typeof(string));
            dt4.Columns.Add("aloc_uf", typeof(string));
            dt4.Columns.Add("aloc_cliente", typeof(int));
            dt4.Columns.Add("aloc_contrato", typeof(int));
            dt4.Columns.Add("horario", typeof(DateTime));
            dt4.Columns.Add("situacao", typeof(string));
            dt4.Columns.Add("observacao", typeof(string));
            dt4.Columns.Add("alerta_amarelo", typeof(int));
            dt4.Columns.Add("alerta_vermelho", typeof(int));
            dt4.Columns.Add("alerta_folga", typeof(int));
            dt4.Columns.Add("quant_dias", typeof(string));

            BLLExames bllexames = new BLLExames(cx);
            BLLTreinamentos blltreinamentos = new BLLTreinamentos(cx);
            BLLAlocacaoFunc bllfolga = new BLLAlocacaoFunc(cx);

            foreach (DataRow row in tabela.Rows)
            {
                int quant = 0;
                int quanttreinamento = 0;
                int alerta_amarelo = 0;
                int alert_vermelho = 0;
                int alert_folga = 0;
                int totalDias = 0;
                int totalDiasLocal = 0;
                int idfolga = 0;
                int idlocal = 0;
                string quant_dias = "";


                if (idfuncionarios != ConverteReader.ConverteInt(row["idfuncionarios"]))
                {
                    quant = bllexames.VencimentoExame(ConverteReader.ConverteInt(row["idfuncionarios"]));
                    quanttreinamento = blltreinamentos.VencimentoTreinamento(ConverteReader.ConverteInt(row["idfuncionarios"]));
                    if (((quant != -999999) && (quant <= 10)) || ((quanttreinamento != -999999) && (quanttreinamento <= 10)))
                    {
                        alert_vermelho = 1;
                    }
                    else if (((quant != -999999) && (quant <= 30)) || ((quanttreinamento != -999999) && (quanttreinamento <= 30)))
                    {
                        alerta_amarelo = 1;
                    }

                    if (row["uf_contratacao"].ToString() != row["aloc_uf"].ToString())
                    {
                        if ((row["aloc_uf"].ToString() == "") && (row["uf_contratacao"].ToString() == "ES"))
                        {
                            //nada!
                        }
                        else
                        {
                            DataTable dtfolga = bllfolga.UltimaFolga(ConverteReader.ConverteInt(row["idfuncionarios"]));
                            foreach (DataRow rowfolga in dtfolga.Rows)
                            {
                                //Verifica a última folga
                                DateTime? inicio = ConverteReader.ConverteDateTime(rowfolga["horario"]);
                                DateTime? fim = ConverteReader.ConverteDateTime(rowfolga["horario_fim"], true);
                                idfolga = Convert.ToInt32(rowfolga["idalocacao_func"]);
                                if (row["aloc_uf"].ToString() == "FO")
                                {
                                    //Verifica dias de folga
                                    TimeSpan datas = DateTime.Now - Convert.ToDateTime(inicio);
                                    totalDias = datas.Days;
                                }
                                else
                                {
                                    //Verifica dias de trabalho
                                    TimeSpan datas = DateTime.Now - Convert.ToDateTime(fim);
                                    totalDias = datas.Days;
                                    
                                }
                            }

                            DataTable dtlocal = bllfolga.UltimaLocal(ConverteReader.ConverteInt(row["idfuncionarios"]));
                            foreach (DataRow rowlocal in dtlocal.Rows)
                            {
                                //Verifica a último registro local
                                DateTime? inicio_local = ConverteReader.ConverteDateTime(rowlocal["horario"]);
                                DateTime? fim_local = ConverteReader.ConverteDateTime(rowlocal["horario_fim"], true);
                                idlocal = Convert.ToInt32(rowlocal["idalocacao_func"]);
                                if (row["aloc_uf"].ToString() != "FO")
                                {
                                    //Verifica dias de trabalho
                                    TimeSpan datas = DateTime.Now - Convert.ToDateTime(fim_local);
                                    totalDiasLocal = datas.Days;
                                }
                            }

                            if (idfolga > idlocal)
                            {
                                quant_dias = totalDias.ToString() + " dias!";
                                if (totalDias >= Convert.ToInt32(row["viagem_dias_trab"]) - 10)
                                {
                                    alert_folga = 1;
                                }
                            }
                            else
                            {
                                quant_dias = totalDiasLocal.ToString() + " dias fora!";
                                if (totalDiasLocal >= Convert.ToInt32(row["viagem_dias_trab"]) - 10)
                                {
                                    alert_folga = 1;
                                }
                            }
                        }
                    }

                    
                    dt4.Rows.Add(ConverteReader.ConverteInt(row["idfuncionarios"]), row["nome"].ToString(), row["sobrenome"].ToString(), row["aloc_uf"].ToString(), ConverteReader.ConverteInt(row["aloc_cliente"]), ConverteReader.ConverteInt(row["aloc_contrato"]), ConverteReader.ConverteDateTime(row["horario"]), row["situacao"].ToString(), row["observacao"].ToString(), alerta_amarelo, alert_vermelho,alert_folga,quant_dias);
                    idfuncionarios = ConverteReader.ConverteInt(row["idfuncionarios"]);
                }

            }

            ViewBag.UF = dt.Rows;

            ViewBag.Clientes = dt2.Rows;

            ViewBag.Contratos = dt3.Rows;

            ViewBag.Pessoas = dt4.Rows;

            return View();
        }

        public ActionResult AlocacaoDados(string id)
        {
            string[] str = id.Split('_');
            string uf = "";
            int idclientes = 0;
            int contrato = 0;
            int idfuncionarios = 0;

            if (str.Length == 3)
            {
                uf = str[0];
                idclientes = Convert.ToInt32(str[1]);
                contrato = Convert.ToInt32(str[2]);
            }
            else if (str.Length == 2)
            {
                uf = str[0];
                idclientes = Convert.ToInt32(str[1]);
                contrato = 0;
            }
            else if (str.Length == 1)
            {
                if (str[0] == "SPEED")
                {
                    uf = "";
                    idclientes = 0;
                    contrato = 0;
                }
                else if (int.TryParse(str[0], out idfuncionarios))
                {
                    uf = "";
                    idclientes = 0;
                    contrato = 0;
                }
                else
                {
                    uf = str[0];
                    idclientes = 0;
                    contrato = 0;
                }
            }
            else
            {
                uf = "";
                idclientes = 0;
                contrato = 0;
            }

            if(idfuncionarios > 0)
            {
                DataTable tabela = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter("select af.aloc_uf,format(GETDATE(), 'MM/dd/yyyy HH:mm:ss') as atual, format(af.horario, 'dd/MM/yyyy HH:mm:ss') as horario,format(af.horario, 'MM/dd/yyyy HH:mm:ss') as horario_us, format(af.horario_fim, 'dd/MM/yyyy HH:mm:ss') as horario_fim, format(af.horario_fim, 'MM/dd/yyyy HH:mm:ss') as horario_fim_us, af.situacao, af.observacao, f.nome, f.sobrenome,(select u.nome from usuarios u where u.idusuarios=af.idusuarios) as usuario,(select cl.nome_fantasia from clientes cl where cl.idclientes=af.aloc_cliente) as cliente, (select c.contrato from contratos c where c.idcontratos=af.aloc_contrato) as contrato from alocacao_func af right join funcionarios f on f.idfuncionarios=af.idfuncionarios where af.idfuncionarios = " + idfuncionarios.ToString() + " order by f.nome asc,f.sobrenome asc,af.horario desc", cx.StringConexao);
                da.Fill(tabela);
                
                DataTable dt4 = new DataTable();
                dt4.Columns.Add("nome", typeof(string));
                dt4.Columns.Add("sobrenome", typeof(string));
                dt4.Columns.Add("usuario", typeof(string));
                dt4.Columns.Add("uf", typeof(string));
                dt4.Columns.Add("cliente", typeof(string));
                dt4.Columns.Add("contrato", typeof(string));
                dt4.Columns.Add("horario", typeof(string));
                dt4.Columns.Add("horario_fim", typeof(string));
                dt4.Columns.Add("tempo", typeof(string));
                dt4.Columns.Add("situacao", typeof(string));
                dt4.Columns.Add("observacao", typeof(string));

                TimeSpan ts;

                
                foreach (DataRow row in tabela.Rows)
                {
                    if ((row["horario_fim"].ToString() != "") && (row["horario"].ToString() != ""))
                    {
                        if (CultureInfo.CurrentCulture.ToString() == "pt-BR")
                        {
                            ts = Convert.ToDateTime(row["horario_fim"]) - Convert.ToDateTime(row["horario"]);
                        }
                        else
                        {
                            ts = Convert.ToDateTime(row["horario_fim_us"]) - Convert.ToDateTime(row["horario_us"]);
                        }
                    }
                    else if ((row["horario_fim"].ToString() == "") && (row["horario"].ToString() != ""))
                    {
                        if (CultureInfo.CurrentCulture.ToString() == "pt-BR")
                        {
                            ts = DateTime.Now - Convert.ToDateTime(row["horario"]);
                        }
                        else
                        {
                            ts = Convert.ToDateTime(row["atual"]) - Convert.ToDateTime(row["horario_us"]);
                        }
                    }
                    else
                    {
                        ts = TimeSpan.Zero;
                    }

                    double min = Convert.ToDouble(ts.TotalHours);

                    dt4.Rows.Add(row["nome"].ToString(), row["sobrenome"].ToString(), row["usuario"].ToString(), row["aloc_uf"].ToString(), row["cliente"].ToString(), row["contrato"].ToString(), row["horario"].ToString(), row["horario_fim"].ToString(), min.ToString("F"), row["situacao"].ToString(), row["observacao"].ToString());
                    ViewBag.nome = row["nome"].ToString() + " " + row["sobrenome"];
                    //Pegando somente o último registro
                    break;
                }

                BLLExames bllexames = new BLLExames(cx);
                BLLTreinamentos blltreinamentos = new BLLTreinamentos(cx);

                int quant = 0;
                int quanttreinamento = 0;
                ViewBag.exame_vermelho = "";
                ViewBag.exame_amarelo = "";
                ViewBag.treinamento_vermelho = "";
                ViewBag.treinamento_amarelo = "";
                quant = bllexames.VencimentoExame(idfuncionarios);
                quanttreinamento = blltreinamentos.VencimentoTreinamento(idfuncionarios);
                if ((quant != -999999) && (quant <= 10))
                {
                    if (quant < 0)
                    {
                        ViewBag.exame_vermelho = "Exame vencido!";
                    }
                    else
                    {
                        ViewBag.exame_vermelho = "Exame vencendo em " + quant + " dias!";
                    }
                }
                else if ((quant != -999999) && (quant <= 30))
                {
                    ViewBag.exame_amarelo = "Exame vencendo em " + quant + " dias!";
                }

                if ((quanttreinamento != -999999) && (quanttreinamento <= 10))
                {
                    if (quanttreinamento < 0)
                    {
                        ViewBag.treinamento_vermelho = "Treinamento vencido!";
                    }
                    else
                    {
                        ViewBag.treinamento_vermelho = "Treinamento vencendo em " + quanttreinamento + " dias!";
                    }
                }
                else if ((quanttreinamento != -999999) && (quanttreinamento <= 30))
                {
                    ViewBag.treinamento_amarelo = "Treinamento vencendo em " + quanttreinamento + " dias!";
                }


                ViewBag.idfuncionarios = idfuncionarios;
                

                ViewBag.Dados = dt4.Rows;
            }
            else if(contrato > 0)
            {
                DataTable tabela = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter("select c.contrato,c.descricao, convert(varchar(10), c.dt_inicio, 104) as dt_inicio, convert(varchar(10), c.dt_termino, 104) as dt_termino from contratos c where c.idcontratos=" + contrato, cx.StringConexao);
                da.Fill(tabela);

                DataTable dt4 = new DataTable();
                dt4.Columns.Add("contrato", typeof(string));
                dt4.Columns.Add("descricao", typeof(string));
                dt4.Columns.Add("dt_inicio", typeof(string));
                dt4.Columns.Add("dt_termino", typeof(string));

                foreach(DataRow row in tabela.Rows)
                {
                    dt4.Rows.Add(row["contrato"].ToString(), row["descricao"].ToString(), row["dt_inicio"].ToString(), row["dt_termino"].ToString());
                }

                ViewBag.contrato = contrato;

                ViewBag.Dados = dt4.Rows;
            }

            return PartialView("_AlocacaoDados");
        }

        public string Salvar(string id, int idfuncionarios, string situacao, string datahora, string observacao)
        {
            string [] str = id.Split('_');
            string uf = "";
            int idclientes = 0;
            int contrato = 0;
            
            if(str.Length == 3)
            {
                uf = str[0];
                idclientes = Convert.ToInt32(str[1]);
                contrato = Convert.ToInt32(str[2]);
            }
            else if (str.Length == 2)
            {
                uf = str[0];
                idclientes = Convert.ToInt32(str[1]);
                contrato = 0;
            }
            else if (str.Length == 1)
            {
                if (str[0] == "SPEED")
                {
                    uf = "";
                    idclientes = 0;
                    contrato = 0;
                }
                else
                {
                    uf = str[0];
                    idclientes = 0;
                    contrato = 0;
                }
            }
            else
            {
                uf = "";
                idclientes = 0;
                contrato = 0;
            }

            try
            {
                SqlCommand cmd2 = new SqlCommand();
                cmd2.Connection = cx.ObjetoConexao;
                cmd2.CommandText = "select top 1 '1' from alocacao_permissao ap where ap.permissao like 'View' and ap.idusuarios=" + Convert.ToInt32(Session["idusuarios"]);
                cx.Conectar();
                SqlDataReader registro2 = cmd2.ExecuteReader();
                if (registro2.HasRows)
                {
                    cx.Desconectar();
                    return "Você não tem permissão de movimentar funcionários!";
                }
                else
                {
                    cx.Desconectar();

                    DALConexao conexao = new DALConexao(DadosConexao.StringConexao);
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = conexao.ObjetoConexao;

                    cmd.CommandText = "select top 1 idalocacao_func from alocacao_func where idfuncionarios=" + idfuncionarios + " order by horario desc";
                    conexao.Conectar();
                    SqlDataReader registro = cmd.ExecuteReader();
                    int idaloc = 0;
                    if (registro.Read())
                    {
                        idaloc = Convert.ToInt32(registro["idalocacao_func"]);
                    }
                    conexao.Desconectar();

                    if (idaloc > 0)
                    {
                        cmd.CommandText = "update alocacao_func set horario_fim='" + datahora + "' where idalocacao_func=" + idaloc;
                        conexao.Conectar();
                        cmd.ExecuteNonQuery();
                        conexao.Desconectar();
                    }

                    cmd.CommandText = "insert into alocacao_func (idfuncionarios,idempresas,idusuarios,aloc_uf,aloc_cliente,aloc_contrato,horario,situacao,observacao) values (@idfuncionarios,@idempresas,@idusuarios,@aloc_uf,@aloc_cliente,@aloc_contrato,'"+datahora+"',@situacao,@observacao);";
                    cmd.Parameters.AddWithValue("@idfuncionarios", Convert.ToInt32(idfuncionarios));
                    cmd.Parameters.AddWithValue("@idempresas", Convert.ToInt32(Session["idempresas"]));
                    cmd.Parameters.AddWithValue("@idusuarios", Convert.ToInt32(Session["idusuarios"]));
                    cmd.Parameters.AddWithValue("@aloc_uf", Convert.ToString(uf));
                    cmd.Parameters.AddWithValue("@aloc_cliente", Convert.ToInt32(idclientes));
                    cmd.Parameters.AddWithValue("@aloc_contrato", Convert.ToInt32(contrato));
                    cmd.Parameters.AddWithValue("@situacao", Convert.ToString(situacao));
                    cmd.Parameters.AddWithValue("@observacao", Convert.ToString(observacao));

                    conexao.Conectar();
                    cmd.ExecuteNonQuery();
                    conexao.Desconectar();
                }
            }
            catch(Exception erro)
            {
                return erro.ToString();
            }

            return "";
        }

        public ActionResult AlocacaoCadastro()
        {
            if (Convert.ToInt32(Session["idempresas"]) <= 0)
            {
                return RedirectToAction("Index", "Home");
            }

            ViewBag.Dt_Inicio = DateTime.Now.AddDays(-10).ToString("dd/MM/yyyy");
            ViewBag.Dt_Fim = DateTime.Now.ToString("dd/MM/yyyy");

            DataTable dt3 = new DataTable();
            DateTime dat = DateTime.Now.AddDays(-10);
            dt3.Columns.Add("data");

            dt3.Rows.Add(new object[] { dat.ToString("dd/MM") });
            for (int i=1; i<=10; i++)
            {
                dat = dat.AddDays(+1);
                dt3.Rows.Add(new object[] { dat.ToString("dd/MM") });
            }

            ViewBag.Datas = dt3.Rows;

            BLLFuncionarios bll = new BLLFuncionarios(cx);
            DataTable dt = bll.LocalizarFuncionarios(Convert.ToInt32(Session["idempresas"]));

            ViewBag.Funcionarios = dt.Rows;

            BLLContratos bll2 = new BLLContratos(cx);
            DataTable dt2 = bll2.CarregaContratosCliente(Convert.ToInt32(Session["idempresas"]));

            ViewBag.Contratos = dt2.Rows;

            return View(dt);
        }
    }
}