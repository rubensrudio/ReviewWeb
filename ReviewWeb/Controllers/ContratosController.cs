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
    public class ContratosController : Controller
    {
        private DALConexao cx = new DALConexao(DadosConexao.StringConexao);
        // GET: Clientes
        public ActionResult ContratosList(int? pagina, int? registros, string buscapor, string valor, string ordenapor)
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

            BLLContratos bll = new BLLContratos(cx);
            int Quant = bll.TotalContratos(Convert.ToInt32(Session["idempresas"]));
            double ultima = Convert.ToDouble(Quant) / Convert.ToDouble(tamanhoPagina);
            ViewBag.PageUlt = Math.Ceiling(ultima);

            DataTable dt = bll.Localizar(valor, buscapor, Convert.ToInt32(Session["idempresas"]), numeroPagina, tamanhoPagina, ordenapor);


            if (Request.IsAjaxRequest())
            {
                return PartialView("_ContratosList", dt);
            }

            return View(dt);
        }

        [HttpPost]
        public string ExcluirSelecionados(string check)
        {
            BLLContratos bll = new BLLContratos(cx);
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

        public ActionResult ContratosCadastro(int idcontratos)
        {
            if (Convert.ToInt32(Session["idempresas"]) <= 0)
            {
                return RedirectToAction("Index", "Home");
            }

            ModeloContratos modelo = new ModeloContratos();
            BLLCliente bllcli = new BLLCliente(cx);
            DataTable dt = bllcli.Localizar("", "Nome Fantasia", Convert.ToInt32(Session["idempresas"]));
            var lista = new List<SelectListItem>();
            
            if (idcontratos > 0)
            {
                BLLContratos bll = new BLLContratos(cx);
                modelo = bll.CarregaContratos(idcontratos);
                BLLLocaisAtuacao bll2 = new BLLLocaisAtuacao(cx);
                ModeloLocaisAtuacao modLocal = bll2.LocalizarLocalAtuacao(modelo.IdContratos_Clientes_Localidades);

                try
                {
                    foreach (DataRow item in dt.Rows)
                    {
                        var option = new SelectListItem()
                        {
                            Text = item["nome_fantasia"].ToString(),
                            Value = item["idclientes"].ToString(),
                            Selected = (Convert.ToInt32(item["idclientes"]) == modLocal.IdClientes)
                        };

                        lista.Add(option);
                    }

                    ViewBag.Clientes = lista;
                }
                catch (Exception erro)
                {

                }

                ViewBag.idcontratos = idcontratos;

            }
            else
            {
                try
                {
                    foreach (DataRow item in dt.Rows)
                    {
                        var option = new SelectListItem()
                        {
                            Text = item["nome_fantasia"].ToString(),
                            Value = item["idclientes"].ToString()
                        };

                        lista.Add(option);
                    }

                    ViewBag.Clientes = lista;
                }
                catch (Exception erro)
                {

                }

                ViewBag.idcontratos = idcontratos;
            }

            return View(modelo);
        }

        public ActionResult LocaisList(int idclientes, int idcontratos)
        {
            var lista = new List<SelectListItem>();
            BLLLocaisAtuacao bll = new BLLLocaisAtuacao(cx);
            DataTable dt = bll.CarregarLocaisAtuacao(idclientes);
            
            BLLContratos bll2 = new BLLContratos(cx);
            ModeloContratos modelo = bll2.CarregaContratos(idcontratos);

            if (idcontratos > 0)
            {
                try
                {
                    foreach (DataRow item in dt.Rows)
                    {
                        var option = new SelectListItem()
                        {
                            Text = item["cidade"].ToString() + " - " + item["uf"].ToString(),
                            Value = item["idclientes_localidades"].ToString(),
                            Selected = (Convert.ToInt32(item["idclientes_localidades"]) == modelo.IdContratos_Clientes_Localidades)
                        };

                        lista.Add(option);
                    }
                }
                catch (Exception err)
                {

                }
            }
            else
            {
                try
                {
                    foreach (DataRow item in dt.Rows)
                    {
                        var option = new SelectListItem()
                        {
                            Text = item["cidade"].ToString() + " - " + item["uf"].ToString(),
                            Value = item["idclientes_localidades"].ToString()
                        };

                        lista.Add(option);
                    }
                }
                catch (Exception err)
                {

                }
            }

            return Json(new { Resultado = lista }, JsonRequestBehavior.AllowGet);
        }



        [HttpPost]
        public ActionResult ContratosCadastro(ModeloContratos modelo)
        {
            BLLContratos bll = new BLLContratos(cx);

            if (ModelState.IsValid == true)
            {
                try
                {
                    if (modelo.IdContratos == 0)
                    {
                        bll.Incluir(modelo);
                    }
                    else
                    {
                        BLLFuncionarios bll2 = new BLLFuncionarios(cx);

                        DataTable dt = bll2.Localizar("", "");
                        if (modelo.Ativo == 0)
                        {
                            foreach (DataRow row in dt.Rows)
                            {
                                if (row["idfuncionarios"].ToString() != "")
                                {
                                    if (bll.VerificaFuncionarios(modelo.IdContratos, Convert.ToInt32(row["idfuncionarios"])) == 1)
                                    {
                                        return RedirectToAction("ContratosList", "Contratos").Mensagem("Esse contrato não pode ser inativado pois possui colaboradores no contrato!");
                                    }
                                }
                            }
                        }

                        bll.Alterar(modelo);
                    }
                }
                catch (Exception erro)
                {
                    return RedirectToAction("ContratosList", "Contratos").Mensagem("Erro!" + erro);
                    //return View(modelo).Mensagem("Erro ao salvar registro!\n\n" + erro);
                }

                return RedirectToAction("ContratosList", "Contratos").Mensagem("Registro salvo com sucesso!");
            }

            return View(modelo);
        }

        public ActionResult Excluir(int id, ModeloContratos modelo)
        {
            BLLContratos bll = new BLLContratos(cx);

            try
            {
                bll.Excluir(id);
            }
            catch (Exception erro)
            {
                return View(modelo).Mensagem("Erro ao excluir registro!\n\n" + erro);
            }

            return RedirectToAction("ContratosList", "Contratos").Mensagem("Registro excluído com sucesso!");
        }
    }
}