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
    public class AlocacaoPermissaoController : Controller
    {
        private DALConexao cx = new DALConexao(DadosConexao.StringConexao);
        // GET: Clientes
        public ActionResult AlocacaoPermissaoList(int? pagina, int? registros, string buscapor, string valor, string ordenapor)
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

            BLLAlocacaoPermissao bll = new BLLAlocacaoPermissao(cx);
            int Quant = bll.TotalPermissao();
            double ultima = Convert.ToDouble(Quant) / Convert.ToDouble(tamanhoPagina);
            ViewBag.PageUlt = Math.Ceiling(ultima);

            DataTable dt = bll.Localizar(valor, buscapor, Convert.ToInt32(Session["idempresas"]), numeroPagina, tamanhoPagina, ordenapor);
            
            if (Request.IsAjaxRequest())
            {
                return PartialView("_AlocacaoPermissaoList", dt);
            }

            return View(dt);
        }

        [HttpPost]
        public string ExcluirSelecionados(string check)
        {
            BLLAlocacaoPermissao bll = new BLLAlocacaoPermissao(cx);
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

        public ActionResult AlocacaoPermissaoCadastro(int idalocacao_permissao)
        {
            if (Convert.ToInt32(Session["idempresas"]) <= 0)
            {
                return RedirectToAction("Index", "Home");
            }

            ModeloAlocacaoPermissao modelo = new ModeloAlocacaoPermissao();
            BLLAlocacaoPermissao bll = new BLLAlocacaoPermissao(cx);

            if (idalocacao_permissao > 0)
            {
                modelo = bll.CarregaPermissoes(idalocacao_permissao);
            }

            BLLUsuarios bll2 = new BLLUsuarios(cx);
            DataTable dt = bll2.Localizar("","",Convert.ToInt32(Session["idempresas"]));

            ViewBag.Usuarios = dt.Rows;

            return View(modelo);
        }
               

        [HttpPost]
        public ActionResult AlocacaoPermissaoCadastro(ModeloAlocacaoPermissao modelo)
        {
            BLLAlocacaoPermissao bll = new BLLAlocacaoPermissao(cx);

            BLLUsuarios bll2 = new BLLUsuarios(cx);
            DataTable dt = bll2.Localizar("", "", Convert.ToInt32(Session["idempresas"]));

            ViewBag.Usuarios = dt.Rows;

            if (ModelState.IsValid == true)
            {
                try
                {
                    if (modelo.IdAlocacaoPermissao == 0)
                    {
                        bll.Incluir(modelo);
                    }
                    else
                    {
                        bll.Alterar(modelo);
                    }
                }
                catch (Exception erro)
                {
                    return View(modelo).Mensagem("Erro ao salvar registro!\n\n" + erro);
                }

                return RedirectToAction("AlocacaoPermissaoList", "AlocacaoPermissao").Mensagem("Registro salvo com sucesso!");
            }

            return View(modelo);
        }

        public ActionResult Excluir(int id, ModeloAlocacaoPermissao modelo)
        {
            BLLAlocacaoPermissao bll = new BLLAlocacaoPermissao(cx);

            try
            {
                bll.Excluir(id);
            }
            catch (Exception erro)
            {
                return View(modelo).Mensagem("Erro ao excluir registro!\n\n" + erro);
            }

            return RedirectToAction("AlocacaoPermissaoList", "AlocacaoPermissao").Mensagem("Registro excluído com sucesso!");
        }
    }
}