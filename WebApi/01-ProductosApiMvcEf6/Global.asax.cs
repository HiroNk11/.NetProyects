using System.Web.Http;
using ProductosApiMvcEf6.App_Start;

namespace ProductosApiMvcEf6
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
        }
    }
}
