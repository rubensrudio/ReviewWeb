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
    public class ClientesController : Controller
    {
        private DALConexao cx = new DALConexao(DadosConexao.StringConexao);
        // GET: Clientes
        public ActionResult ClientesList(int? pagina, int? registros, string buscapor, string valor, string ordenapor)
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

            BLLCliente bll = new BLLCliente(cx);
            int QuantEmp = bll.TotalClientes();
            double ultima = Convert.ToDouble(QuantEmp) / Convert.ToDouble(tamanhoPagina);
            ViewBag.PageUlt = Math.Ceiling(ultima);

            DataTable dt = bll.Localizar(valor, buscapor, Convert.ToInt32(Session["idempresas"]), "cl", numeroPagina, tamanhoPagina, ordenapor);


            if (Request.IsAjaxRequest())
            {
                return PartialView("_ClientesList", dt);
            }

            return View(dt);
        }

        [HttpPost]
        public string ExcluirSelecionados(string checkClientes)
        {
            BLLCliente bll = new BLLCliente(cx);
            string[] ids = checkClientes.Split(new char[] { ';' });
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

        public ActionResult ClientesCadastro(int idclientes, string cidade, string uf)
        {
            if (Convert.ToInt32(Session["idempresas"]) <= 0)
            {
                return RedirectToAction("Index", "Home");
            }

            ModeloCliente modcli = new ModeloCliente();

            if (idclientes > 0)
            {
                BLLCliente bll = new BLLCliente(cx);
                modcli = bll.CarregaCliente(idclientes);

                BLLLocaisAtuacao bll2 = new BLLLocaisAtuacao(cx);
                DataTable dt = bll2.CarregarLocaisAtuacao(idclientes);

                ViewBag.Model2 = dt;
                ViewBag.idclientes = idclientes;
                
            }
            else
            {
                modcli.IdEmpresas = Convert.ToInt32(Session["idempresas"]);
            }

            return View(modcli);
        }

        public PartialViewResult LocaisAtuacaoInserir(int idclientes, string cidade, string uf)
        {
            BLLLocaisAtuacao bll2 = new BLLLocaisAtuacao(cx);
            
            ViewBag.idclientes = idclientes;

            if (Request.IsAjaxRequest())
            {
                if ((cidade != "") && (idclientes > 0))
                {
                    ModeloLocaisAtuacao modloc = new ModeloLocaisAtuacao();
                    modloc.IdClientes = idclientes;
                    modloc.Cidade = cidade;
                    modloc.UF = uf;
                    string msg = "";

                    try
                    {
                        bll2.Incluir(modloc);
                    }
                    catch (Exception erro)
                    {
                        msg = "Erro ao excluir!\n\n" + erro.ToString();
                    }

                    DataTable dt = bll2.CarregarLocaisAtuacao(idclientes);
                    ViewBag.Model2 = dt;
                }
                
            }

            return PartialView("_LocaisAtuacao");
        }

        public PartialViewResult LocaisAtuacaoExcluir(int idclientes, int idclientes_localidades)
        {
            BLLLocaisAtuacao bll2 = new BLLLocaisAtuacao(cx);

            ViewBag.idclientes = idclientes;

            if (Request.IsAjaxRequest())
            {
                if ((idclientes_localidades > 0) && (idclientes > 0))
                {
                    string msg = "";
                    try
                    {
                        bll2.Excluir(idclientes_localidades);
                    }
                    catch (Exception erro)
                    {
                        msg = "Erro ao excluir!\n\n" + erro.ToString();
                    }

                    DataTable dt = bll2.CarregarLocaisAtuacao(idclientes);
                    ViewBag.Model2 = dt;
                }
            }

            return PartialView("_LocaisAtuacao");
        }

        [HttpPost]
        public ActionResult ClientesCadastro(ModeloCliente modCli)
        {
            BLLCliente bll = new BLLCliente(cx);
            int res = bll.VerificaCNPJ(modCli.CNPJ, modCli.IdEmpresas);

            if (res == 1)
            {
                ModelState.AddModelError("CNPJ", "CNPJ já cadastrado!");
            }

            if (ModelState.IsValid == true)
            {
                try
                {
                    if (modCli.IdClientes == 0)
                    {
                        bll.Incluir(modCli);
                    }
                    else
                    {
                        bll.Alterar(modCli);
                    }
                }
                catch (Exception erro)
                {
                    return View(modCli).Mensagem("Erro ao salvar registro!\n\n" + erro);
                }

                return RedirectToAction("ClientesList", "Clientes").Mensagem("Registro salvo com sucesso!");
            }

            return View(modCli);
        }

        public ActionResult Excluir(int id, ModeloCliente modCli)
        {
            BLLCliente bll = new BLLCliente(cx);

            try
            {
                bll.Excluir(id);
            }
            catch (Exception erro)
            {
                return View(modCli).Mensagem("Erro ao excluir registro!\n\n" + erro);
            }

            return RedirectToAction("ClientesList", "Clientes").Mensagem("Registro excluído com sucesso!");
        }
    }
}