using System.Web.Http;
using BibliotecaApiMvcEf6.App_Start;

namespace BibliotecaApiMvcEf6
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
        }
    }
}
