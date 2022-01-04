using BLL;
using DAL;
using Modelo;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ReviewWeb.Controllers
{
    public class ProgHorasExtrasController : Controller
    {
        private DALConexao cx = new DALConexao(DadosConexao.StringConexao);
        // GET: ProgHorasExtras
        public ActionResult ProgHorasExtrasList(int? pagina, int? registros, string buscapor, string valor, string ordenapor)
        {
            if (Convert.ToInt32(Session["idempresas"]) <= 0)
            {
                return RedirectToAction("Index", "Home");
            }

            int tamanhoPagina = registros ?? 10;
            int numeroPagina = pagina ?? 1;

            ViewBag.RowsPage = tamanhoPagina;
            ViewBag.PageNum = numeroPagina;
            ViewBag.PageAnt = numeroPagina - 1;
            ViewBag.PageProx = numeroPagina + 1;

            BLLProgHorasExtras bll = new BLLProgHorasExtras(cx);
            int QuantProgramas = bll.TotalProgramas();
            double ultima = Convert.ToDouble(QuantProgramas) / Convert.ToDouble(tamanhoPagina);
            ViewBag.PageUlt = Math.Ceiling(ultima);

            DataTable dt = bll.Localizar(valor, buscapor, Convert.ToInt32(Session["idempresas"]), Convert.ToInt32(Session["idusuarios"]), numeroPagina, tamanhoPagina, ordenapor);

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cx.ObjetoConexao;
            cmd.CommandText = "select idalocacao_permissao from alocacao_permissao where idusuarios=" + Session["idusuarios"].ToString() + " and permissao='Todos'";
            cx.Conectar();
            SqlDataReader registro = cmd.ExecuteReader();
            ViewBag.Todos = 0;
            if (registro.HasRows)
            {
                ViewBag.Todos = 1;
            }
            cx.Desconectar();


            if (Request.IsAjaxRequest())
            {
                return PartialView("_ProgHorasExtrasList", dt);
            }

            return View(dt);
        }

        [HttpPost]
        public string ExcluirSelecionados(string check)
        {
            BLLProgHorasExtras bll = new BLLProgHorasExtras(cx);
            string[] ids = check.Split(new char[] { ';' });
            string msg = "Registros excluídos com sucesso!";
            foreach (string item in ids)
            {
                if (item != "")
                {
                    //Excluir Selecionados
                    try
                    {
                        bll.Excluir(Convert.ToInt32(item));
                    }
                    catch (Exception erro)
                    {
                        msg = "Erro ao excluir!\n\n" + erro.ToString();
                    }
                }
            }

            return msg;
        }

        public ActionResult ProgHorasExtrasCadastro(int idproghorasextras, int idcontratos)
        {
            if (Convert.ToInt32(Session["idempresas"]) <= 0)
            {
                return RedirectToAction("Index", "Home");
            }

            ModeloProgHorasExtras modprogramacao = new ModeloProgHorasExtras();
            DataTable dt4 = new DataTable();
            BLLContratos bll2 = new BLLContratos(cx);

            if (idproghorasextras > 0)
            {
                BLLProgHorasExtras bll = new BLLProgHorasExtras(cx);
                modprogramacao = bll.CarregaProgHorasExtras(idproghorasextras);

                ViewBag.idproghorasextras = idproghorasextras;

                DataTable dt = bll2.LocalizarContratos(Convert.ToInt32(Session["idempresas"]),Convert.ToInt32(Session["idusuarios"]));

                ViewBag.Contratos = dt.Rows;
                
            }
            else
            {
                DataTable dt = bll2.LocalizarContratos(Convert.ToInt32(Session["idempresas"]), Convert.ToInt32(Session["idusuarios"]));

                ViewBag.Contratos = dt.Rows;
            }

            
            DataTable tabela = bll2.CarregaFuncionarios(idcontratos);
            int idfuncionarios = 0;

            dt4.Columns.Add("idfuncionarios", typeof(int));
            dt4.Columns.Add("nome", typeof(string));
            dt4.Columns.Add("sobrenome", typeof(string));

            foreach (DataRow row in tabela.Rows)
            {
                if ((idfuncionarios != ConverteReader.ConverteInt(row["idfuncionarios"])) && (idcontratos == ConverteReader.ConverteInt(row["idcontratos"])))
                {
                    dt4.Rows.Add(ConverteReader.ConverteInt(row["idfuncionarios"]), row["nome"].ToString(), row["sobrenome"].ToString());
                    idfuncionarios = ConverteReader.ConverteInt(row["idfuncionarios"]);
                }
            }

            ViewBag.Funcionarios = dt4.Rows;

            if (Request.IsAjaxRequest())
            {
                return PartialView("_ProgHorasExtrasFuncionarios");
            }

            return View(modprogramacao);
        }

        public ActionResult ExibeFuncionarios(int idcontratos)
        {
            BLLContratos bll2 = new BLLContratos(cx);
            DataTable dt4 = new DataTable();
            DataTable tabela = bll2.CarregaFuncionarios(idcontratos);
            int idfuncionarios = 0;

            dt4.Columns.Add("idfuncionarios", typeof(int));
            dt4.Columns.Add("nome", typeof(string));
            dt4.Columns.Add("sobrenome", typeof(string));

            foreach (DataRow row in tabela.Rows)
            {
                if ((idfuncionarios != ConverteReader.ConverteInt(row["idfuncionarios"])) && (idcontratos == ConverteReader.ConverteInt(row["idcontratos"])))
                {
                    dt4.Rows.Add(ConverteReader.ConverteInt(row["idfuncionarios"]), row["nome"].ToString(), row["sobrenome"].ToString());
                    idfuncionarios = ConverteReader.ConverteInt(row["idfuncionarios"]);
                }
            }

            ViewBag.Funcionarios = dt4.Rows;

            return PartialView("_ProgHorasExtrasFuncionarios");
        }
        

        public string FimdeSemana(int dia)
        {
            string res = "";
            for(int i=1; i <= 7; i++)
            {
                if(Convert.ToInt32(DateTime.Now.AddDays(i).DayOfWeek) == dia)
                {
                    res = DateTime.Now.AddDays(i).ToString("dd/MM/yyyy");

                    break;
                }
            }
            return res;
        }

        public string DataAtual()
        {
            return DateTime.Now.ToString("dd/MM/yyyy");
        }
        

        [HttpPost]
        public JsonResult ProgHorasExtrasCadastro(string check, string idcontratos, string tipo, string dia_todo, string quant_horas, string dt_prog)
        { 
            ModeloProgHorasExtras modProgHorasExtras = new ModeloProgHorasExtras();
            DateTime resultado;
            modProgHorasExtras.IdContratos = Convert.ToInt32(idcontratos);
            modProgHorasExtras.Tipo = tipo;
            modProgHorasExtras.Dia_Todo = Convert.ToInt32(dia_todo);
            modProgHorasExtras.IdUsuarios = Convert.ToInt32(Session["idusuarios"]);

            BLLContratos bll2 = new BLLContratos(cx);
            DataTable dt = bll2.LocalizarContratos(Convert.ToInt32(Session["idempresas"]), Convert.ToInt32(Session["idusuarios"]));
            ViewBag.Contratos = dt.Rows;

            if (!DateTime.TryParse(dt_prog.ToString().Trim(), out resultado))
            {
                ModelState.AddModelError("Dt_Prog", "Data inválida!");
            }
            else
            {
                modProgHorasExtras.Dt_Prog = Convert.ToDateTime(dt_prog);
            }

            if (ModelState.IsValid == true)
            {
                if(Convert.ToInt32(dia_todo) == 1)
                {
                    modProgHorasExtras.Quant_Horas = 8;
                }
                else
                {
                    modProgHorasExtras.Quant_Horas = Convert.ToInt32(quant_horas);
                }

                modProgHorasExtras.Liberado = 0;
                
                try
                {
                    BLLProgHorasExtras bll = new BLLProgHorasExtras(cx);
                    string[] ids = check.Split(new char[] { ';' });
                    foreach (string item in ids)
                    {
                        if (item != "")
                        {
                            //Incluir Selecionados
                            try
                            {
                                bll.IncluirItens(Convert.ToInt32(item), modProgHorasExtras);
                            }
                            catch (Exception erro)
                            {
                                return Json("Erro ao salvar registro!\n\n" + erro.ToString());
                            }
                        }
                    }

                    return Json(Url.Action("ProgHorasExtrasList", "ProgHorasExtras"));
                }
                catch (Exception erro)
                {
                    modProgHorasExtras.Dt_Prog = null;
                    modProgHorasExtras.IdContratos = 0;
                    modProgHorasExtras.Tipo = "";
                    modProgHorasExtras.Dia_Todo = 0;
                    modProgHorasExtras.Quant_Horas = 0;

                    return Json("Erro ao salvar registro!\n\n" + erro);
                }
            }

            return Json(modProgHorasExtras);
        }

        public ActionResult Liberar(int id, ModeloProgHorasExtras modProgHorasExtras)
        {
            BLLProgHorasExtras bll = new BLLProgHorasExtras(cx);

            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = cx.ObjetoConexao;
                cmd.CommandText = "update prog_horas_extras set liberado=1 where idprog_horas_extras=@idprog_horas_extras;";
                cmd.Parameters.AddWithValue("@idprog_horas_extras", id);

                cx.Conectar();
                cmd.ExecuteNonQuery();
                cx.Desconectar();
            }
            catch (Exception erro)
            {
                return View(modProgHorasExtras).Mensagem("Erro ao excluir registro!\n\n" + erro);
            }

            return RedirectToAction("ProgHorasExtrasList", "ProgHorasExtras").Mensagem("Registro excluído com sucesso!");
        }

        public ActionResult Excluir(int id, ModeloProgHorasExtras modProgHorasExtras)
        {
            BLLProgHorasExtras bll = new BLLProgHorasExtras(cx);

            try
            {
                bll.Excluir(id);
            }
            catch (Exception erro)
            {
                return View(modProgHorasExtras).Mensagem("Erro ao excluir registro!\n\n" + erro);
            }

            return RedirectToAction("ProgHorasExtrasList", "ProgHorasExtras").Mensagem("Registro excluído com sucesso!");
        }
    }
}