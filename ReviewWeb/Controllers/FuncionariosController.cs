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
    public class FuncionariosController : Controller
    {
        private DALConexao cx = new DALConexao(DadosConexao.StringConexao);
        // GET: Clientes
        public ActionResult FuncionariosList(int? pagina, int? registros, string buscapor, string valor, string ordenapor)
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

            BLLFuncionarios bll = new BLLFuncionarios(cx);
            int QuantFunc = bll.TotalFuncionarios();
            double ultima = Convert.ToDouble(QuantFunc) / Convert.ToDouble(tamanhoPagina);
            ViewBag.PageUlt = Math.Ceiling(ultima);

            DataTable dt = bll.Localizar(valor, buscapor, Convert.ToInt32(Session["idempresas"]), numeroPagina, tamanhoPagina, ordenapor);


            if (Request.IsAjaxRequest())
            {
                return PartialView("_FuncionariosList", dt);
            }

            return View(dt);
        }

        [HttpPost]
        public string ExcluirSelecionados(string check)
        {
            BLLFuncionarios bll = new BLLFuncionarios(cx);
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

        public ActionResult FuncionariosCadastro(int idfuncionarios)
        {
            if (Convert.ToInt32(Session["idempresas"]) <= 0)
            {
                return RedirectToAction("Index", "Home");
            }

            ModeloFuncionarios modfun = new ModeloFuncionarios();

            if (idfuncionarios > 0)
            {
                BLLFuncionarios bll = new BLLFuncionarios(cx);
                modfun = bll.CarregaFuncionarios(idfuncionarios);

                ViewBag.idfuncionarios = idfuncionarios;

                BLLCargos bll2 = new BLLCargos(cx);
                DataTable dt = bll2.Localizar("","");

                ViewBag.Cargos = dt.Rows;

            }
            else
            {
                BLLCargos bll2 = new BLLCargos(cx);
                DataTable dt = bll2.Localizar("", "");

                ViewBag.Cargos = dt.Rows;

                modfun.IdEmpresas = Convert.ToInt32(Session["idempresas"]);
            }

            return View(modfun);
        }

        [HttpPost]
        public ActionResult FuncionariosCadastro(ModeloFuncionarios modFun)
        {
            BLLFuncionarios bll = new BLLFuncionarios(cx);
            int res = bll.VerificaCPF(modFun.CPF, modFun.IdEmpresas);

            BLLCargos bll2 = new BLLCargos(cx);
            DataTable dt = bll2.Localizar("", "");

            ViewBag.Cargos = dt.Rows;

            if (res == 1)
            {
                ModelState.AddModelError("CPF", "CPF já cadastrado!");
            }

            if (ModelState.IsValid == true)
            {
                try
                {
                    if (modFun.IdFuncionarios == 0)
                    {
                        bll.Incluir(modFun);
                    }
                    else
                    {
                        bll.Alterar(modFun);
                    }
                }
                catch (Exception erro)
                {
                    return View(modFun).Mensagem("Erro ao salvar registro!\n\n" + erro);
                }

                return RedirectToAction("FuncionariosList", "Funcionarios").Mensagem("Registro salvo com sucesso!");
            }

            return View(modFun);
        }

        public ActionResult Excluir(int id, ModeloFuncionarios modFun)
        {
            BLLFuncionarios bll = new BLLFuncionarios(cx);

            try
            {
                bll.Excluir(id);
            }
            catch (Exception erro)
            {
                return View(modFun).Mensagem("Erro ao excluir registro!\n\n" + erro);
            }

            return RedirectToAction("FuncionariosList", "Funcionarios").Mensagem("Registro excluído com sucesso!");
        }
    }
}