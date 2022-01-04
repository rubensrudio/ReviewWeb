using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using Rotativa;
using Rotativa.Options;
using ReviewWeb.Models;
using Modelo;
using BLL;
using DAL;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using ClosedXML;
using ClosedXML.Excel;
using System.IO;

namespace ReviewWeb.Controllers
{
    public class RelatoriosController : Controller
    {
        private RelatoriosConexao db = new RelatoriosConexao();
        // GET: Relatorios
        public ActionResult ListagemClientes(int? pagina, Boolean? gerarPDF)
        {
            var listaClientes = db.cliente.OrderBy(c => c.Nome_Fantasia).ToList();
            //Definindo a paginação
            int paginaQdteRegistros = 10;
            int paginaNumeroNavegacao = (pagina ?? 1);

            if (gerarPDF != true)
            {
                return View(listaClientes.ToPagedList(paginaNumeroNavegacao, paginaQdteRegistros));
            }
            else
            {
                return new PartialViewAsPdf("_ListagemClientes", listaClientes.ToPagedList(paginaNumeroNavegacao, listaClientes.Count))
                {
                    PageSize = Size.A4,
                    PageMargins = new Margins { Bottom = 5, Left = 5, Right = 5, Top = 5 },
                    PageOrientation = Orientation.Portrait,
                    IsGrayScale = true,
                    CustomSwitches = "--footer-right \"" + DateTime.Now.Date.ToString("dd/MM/yyyy") + "  Pag.: [page]/[toPage]\" --footer-line --footer-font-size \"9\" --footer-spacing 6 --footer-font-name \"calibri light\""
                };
            }
        }

        //Relatório de Horas
        public ActionResult ListagemHoras()
        {
            DALConexao cx = new DALConexao(DadosConexao.StringConexao);
            BLLContratos bll = new BLLContratos(cx);
            DataTable dt = bll.LocalizarContratos(Convert.ToInt32(Session["idempresas"]), Convert.ToInt32(Session["idusuarios"]));
            ViewBag.Contratos = dt.Rows;

            BLLFuncionarios bll2 = new BLLFuncionarios(cx);
            DataTable dt2 = bll2.LocalizarFuncionarios(Convert.ToInt32(Session["idempresas"]));
            ViewBag.Funcionarios = dt2.Rows;

            return View();
        }

