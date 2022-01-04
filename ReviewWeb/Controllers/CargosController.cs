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
    public class CargosController : Controller
    {
        private DALConexao cx = new DALConexao(DadosConexao.StringConexao);
        // GET: Cargos
        public ActionResult CargosList(int? pagina, int? registros, string buscapor, string valor, string ordenapor)
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

            BLLCargos bll = new BLLCargos(cx);
            int QuantCargos = bll.TotalCargos();
            double ultima = Convert.ToDouble(QuantCargos) / Convert.ToDouble(tamanhoPagina);
            ViewBag.PageUlt = Math.Ceiling(ultima);

            DataTable dt = bll.Localizar(valor, buscapor, Convert.ToInt32(Session["idempresas"]), numeroPagina, tamanhoPagina, ordenapor);


            if (Request.IsAjaxRequest())
            {
                return PartialView("_CargosList", dt);
            }

            return View(dt);
        }

        [HttpPost]
        public string ExcluirSelecionados(string check)
        {
            BLLCargos bll = new BLLCargos(cx);
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

        public ActionResult CargosCadastro(int idcargos)
        {
            if (Convert.ToInt32(Session["idempresas"]) <= 0)
            {
                return RedirectToAction("Index", "Home");
            }

            ModeloCargos modelo = new ModeloCargos();

            if (idcargos > 0)
            {
                BLLCargos bll = new BLLCargos(cx);
                modelo = bll.CarregaCargos(idcargos);

                ViewBag.idcargos = idcargos;

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

            return View(modelo);
        }

        [HttpPost]
        public ActionResult CargosCadastro(ModeloCargos modCargos)
        {
            BLLCargos bll = new BLLCargos(cx);

            modCargos.IdEmpresas = Convert.ToInt32(Session["idempresas"]);

            if (ModelState.IsValid == true)
            {
                try
                {
                    if (modCargos.IdCargos == 0)
                    {
                        bll.Incluir(modCargos);
                    }
                    else
                    {
                        bll.Alterar(modCargos);
                    }
                }
                catch (Exception erro)
                {
                    return View(modCargos).Mensagem("Erro ao salvar registro!\n\n" + erro);
                }

                return RedirectToAction("CargosList", "Cargos").Mensagem("Registro salvo com sucesso!");
            }

            return View(modCargos);
        }

        public ActionResult Excluir(int id, ModeloCargos modCargos)
        {
            BLLCargos bll = new BLLCargos(cx);

            try
            {
                bll.Excluir(id);
            }
            catch (Exception erro)
            {
                return View(modCargos).Mensagem("Erro ao excluir registro!\n\n" + erro);
            }

            return RedirectToAction("CargosList", "Cargos").Mensagem("Registro excluído com sucesso!");
        }
    }
}