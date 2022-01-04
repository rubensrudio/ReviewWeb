using BLL;
using DAL;
using Modelo;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ReviewWeb.Controllers
{
    public class ExamesController : Controller
    {
        private DALConexao cx = new DALConexao(DadosConexao.StringConexao);
        // GET: Clientes
        public ActionResult ExamesList(int? pagina, int? registros, string buscapor, string valor, string ordenapor)
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

            BLLExames bll = new BLLExames(cx);
            int QuantExames = bll.TotalExames();
            double ultima = Convert.ToDouble(QuantExames) / Convert.ToDouble(tamanhoPagina);
            ViewBag.PageUlt = Math.Ceiling(ultima);

            DataTable dt = bll.Localizar(valor, buscapor, Convert.ToInt32(Session["idempresas"]), numeroPagina, tamanhoPagina, ordenapor);


            if (Request.IsAjaxRequest())
            {
                return PartialView("_ExamesList", dt);
            }

            return View(dt);
        }

        [HttpPost]
        public string ExcluirSelecionados(string check)
        {
            BLLExames bll = new BLLExames(cx);
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

        public ActionResult ExamesCadastro(int idexames)
        {
            if (Convert.ToInt32(Session["idempresas"]) <= 0)
            {
                return RedirectToAction("Index", "Home");
            }

            ModeloExames modexame = new ModeloExames();

            if (idexames > 0)
            {
                BLLExames bll = new BLLExames(cx);
                modexame = bll.CarregaExames(idexames);

                ViewBag.idexames = idexames;

                BLLFuncionarios bll2 = new BLLFuncionarios(cx);
                DataTable dt = bll2.LocalizarFuncionarios(Convert.ToInt32(Session["idempresas"]));

                ViewBag.Funcionarios = dt.Rows;
            }
            else
            {
                BLLFuncionarios bll = new BLLFuncionarios(cx);
                DataTable dt = bll.LocalizarFuncionarios(Convert.ToInt32(Session["idempresas"]));

                ViewBag.Funcionarios = dt.Rows;
            }

            return View(modexame);
        }

        [HttpPost]
        public ActionResult ExamesCadastro(ModeloExames modExames)
        {
            BLLExames bll = new BLLExames(cx);
            
            if (ModelState.IsValid == true)
            {
                try
                {
                    if (modExames.IdExames == 0)
                    {
                        bll.Incluir(modExames);
                    }
                    else
                    {
                        bll.Alterar(modExames);
                    }
                }
                catch (Exception erro)
                {
                    return View(modExames).Mensagem("Erro ao salvar registro!\n\n" + erro);
                }

                return RedirectToAction("ExamesList", "Exames").Mensagem("Registro salvo com sucesso!");
            }

            return View(modExames);
        }

        public ActionResult Excluir(int id, ModeloExames modExames)
        {
            BLLExames bll = new BLLExames(cx);

            try
            {
                bll.Excluir(id);
            }
            catch (Exception erro)
            {
                return View(modExames).Mensagem("Erro ao excluir registro!\n\n" + erro);
            }

            return RedirectToAction("ExamesList", "Exames").Mensagem("Registro excluído com sucesso!");
        }
    }
}