        [HttpGet]
        public ActionResult HorasContrato(int tipo, string dataInicial, string dataFinal, string IdContratos, string IdFuncionarios, string Rateio)
        {
            DALConexao cx = new DALConexao(DadosConexao.StringConexao);

            BLLLocaisAtuacao bll = new BLLLocaisAtuacao(cx);
            DataTable dt2 = bll.CarregarClientes(Convert.ToInt32(Session["idempresas"]));
            BLLContratos bll2 = new BLLContratos(cx);
            DataTable dt3 = bll2.CarregaContratosEmpresa(Convert.ToInt32(Session["idempresas"]), dataInicial, dataFinal);
            DataTable dt4 = bll.CarregarUF(Convert.ToInt32(Session["idempresas"]), Convert.ToInt32(Session["idusuarios"]));

            ViewBag.Total_Contratos = bll2.TotalContratos(Convert.ToInt32(Session["idempresas"]));
            ViewBag.Clientes = dt2.Rows;
            ViewBag.Contratos = dt3.Rows;
            ViewBag.UF = dt4.Rows;
            ViewBag.Rateio = Rateio;

            //DateTime horario = Convert.ToDateTime("01/05/2019 00:00:00");
            DataTable dt = new DataTable();
            DataTable tabela = new DataTable("tabela");
            dataInicial = DateTime.Parse(dataInicial).ToString("dd/MM/yyyy");
            dataFinal = DateTime.Parse(dataFinal).ToString("dd/MM/yyyy");
            string where = "";
            string where2 = "";
            if (IdContratos != "")
            {
                where += " and af.aloc_contrato=" + IdContratos;
                where2 += " and he.idcontratos=" + IdContratos;
            }

            if (IdFuncionarios != "")
            {
                where += " and af.idfuncionarios=" + IdFuncionarios;
                where2 += " and he.idfuncionarios=" + IdFuncionarios;
            }

            string sql = "select f.nome,f.sobrenome,(select CONCAT(c.contrato,' - ',c.descricao) from contratos c where c.idcontratos=af.aloc_contrato) as contrato,af.horario,af.horario_fim, af.aloc_uf, af.aloc_cliente, f.salario_base " +
                "from alocacao_func af " +
                "right join funcionarios f on f.idfuncionarios = af.idfuncionarios " +
                "where af.idempresas = " + Session["idempresas"].ToString() + " and af.horario>= '" + dataInicial + " 00:00:00' and af.horario<='" + dataFinal + " 23:59:59' " + where + " " +
                "order by contrato,f.nome,f.sobrenome,af.horario desc;";
            SqlDataAdapter da = new SqlDataAdapter(sql, cx.StringConexao);
            da.Fill(dt);

            tabela.Columns.Add("nome", typeof(string));
            tabela.Columns.Add("contrato", typeof(string));
            tabela.Columns.Add("inicio", typeof(DateTime));
            tabela.Columns.Add("termino", typeof(DateTime));
            tabela.Columns.Add("horas", typeof(double));
            tabela.Columns.Add("valor", typeof(double));
            tabela.Columns.Add("aloc_uf", typeof(string));
            tabela.Columns.Add("aloc_cliente", typeof(string));
            TimeSpan ts;
            foreach (DataRow item in dt.Rows)
            {
                double horas = 0;

                //Alterar para Filtro
                DateTime dt_termino = Convert.ToDateTime(dataFinal + " 23:59:59");
                DateTime data_aux = Convert.ToDateTime(item["horario"]);
                DateTime data_fim_aux;
                if (item["horario_fim"].ToString() == "")
                {
                    if (dt_termino > DateTime.Now)
                    {
                        data_fim_aux = DateTime.Now;
                    }
                    else
                    {
                        data_fim_aux = dt_termino;
                    }
                }
                else
                {
                    if (Convert.ToDateTime(item["horario_fim"]) > dt_termino)
                    {
                        data_fim_aux = dt_termino;
                    }
                    else
                    {
                        data_fim_aux = Convert.ToDateTime(item["horario_fim"]);
                    }
                }
                int i = 0;
                string data_only, data_fim_only, inicio_dia, fim_dia, dt_termino_s;
                string[] data_arr, data_fim_arr, dt_termino_arr;

                dt_termino_s = dt_termino.ToString();
                dt_termino_arr = dt_termino_s.Split(' ');
                dt_termino_s = dt_termino_arr[0] + " " + "17:00:00";

                while (i == 0)
                {
                    //Somente data inicio
                    data_only = data_aux.ToString();
                    data_arr = data_only.Split(' ');
                    data_only = data_arr[0] + " " + "00:00:00";
                    inicio_dia = data_arr[0] + " " + "08:00:00";
                    fim_dia = data_arr[0] + " " + "17:00:00";

                    //Somente data fim
                    data_fim_only = data_fim_aux.ToString();
                    data_fim_arr = data_fim_only.Split(' ');
                    data_fim_only = data_fim_arr[0] + " " + "00:00:00";

                    if ((data_aux.DayOfWeek != DayOfWeek.Saturday) && (data_aux.DayOfWeek != DayOfWeek.Sunday))
                    {
                        //Se segunda a sexta
                        //vai até o horário fim
                        if (Convert.ToDateTime(data_only) < Convert.ToDateTime(data_fim_only))
                        {
                            //Se maior que 08:00
                            if (data_aux > Convert.ToDateTime(inicio_dia))
                            {
                                ts = Convert.ToDateTime(fim_dia) - data_aux;
                            }
                            else
                            {
                                ts = Convert.ToDateTime(fim_dia) - Convert.ToDateTime(inicio_dia);
                            }
                        }
                        else
                        {
                            ts = data_fim_aux - Convert.ToDateTime(inicio_dia);
                            i = 1;
                        }
                    }
                    else
                    {
                        ts = TimeSpan.Zero;
                    }

                    data_aux = data_aux.AddDays(1);
                    data_arr = data_aux.ToString().Split(' ');
                    data_aux = Convert.ToDateTime(data_arr[0] + " " + "08:00:00");
                    if (ts != TimeSpan.Zero)
                    {
                        horas += ts.TotalHours - 1; //Desconta 1 hora de almoço
                    }
                }

                double valor = (Convert.ToDouble(item["salario_base"]) / 176) * horas;
                DateTime dt_fim;
                if(item["horario_fim"].ToString() == "")
                {
                    dt_fim = DateTime.MinValue;
                }
                else
                {
                    dt_fim = Convert.ToDateTime(item["horario_fim"]);
                }
                

                if (dt_fim > dt_termino)
                {
                    tabela.Rows.Add(item["nome"] + " " + item["sobrenome"], item["contrato"], item["horario"], dt_termino, Math.Round(horas, 2), Math.Round(valor, 2), item["aloc_uf"], item["aloc_cliente"]);
                }
                else
                {
                    tabela.Rows.Add(item["nome"] + " " + item["sobrenome"], item["contrato"], item["horario"], item["horario_fim"], Math.Round(horas, 2), Math.Round(valor, 2), item["aloc_uf"], item["aloc_cliente"]);
                }

                
            }


            //Horas Extras falta concluir...

            DataTable dt5 = new DataTable();
            DataTable tabela2 = new DataTable("tabela2");

            string sql2 = "select f.nome,f.sobrenome,f.salario_base,c.idcontratos, cl.uf, he.tipo, he.quant_horas, he.dt_prog, cli.idclientes " +
                "from prog_horas_extras he " +
                "join contratos c on c.idcontratos = he.idcontratos " +
                "join clientes_localidades cl on cl.idclientes_localidades = c.idcontratos_clientes_localidades " +
                "join clientes cli on cli.idclientes = cl.idclientes " +
                "join funcionarios f on f.idfuncionarios = he.idfuncionarios " +
                "where cli.idempresas = " + Session["idempresas"].ToString() + " and he.dt_prog >= '" + dataInicial + " 00:00:00' and he.dt_prog <= '" + dataFinal + " 23:59:59' and he.liberado = 1 " + where2 + " " +
                "order by contrato,f.nome,f.sobrenome,he.dt_prog desc; ";

            SqlDataAdapter da2 = new SqlDataAdapter(sql2, cx.StringConexao);
            da2.Fill(dt5);

            tabela2.Columns.Add("nome", typeof(string));
            tabela2.Columns.Add("idcontratos", typeof(string));
            tabela2.Columns.Add("dt_prog", typeof(DateTime));
            tabela2.Columns.Add("horas", typeof(double));
            tabela2.Columns.Add("valor", typeof(double));
            tabela2.Columns.Add("uf", typeof(string));
            tabela2.Columns.Add("cliente", typeof(string));
            tabela2.Columns.Add("tipo", typeof(string));

            double valor2 = 0;
            foreach (DataRow item2 in dt5.Rows)
            {
                
                if (item2["tipo"].ToString() == "Domingo")
                {
                    //Domingo 100%
                    valor2 = (Convert.ToDouble(item2["salario_base"]) / 176) * 2 * Convert.ToDouble(item2["quant_horas"]);
                }
                else if (item2["tipo"].ToString() == "Sábado")
                {
                    //Sábado 50%
                    valor2 = (Convert.ToDouble(item2["salario_base"]) / 176) * 1.5 * Convert.ToDouble(item2["quant_horas"]);
                }
                else if (item2["tipo"].ToString() == "Feriado")
                {
                    //Feriado 100%
                    valor2 = (Convert.ToDouble(item2["salario_base"]) / 176) * 2 * Convert.ToDouble(item2["quant_horas"]);
                }
                else if (item2["tipo"].ToString() == "Banco")
                {
                    //Banco dia normal
                    valor2 = (Convert.ToDouble(item2["salario_base"]) / 176) * Convert.ToDouble(item2["quant_horas"]);
                }
                else if (item2["tipo"].ToString() == "Hora Extra - 1 hora")
                {
                    //Hora Extra - 1 hora
                    valor2 = (Convert.ToDouble(item2["salario_base"]) / 176) * 1.5 * Convert.ToDouble(item2["quant_horas"]);
                }
                else if (item2["tipo"].ToString() == "Feriado")
                {
                    //Hora Extra - 2 horas
                    valor2 = (Convert.ToDouble(item2["Hora Extra - 2 horas"]) / 176) * 1.5 * Convert.ToDouble(item2["quant_horas"]);
                }
                else
                {
                    valor2 = (Convert.ToDouble(item2["salario_base"]) / 176) * Convert.ToDouble(item2["quant_horas"]);
                }

                tabela2.Rows.Add(item2["nome"] + " " + item2["sobrenome"], item2["idcontratos"], item2["dt_prog"],  Math.Round(Convert.ToDouble(item2["quant_horas"]), 2), Math.Round(valor2, 2), item2["uf"], item2["idclientes"], item2["tipo"]);
            }

            ViewBag.HorasExtras = tabela2.Rows;

            if (tipo == 1)
            {
                return new PartialViewAsPdf("_HorasContrato", tabela)
                {
                    PageSize = Size.A4,
                    PageMargins = new Margins { Bottom = 5, Left = 5, Right = 5, Top = 5 },
                    PageOrientation = Orientation.Landscape,
                    IsGrayScale = true
                    //CustomSwitches = "--footer-right \"" + DateTime.Now.Date.ToString("dd/MM/yyyy") + "  Pag.: [page]/[toPage]\" --footer-line --footer-font-size \"9\" --footer-spacing 6 --footer-font-name \"calibri light\""
                };
            }
            else
            {
                return PartialView("_HorasContrato", tabela);
            }
        }


