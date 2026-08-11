using System.Web.Http;
using PedidosApiMvcEf6.App_Start;

namespace PedidosApiMvcEf6
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
        }
    }
}
