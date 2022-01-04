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
    public class UsuariosController : Controller
    {
        private DALConexao cx = new DALConexao(DadosConexao.StringConexao);
        // GET: Clientes
        public ActionResult UsuariosList(int? pagina, int? registros, string buscapor, string valor, string ordenapor)
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

            BLLUsuarios bll = new BLLUsuarios(cx);
            int QuantExames = bll.TotalUsuarios();
            double ultima = Convert.ToDouble(QuantExames) / Convert.ToDouble(tamanhoPagina);
            ViewBag.PageUlt = Math.Ceiling(ultima);

            DataTable dt = bll.Localizar(valor, buscapor, Convert.ToInt32(Session["idempresas"]), numeroPagina, tamanhoPagina, ordenapor);


            if (Request.IsAjaxRequest())
            {
                return PartialView("_UsuariosList", dt);
            }

            return View(dt);
        }

        [HttpPost]
        public string ExcluirSelecionados(string check)
        {
            BLLUsuarios bll = new BLLUsuarios(cx);
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

        public ActionResult UsuariosCadastro(int idusuarios)
        {
            if (Convert.ToInt32(Session["idempresas"]) <= 0)
            {
                return RedirectToAction("Index", "Home");
            }

            ModeloUsuario modusuario = new ModeloUsuario();

            if (idusuarios > 0)
            {
                BLLUsuarios bll = new BLLUsuarios(cx);
                modusuario = bll.CarregaUsuario(idusuarios);

                ViewBag.idusuarios = idusuarios;

            }
            else
            {
                modusuario.IdEmpresas = Convert.ToInt32(Session["idempresas"]);
                ViewBag.idusuarios = 0;
            }

            return View(modusuario);
        }

        [HttpPost]
        public ActionResult UsuariosCadastro(ModeloUsuario modUsuarios)
        {
            BLLUsuarios bll = new BLLUsuarios(cx);

            if (ModelState.IsValid == true)
            {
                try
                {
                    if (modUsuarios.IdUsuarios == 0)
                    {
                        bll.Incluir(modUsuarios);
                    }
                    else
                    {
                        bll.Alterar(modUsuarios);
                    }
                }
                catch (Exception erro)
                {
                    return View(modUsuarios).Mensagem("Erro ao salvar registro!\n\n" + erro);
                }

                return RedirectToAction("UsuariosList", "Usuarios").Mensagem("Registro salvo com sucesso!");
            }

            return View(modUsuarios);
        }

        public ActionResult Excluir(int id, ModeloUsuario modUsuarios)
        {
            BLLUsuarios bll = new BLLUsuarios(cx);

            try
            {
                bll.Excluir(id);
            }
            catch (Exception erro)
            {
                return View(modUsuarios).Mensagem("Erro ao excluir registro!\n\n" + erro);
            }

            return RedirectToAction("UsuariosList", "Usuarios").Mensagem("Registro excluído com sucesso!");
        }
    }
}