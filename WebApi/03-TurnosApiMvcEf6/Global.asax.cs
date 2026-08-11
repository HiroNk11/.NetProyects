using System.Web.Http;
using TurnosApiMvcEf6.App_Start;

namespace TurnosApiMvcEf6
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
        }
    }
}
