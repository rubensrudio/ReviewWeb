using BLL;
using DAL;
using Modelo;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Web;
using System.Web.Mvc;

namespace ReviewWeb.Controllers
{
    public class EmpresasController : Controller
    {
        private DALConexao cx = new DALConexao(DadosConexao.StringConexao);
        
        // GET: Empresas
        public ActionResult EmpresasList(int? pagina, int? registros, string buscapor, string valor, string ordenapor)
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

            BLLEmpresas bll = new BLLEmpresas(cx);
            int QuantEmp = bll.TotalEmpresas();
            double ultima = Convert.ToDouble(QuantEmp) / Convert.ToDouble(tamanhoPagina);
            ViewBag.PageUlt = Math.Ceiling(ultima);

            DataTable dt = bll.Localizar(valor, buscapor, numeroPagina, tamanhoPagina, ordenapor);
            

            if (Request.IsAjaxRequest())
            {
                return PartialView("_EmpresasList", dt);
            }

            return View(dt);
        }
        
        [HttpPost]
        public string ExcluirSelecionados(string checkEmpresas)
        {
            BLLEmpresas bll = new BLLEmpresas(cx);
            string[] ids = checkEmpresas.Split(new char[] { ';' });
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
                        msg = "Erro ao excluir!\n\n"+erro.ToString();
                    }
                }
            }

            return msg;
        }


        public ActionResult EmpresasCadastro(int idempresas)
        {
            if (Convert.ToInt32(Session["idempresas"]) <= 0)
            {
                return RedirectToAction("Index", "Home");
            }

            ModeloEmpresa modemp = new ModeloEmpresa();

            if(idempresas > 0)
            {
                BLLEmpresas bll = new BLLEmpresas(cx);
                modemp = bll.CarregaEmpresa(idempresas);

                if (modemp.Nome_Arquivo != "")
                {
                    string[] ext = modemp.Nome_Arquivo.Split('.');
                    ViewData["ext"] = ext[1];
                }
            }

            return View(modemp);
        }

        private static byte[] ConverToBytes(HttpPostedFileBase file)
        {
            int fileSizeInBytes = file.ContentLength; //134675091 (129MB)
            MemoryStream target = new MemoryStream();
            file.InputStream.CopyTo(target);//System.OutOfMemoryException
            byte[] data = target.ToArray();

            return data;
        }

        [HttpPost]
        public ActionResult EmpresasCadastro(ModeloEmpresa modEmp)
        {
            BLLEmpresas bll = new BLLEmpresas(cx);
            int res = bll.VerificaCNPJ(modEmp.CNPJ,modEmp.IdEmpresas);
            
            if (modEmp.wLogo != null)
            {
                modEmp.Logo = ConverToBytes(modEmp.wLogo);

                modEmp.Nome_Arquivo = modEmp.wLogo.FileName;
                
            }

            if(modEmp.Logo == null)
            {
                ModelState.AddModelError("Logo", "Logo é obrigatória!");
            }
            else
            {
                ModelState.Remove("wLogo");
            }

            if (res == 1)
            {
                ModelState.AddModelError("CNPJ", "CNPJ já cadastrado!");
            }

            if (ModelState.IsValid == true)
            {
                try
                {
                    if (modEmp.IdEmpresas == 0)
                    {
                         bll.Incluir(modEmp);
                    }
                    else
                    {
                        bll.Alterar(modEmp);
                    }

                    if (modEmp.Logo != null)
                    {
                        ImageConverter converter = new ImageConverter();
                        Image img = (Image)converter.ConvertFrom(modEmp.Logo);
                        img = Tools.ResizeImage(img, 180, 90, true);

                        string[] ext = modEmp.Nome_Arquivo.Split('.');

                        string nome = "logo_" + modEmp.IdEmpresas + "." + ext[1];
                        img.Save(Server.MapPath("/Images/" + nome), ImageFormat.Png);

                        img.Dispose();
                    }
                }
                catch (Exception erro)
                {
                    return View(modEmp).Mensagem("Erro ao salvar registro!\n\n"+erro);
                }

                return RedirectToAction("EmpresasList", "Empresas").Mensagem("Registro salvo com sucesso!");
            }

            return View(modEmp);
        }
        

        public ActionResult Excluir(int id, ModeloEmpresa modEmp)
        {
            BLLEmpresas bll = new BLLEmpresas(cx);

            try
            {
                bll.Excluir(id);
            }
            catch (Exception erro)
            {
                return View(modEmp).Mensagem("Erro ao excluir registro!\n\n" + erro);
            }

            return RedirectToAction("EmpresasList", "Empresas").Mensagem("Registro excluído com sucesso!");
        }

        
    }
}