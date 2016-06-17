using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.SqlServer;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(SignalRSever.Startup))]

namespace SignalRSever
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            string sqlConnectionString =System.Configuration.ConfigurationManager.ConnectionStrings["DSN_CHAT_SIGNALR"].ConnectionString;
            GlobalHost.DependencyResolver.UseSqlServer(sqlConnectionString);
            // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=316888
            app.MapSignalR();
        }
    }
}
