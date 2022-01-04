using System;

namespace DAL
{
    public class DadosConexao
    {
        public static String servidor = "187.109.252.114";
        public static String banco = "speedservicos12";
        public static String usuario = "speed";
        public static String senha = "@speed";
        public static String StringConexao
        {
            get
            {
                string strValue = "";
                System.Configuration.Configuration rootWebConfig =
                    System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("~/");
                System.Configuration.ConnectionStringSettings connString;
                if (0 < rootWebConfig.ConnectionStrings.ConnectionStrings.Count)
                {
                    connString = rootWebConfig.ConnectionStrings.ConnectionStrings["SqlServer"];
                    if (null != connString)
                        strValue = connString.ConnectionString;
                    else
                        strValue = "Data Source=" + servidor + ";Initial Catalog=" + banco + ";User ID=" + usuario + ";Password=" + senha;
                }
                else
                {
                    strValue = "Data Source=" + servidor + ";Initial Catalog=" + banco + ";User ID=" + usuario + ";Password=" + senha;
                }

                DALConexao cx = new DALConexao(strValue);
                try
                {
                    cx.Conectar();
                    cx.Desconectar();
                }
                catch
                {
                    strValue = "Data Source=" + servidor + ";Initial Catalog=" + banco + ";User ID=" + usuario + ";Password=" + senha;
                    try
                    {
                        cx = new DALConexao(strValue);
                        cx.Conectar();
                        cx.Desconectar();
                    }
                    catch
                    {
                        strValue = "";
                    }
                }

                return strValue;
            }
        }
    }
}
