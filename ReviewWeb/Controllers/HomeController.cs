using BLL;
using DAL;
using Modelo;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Imaging;
using System.Web.Mvc;

namespace ReviewWeb.Controllers
{
    public class HomeController : Controller
    {
        private DALConexao cx = new DALConexao(DadosConexao.StringConexao);
        private ModeloUsuario modusuario;

        public ActionResult Index()
        {
            ViewBag.Title = "Login";

            ModeloUsuario modusu = new ModeloUsuario();
            modusu.Usuario = "";
            modusu.Senha = "";

            return View(modusu);
        }

        [HttpPost]
        public ActionResult Index(ModeloUsuario modusu)
        {
            ViewBag.Title = "Login";

            if (string.IsNullOrEmpty(modusu.Usuario))
            {
                ModelState.AddModelError("Usuario", "Usuário é obrigatório!");
            }

            if (string.IsNullOrEmpty(modusu.Senha))
            {
                ModelState.AddModelError("Senha", "Senha é obrigatória!");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    BLLLogin bll = new BLLLogin(cx);

                    modusuario = bll.Login(modusu.Usuario, modusu.Senha);

                    if (!string.IsNullOrEmpty(modusuario.Usuario))
                    {
                        BLLModulos bll2 = new BLLModulos(cx);
                        BLLItemModulo bll3 = new BLLItemModulo(cx);
                        DataTable dt = bll2.CarregarModulosUsuario(modusuario.IdUsuarios);

                        List<string> list = new List<string>();
                        List<string> listItem = new List<string>();
                        foreach (DataRow dr in dt.Rows)
                        {
                            list.Add(dr["modulo"].ToString());

                            DataTable dt2 = bll3.CarregarItensModulosUsuario(modusuario.IdUsuarios, Convert.ToInt32(dr["idmodulos"]));
                            foreach (DataRow dr2 in dt2.Rows)
                            {
                                listItem.Add(dr["modulo"].ToString() + "-" + dr2["item"].ToString() + "-" + dr2["controller"].ToString() + "-" + dr2["action"].ToString());
                            }
                        }
                        
                        Session["Modulos"] = list;
                        Session["ItemModulos"] = listItem;
                        Session["idusuarios"] = modusuario.IdUsuarios;
                        Session["idempresas"] = modusuario.IdEmpresas;
                        Session["usuario"] = modusuario.Usuario;
                        Session["nome"] = modusuario.Nome;

                        BLLEmpresas bll4 = new BLLEmpresas(cx);
                        ModeloEmpresa modempresa = bll4.CarregaEmpresa(modusuario.IdEmpresas);

                        if (modempresa.Logo.ToString() != "")
                        {
                            string[] ext = modempresa.Nome_Arquivo.Split('.');

                            string nome = "logo_" + modempresa.IdEmpresas + "." + ext[1];
                            Session["logo"] = nome;
                        }
                        else
                        {
                            //btEmpresa.Text = registro["nome_fantasia"].ToString();
                        }

                        return View("Corpo", modusuario);
                    }
                    else
                    {
                        ViewBag.Erro = "Usuário e/ou Senha inválidos!";
                    }
                }
                catch (SqlException errosql)
                {
                    ViewBag.Erro = "SqlException!\n\n" + errosql.ToString();
                }
                catch (Exception erro)
                {
                    ViewBag.Erro = "ExceptionError!\n\n" + erro.ToString();
                }
            }

            return View(modusu);
        }

        public ActionResult Corpo(ModeloUsuario modusu)
        {
            return View(modusu);
        }

        public ActionResult About()
        {
            ViewBag.Title = "Sobre";
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Menu()
        {
            
            return PartialView();
        }

        public ActionResult Menu2()
        {

            return PartialView();
        }

        public ActionResult Logoff()
        {
            Session.Abandon();
            Session.Clear();
            Session.RemoveAll();

            return View("Index");
        }
    }
}