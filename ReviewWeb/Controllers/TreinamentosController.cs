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
    public class TreinamentosController : Controller
    {
        private DALConexao cx = new DALConexao(DadosConexao.StringConexao);
        // GET: Clientes
        public ActionResult TreinamentosList(int? pagina, int? registros, string buscapor, string valor, string ordenapor)
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

            BLLTreinamentos bll = new BLLTreinamentos(cx);
            int QuantTreinamentos = bll.TotalTreinamentos();
            double ultima = Convert.ToDouble(QuantTreinamentos) / Convert.ToDouble(tamanhoPagina);
            ViewBag.PageUlt = Math.Ceiling(ultima);

            DataTable dt = bll.Localizar(valor, buscapor, Convert.ToInt32(Session["idempresas"]), numeroPagina, tamanhoPagina, ordenapor);


            if (Request.IsAjaxRequest())
            {
                return PartialView("_TreinamentosList", dt);
            }

            return View(dt);
        }

        [HttpPost]
        public string ExcluirSelecionados(string check)
        {
            BLLTreinamentos bll = new BLLTreinamentos(cx);
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

        public ActionResult TreinamentosCadastro(int idtreinamentos)
        {
            if (Convert.ToInt32(Session["idempresas"]) <= 0)
            {
                return RedirectToAction("Index", "Home");
            }

            ModeloTreinamentos modexame = new ModeloTreinamentos();

            if (idtreinamentos > 0)
            {
                BLLTreinamentos bll = new BLLTreinamentos(cx);
                modexame = bll.CarregaTreinamentos(idtreinamentos);

                ViewBag.idtreinamentos = idtreinamentos;

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
        public ActionResult TreinamentosCadastro(ModeloTreinamentos modTreinamentos)
        {
            BLLTreinamentos bll = new BLLTreinamentos(cx);

            if (ModelState.IsValid == true)
            {
                try
                {
                    if (modTreinamentos.IdTreinamentos == 0)
                    {
                        bll.Incluir(modTreinamentos);
                    }
                    else
                    {
                        bll.Alterar(modTreinamentos);
                    }
                }
                catch (Exception erro)
                {
                    return View(modTreinamentos).Mensagem("Erro ao salvar registro!\n\n" + erro);
                }

                return RedirectToAction("TreinamentosList", "Treinamentos").Mensagem("Registro salvo com sucesso!");
            }

            return View(modTreinamentos);
        }

        public ActionResult Excluir(int id, ModeloTreinamentos modTreinamentos)
        {
            BLLTreinamentos bll = new BLLTreinamentos(cx);

            try
            {
                bll.Excluir(id);
            }
            catch (Exception erro)
            {
                return View(modTreinamentos).Mensagem("Erro ao excluir registro!\n\n" + erro);
            }

            return RedirectToAction("TreinamentosList", "Treinamentos").Mensagem("Registro excluído com sucesso!");
        }
    }
}