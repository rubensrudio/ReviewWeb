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
    public class FolhaPagamentosController : Controller
    {
        private DALConexao cx = new DALConexao(DadosConexao.StringConexao);
        // GET: FolhaPagamentos
        public ActionResult FolhaPagamentosList(int? pagina, int? registros, string buscapor, string valor, string ordenapor)
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

            BLLFolhaPagamentos bll = new BLLFolhaPagamentos(cx);
            int QuantFolha = bll.TotalFolhaPagamento();
            double ultima = Convert.ToDouble(QuantFolha) / Convert.ToDouble(tamanhoPagina);
            ViewBag.PageUlt = Math.Ceiling(ultima);

            DataTable dt = bll.Localizar(valor, buscapor, Convert.ToInt32(Session["idempresas"]), numeroPagina, tamanhoPagina, ordenapor);


            if (Request.IsAjaxRequest())
            {
                return PartialView("_FolhaPagamentosList", dt);
            } 
            return View(dt);
        }

        [HttpPost]
        public string ExcluirSelecionados(string check)
        {
            BLLFolhaPagamentos bll = new BLLFolhaPagamentos(cx);
            string[] ids = check.Split(new char[] { ';' });
            string msg = "Registros excluídos com sucesso!";
            foreach (string item in ids)
            {
                if (item != "")
                {
                    //Excluir Selecionados
                    try
                    {
                        bll.Excluir(item);
                    }
                    catch (Exception erro)
                    {
                        msg = "Erro ao excluir!\n\n" + erro.ToString();
                    }
                }
            }

            return msg;
        }

        public ActionResult FolhaPagamentosCadastro(string mes_base)
        {
            if (Convert.ToInt32(Session["idempresas"]) <= 0)
            {
                return RedirectToAction("Index", "Home");
            }

            ModeloFolhaPagamentos modelo = new ModeloFolhaPagamentos();

            if (mes_base != "")
            {
                BLLFolhaPagamentos bll = new BLLFolhaPagamentos(cx);
                modelo = bll.CarregaFolhaPagamentos(mes_base);
            }

            return View(modelo);
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