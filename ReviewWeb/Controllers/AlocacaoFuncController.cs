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
    public class AlocacaoFuncController : Controller
    {
        private DALConexao cx = new DALConexao(DadosConexao.StringConexao);
        // GET: Clientes
        public ActionResult AlocacaoFuncList(int? pagina, int? registros, string buscapor, string valor, string ordenapor)
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

            BLLAlocacaoFunc bll = new BLLAlocacaoFunc(cx);
            int QuantAlocacaoFunc = bll.TotalAlocacaoFunc(valor,buscapor, Convert.ToInt32(Session["idempresas"]));
            double ultima = Convert.ToDouble(QuantAlocacaoFunc) / Convert.ToDouble(tamanhoPagina);
            ViewBag.PageUlt = Math.Ceiling(ultima);

            DataTable dt = bll.Localizar(valor, buscapor, Convert.ToInt32(Session["idempresas"]), numeroPagina, tamanhoPagina, ordenapor);


            if (Request.IsAjaxRequest())
            {
                return PartialView("_AlocacaoFuncList", dt);
            }

            return View(dt);
        }

        [HttpPost]
        public string ExcluirSelecionados(string check)
        {
            BLLAlocacaoFunc bll = new BLLAlocacaoFunc(cx);
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

        public ActionResult AlocacaoFuncCadastro(int idalocacao_func)
        {
            if (Convert.ToInt32(Session["idempresas"]) <= 0)
            {
                return RedirectToAction("Index", "Home");
            }

            ModeloAlocacaoFunc modelo = new ModeloAlocacaoFunc();

            if (idalocacao_func > 0)
            {
                BLLAlocacaoFunc bll = new BLLAlocacaoFunc(cx);
                modelo = bll.CarregaAlocacaoFunc(idalocacao_func);

                ViewBag.idalocacao_func = idalocacao_func;
            }

            return View(modelo);
        }

        [HttpPost]
        public ActionResult AlocacaoFuncCadastro(ModeloAlocacaoFunc modelo)
        {
            BLLAlocacaoFunc bll = new BLLAlocacaoFunc(cx);

            if (ModelState.IsValid == true)
            {
                try
                {
                    bll.Alterar(modelo);
                }
                catch (Exception erro)
                {
                    return View(modelo).Mensagem("Erro ao salvar registro!\n\n" + erro);
                }

                return RedirectToAction("AlocacaoFuncList", "AlocacaoFunc").Mensagem("Registro salvo com sucesso!");
            }

            return View(modelo);
        }

        public ActionResult Excluir(int id, ModeloAlocacaoFunc modelo)
        {
            BLLAlocacaoFunc bll = new BLLAlocacaoFunc(cx);

            try
            {
                bll.Excluir(id);
            }
            catch (Exception erro)
            {
                return View(modelo).Mensagem("Erro ao excluir registro!\n\n" + erro);
            }

            return RedirectToAction("AlocacaoFuncList", "AlocacaoFunc").Mensagem("Registro excluído com sucesso!");
        }
    }
}