        //Relatório por função
        public ActionResult ListagemHorasCargos()
        {
            DALConexao cx = new DALConexao(DadosConexao.StringConexao);
            BLLContratos bll = new BLLContratos(cx);
            DataTable dt = bll.LocalizarContratos(Convert.ToInt32(Session["idempresas"]), Convert.ToInt32(Session["idusuarios"]));
            ViewBag.Contratos = dt.Rows;

            BLLCargos bll2 = new BLLCargos(cx);
            DataTable dt2 = bll2.Localizar("", "");
            ViewBag.Cargos = dt2.Rows;

            return View();
        }

        [HttpGet]
        public ActionResult HorasCargos(string dataInicial, string dataFinal, string IdContratos, string IdCargos)
        {
            DALConexao cx = new DALConexao(DadosConexao.StringConexao);

            BLLLocaisAtuacao bll = new BLLLocaisAtuacao(cx);
            DataTable dt2 = bll.CarregarClientes(Convert.ToInt32(Session["idempresas"]));
            BLLContratos bll2 = new BLLContratos(cx);
            DataTable dt3 = bll2.CarregaContratosEmpresa(Convert.ToInt32(Session["idempresas"]), dataInicial, dataFinal);
            DataTable dt4 = bll.CarregarUF(Convert.ToInt32(Session["idempresas"]), Convert.ToInt32(Session["idusuarios"]));

            ViewBag.Total_Contratos = bll2.TotalContratos(Convert.ToInt32(Session["idempresas"]));
            ViewBag.Clientes = dt2.Rows;
            ViewBag.Contratos = dt3.Rows;
            ViewBag.UF = dt4.Rows;

            //DateTime horario = Convert.ToDateTime("01/05/2019 00:00:00");
            DataTable dt = new DataTable();
            DataTable tabela = new DataTable("tabela");
            dataInicial = DateTime.Parse(dataInicial).ToString("dd/MM/yyyy");
            dataFinal = DateTime.Parse(dataFinal).ToString("dd/MM/yyyy");
            string where = "";
            if (IdContratos != "")
            {
                where += " and af.aloc_contrato=" + IdContratos;
            }

            if (IdCargos != "")
            {
                where += " and cg.idcargos=" + IdCargos;
            }

            string sql = "select cg.descricao,(select CONCAT(c.contrato,' - ',c.descricao) from contratos c where c.idcontratos=af.aloc_contrato) as contrato,af.horario,af.horario_fim, af.aloc_uf, af.aloc_cliente, f.salario_base " +
                "from alocacao_func af " +
                "right join funcionarios f on f.idfuncionarios = af.idfuncionarios " +
                "right join cargos cg on cg.idcargos = f.idcargos " +
                "where af.idempresas = " + Session["idempresas"].ToString() + " and af.horario>= '" + dataInicial + " 00:00:00' and af.horario<='" + dataFinal + " 23:59:59' " + where + " " +
                "order by contrato,cg.descricao,af.horario desc;";
            SqlDataAdapter da = new SqlDataAdapter(sql, cx.StringConexao);
            da.Fill(dt);

            tabela.Columns.Add("descricao", typeof(string));
            tabela.Columns.Add("contrato", typeof(string));
            tabela.Columns.Add("inicio", typeof(DateTime));
            tabela.Columns.Add("termino", typeof(DateTime));
            tabela.Columns.Add("horas", typeof(double));
            tabela.Columns.Add("valor", typeof(double));
            tabela.Columns.Add("aloc_uf", typeof(string));
            tabela.Columns.Add("aloc_cliente", typeof(string));
            TimeSpan ts;
            string contrato = "";
            string descricao = "";
            double horas = 0;
            double valor = 0;
            DateTime? horario_aux = null;
            DateTime? horario_fim_aux= null;
            string aloc_uf = "";
            string aloc_cliente = "";
            foreach (DataRow item in dt.Rows)
            {
                double horas_func = 0;
                if ((item["descricao"].ToString() != descricao) && (item["contrato"].ToString() == ""))
                {
                    descricao = item["descricao"].ToString();
                    contrato = item["contrato"].ToString();
                    horas = 0;
                    valor = 0;
                    horario_aux = Convert.ToDateTime(item["horario"]);
                    if (item["horario_fim"].ToString() == "")
                        horario_fim_aux = null;
                    else
                        horario_fim_aux = Convert.ToDateTime(item["horario_fim"]);
                    aloc_uf = item["aloc_uf"].ToString();
                    aloc_cliente = item["aloc_cliente"].ToString();
                }
                else if ((item["descricao"].ToString() != descricao) || (item["contrato"].ToString() != contrato))
                {
                    tabela.Rows.Add(descricao, contrato, horario_aux, horario_fim_aux, Math.Round(horas, 2), Math.Round(valor, 2), aloc_uf, aloc_cliente);

                    descricao = item["descricao"].ToString();
                    contrato = item["contrato"].ToString();
                    horas = 0;
                    valor = 0;
                    horario_aux = Convert.ToDateTime(item["horario"]);
                    if (item["horario_fim"].ToString() == "")
                        horario_fim_aux = null;
                    else
                        horario_fim_aux = Convert.ToDateTime(item["horario_fim"]);
                    aloc_uf = item["aloc_uf"].ToString();
                    aloc_cliente = item["aloc_cliente"].ToString();
                }
                else if ((contrato == "") || (descricao == ""))
                {
                    descricao = item["descricao"].ToString();
                    contrato = item["contrato"].ToString();
                    horas = 0;
                    valor = 0;
                    horario_aux = Convert.ToDateTime(item["horario"]);
                    if (item["horario_fim"].ToString() == "")
                        horario_fim_aux = null;
                    else
                        horario_fim_aux = Convert.ToDateTime(item["horario_fim"]);
                    aloc_uf = item["aloc_uf"].ToString();
                    aloc_cliente = item["aloc_cliente"].ToString();
                }


                //Alterar para Filtro
                DateTime dt_termino = Convert.ToDateTime(dataFinal + " 23:59:59");
                DateTime data_aux = Convert.ToDateTime(item["horario"]);
                DateTime data_fim_aux;
                if (item["horario_fim"].ToString() == "")
                {
                    if (dt_termino > DateTime.Now)
                    {
                        data_fim_aux = DateTime.Now;
                    }
                    else
                    {
                        data_fim_aux = dt_termino;
                    }
                }
                else
                {
                    data_fim_aux = Convert.ToDateTime(item["horario_fim"]);
                }
                int i = 0;
                string data_only, data_fim_only, inicio_dia, fim_dia, dt_termino_s;
                string[] data_arr, data_fim_arr, dt_termino_arr;

                dt_termino_s = dt_termino.ToString();
                dt_termino_arr = dt_termino_s.Split(' ');
                dt_termino_s = dt_termino_arr[0] + " " + "17:00:00";

                while (i == 0)
                {
                    //Somente data inicio
                    data_only = data_aux.ToString();
                    data_arr = data_only.Split(' ');
                    data_only = data_arr[0] + " " + "00:00:00";
                    inicio_dia = data_arr[0] + " " + "08:00:00";
                    fim_dia = data_arr[0] + " " + "17:00:00";

                    //Somente data fim
                    data_fim_only = data_fim_aux.ToString();
                    data_fim_arr = data_fim_only.Split(' ');
                    data_fim_only = data_fim_arr[0] + " " + "00:00:00";

                    if ((data_aux.DayOfWeek != DayOfWeek.Saturday) && (data_aux.DayOfWeek != DayOfWeek.Sunday))
                    {
                        //Se segunda a sexta
                        //vai até o horário fim
                        if (Convert.ToDateTime(data_only) < Convert.ToDateTime(data_fim_only))
                        {
                            //Se maior que 08:00
                            if (data_aux > Convert.ToDateTime(inicio_dia))
                            {
                                ts = Convert.ToDateTime(fim_dia) - data_aux;
                            }
                            else
                            {
                                ts = Convert.ToDateTime(fim_dia) - Convert.ToDateTime(inicio_dia);
                            }
                        }
                        else
                        {
                            ts = data_fim_aux - Convert.ToDateTime(inicio_dia);
                            i = 1;
                        }
                    }
                    else
                    {
                        ts = TimeSpan.Zero;
                    }

                    data_aux = data_aux.AddDays(1);
                    data_arr = data_aux.ToString().Split(' ');
                    data_aux = Convert.ToDateTime(data_arr[0] + " " + "08:00:00");
                    if (ts != TimeSpan.Zero)
                    {
                        horas += ts.TotalHours - 1; //Desconta 1 hora de almoço
                        horas_func += ts.TotalHours - 1;
                    }
                }

                valor = valor + ((Convert.ToDouble(item["salario_base"]) / 176) * horas_func);

                horario_aux = Convert.ToDateTime(item["horario"]);
                if (item["horario_fim"].ToString() == "")
                    horario_fim_aux = null;
                else
                    horario_fim_aux = Convert.ToDateTime(item["horario_fim"]);
                aloc_uf = item["aloc_uf"].ToString();
                aloc_cliente = item["aloc_cliente"].ToString();
            }

            if (horas != 0)
            {
                tabela.Rows.Add(descricao, contrato, horario_aux, horario_fim_aux, Math.Round(horas, 2), Math.Round(valor, 2), aloc_uf, aloc_cliente);
            }


            return new PartialViewAsPdf("_HorasCargos", tabela)
            {
                PageSize = Size.A4,
                PageMargins = new Margins { Bottom = 5, Left = 5, Right = 5, Top = 5 },
                PageOrientation = Orientation.Landscape,
                IsGrayScale = true,
                CustomSwitches = "--footer-right \"" + DateTime.Now.Date.ToString("dd/MM/yyyy") + "  Pag.: [page]/[toPage]\" --footer-line --footer-font-size \"9\" --footer-spacing 6 --footer-font-name \"calibri light\""
            };
        }
        
