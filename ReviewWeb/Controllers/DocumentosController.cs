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
    public class DocumentosController : Controller
    {
        private DALConexao cx = new DALConexao(DadosConexao.StringConexao);
        // GET: Clientes
        public ActionResult DocumentosList(int? pagina, int? registros, string buscapor, string valor, string ordenapor)
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

            BLLDocumentos bll = new BLLDocumentos(cx);
            int QuantDocumentos = bll.TotalDocumentos();
            double ultima = Convert.ToDouble(QuantDocumentos) / Convert.ToDouble(tamanhoPagina);
            ViewBag.PageUlt = Math.Ceiling(ultima);

            DataTable dt = bll.Localizar(valor, buscapor, Convert.ToInt32(Session["idempresas"]), numeroPagina, tamanhoPagina, ordenapor);


            if (Request.IsAjaxRequest())
            {
                return PartialView("_DocumentosList", dt);
            }

            return View(dt);
        }

        [HttpPost]
        public string ExcluirSelecionados(string check)
        {
            BLLDocumentos bll = new BLLDocumentos(cx);
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

        public ActionResult DocumentosCadastro(int iddocumentos)
        {
            if (Convert.ToInt32(Session["idempresas"]) <= 0)
            {
                return RedirectToAction("Index", "Home");
            }

            ModeloDocumentos moddocumentos = new ModeloDocumentos();

            if (iddocumentos > 0)
            {
                BLLDocumentos bll = new BLLDocumentos(cx);
                moddocumentos = bll.CarregaDocumentos(iddocumentos);

                ViewBag.iddocumentos = iddocumentos;
            }
            else
            {
                moddocumentos.IdEmpresas = Convert.ToInt32(Session["idempresas"]);
            }

            return View(moddocumentos);
        }

        [HttpPost]
        public ActionResult DocumentosCadastro(ModeloDocumentos modDocumentos)
        {
            BLLDocumentos bll = new BLLDocumentos(cx);

            if (ModelState.IsValid == true)
            {
                try
                {
                    if (modDocumentos.IdDocumentos == 0)
                    {
                        bll.Incluir(modDocumentos);
                    }
                    else
                    {
                        bll.Alterar(modDocumentos);
                    }
                }
                catch (Exception erro)
                {
                    return View(modDocumentos).Mensagem("Erro ao salvar registro!\n\n" + erro);
                }

                return RedirectToAction("DocumentosList", "Documentos").Mensagem("Registro salvo com sucesso!");
            }

            return View(modDocumentos);
        }

        public ActionResult Excluir(int id, ModeloDocumentos modDocumentos)
        {
            BLLDocumentos bll = new BLLDocumentos(cx);

            try
            {
                bll.Excluir(id);
            }
            catch (Exception erro)
            {
                return View(modDocumentos).Mensagem("Erro ao excluir registro!\n\n" + erro);
            }

            return RedirectToAction("DocumentosList", "Documentos").Mensagem("Registro excluído com sucesso!");
        }
    }
}