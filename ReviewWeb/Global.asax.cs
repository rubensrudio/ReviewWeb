using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace ReviewWeb
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            // Extensão Mensagem para ActionResult
            ClientDataTypeModelValidatorProvider.ResourceClassKey = "Mensagens";
            DefaultModelBinder.ResourceClassKey = "Mensagens";

            ModelBinders.Binders.Add(typeof(double), new DoubleModelBinder());
            ModelBinders.Binders.Add(typeof(double?), new DoubleModelBinder());
            ModelBinders.Binders.Add(typeof(int), new Int32ModelBinder());
            ModelBinders.Binders.Add(typeof(int?), new Int32ModelBinder());
            ModelBinders.Binders.Add(typeof(decimal), new DecimalModelBinder());
            ModelBinders.Binders.Add(typeof(decimal?), new DecimalModelBinder());

            HtmlHelper.ClientValidationEnabled = true;
            HtmlHelper.UnobtrusiveJavaScriptEnabled = true;
        }
    }
}