        //Relatório Histograma
        public ActionResult HistogramaList()
        {
            DALConexao cx = new DALConexao(DadosConexao.StringConexao);
            BLLContratos bll = new BLLContratos(cx);
            DataTable dt = bll.LocalizarContratos(Convert.ToInt32(Session["idempresas"]), Convert.ToInt32(Session["idusuarios"]));
            ViewBag.Contratos = dt.Rows;

            ViewBag.Ano = DateTime.Now.Year;

            return View();
        }

        /*
        [HttpGet]
        public ActionResult Histograma(int tipo, string mes, string ano, string IdContratos)
        {
            DALConexao cx = new DALConexao(DadosConexao.StringConexao);

            BLLContratos bll = new BLLContratos(cx);
            DataTable dt = bll.LocalizarContratos(Convert.ToInt32(Session["idempresas"]), Convert.ToInt32(Session["idusuarios"]));
            ViewBag.Contratos = dt.Rows;

            BLLFuncionarios bll2 = new BLLFuncionarios(cx);
            DataTable dt2 = bll.()

            DataTable tabela = new DataTable("tabela");

            if (tipo == 1)
            {
                return new PartialViewAsPdf("_RelHistograma", tabela)
                {
                    PageSize = Size.A4,
                    PageMargins = new Margins { Bottom = 5, Left = 5, Right = 5, Top = 5 },
                    PageOrientation = Orientation.Landscape,
                    IsGrayScale = true
                    //CustomSwitches = "--footer-right \"" + DateTime.Now.Date.ToString("dd/MM/yyyy") + "  Pag.: [page]/[toPage]\" --footer-line --footer-font-size \"9\" --footer-spacing 6 --footer-font-name \"calibri light\""
                };
            }
            else
            {
                return PartialView("_RelHistograma", tabela);
            }
        }*/
    }
}