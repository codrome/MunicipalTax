using Microsoft.Owin.Hosting;
using MunicipalTax.Service;
using MunicpalTax.Data;
using System;
using System.Net.Http;
using Topshelf;
//using TopShelf.Owin;

namespace OwinSelfhostSample
{
    public class Program
    {
        static void Main()
        {
            var rc = HostFactory.Run(x =>
            {
                x.Service<Service>(s =>
                {
                    s.ConstructUsing(name => new Service());
                    s.WhenStarted(tc => tc.Start());
                    s.WhenStopped(tc => tc.Stop());
                });

                x.RunAsPrompt();
                x.StartManually();

                //x.Service<Service>(s =>
                //{
                //    s.ConstructUsing(() => new Service());
                //    s.WhenStarted((service, control) => service.Start());
                //    s.WhenStopped((service, control) => service.Stop());

                //    s.OwinEndpoint(app =>
                //    {
                //        app.Domain = "localhost";
                //        app.Port = 9000;

                //        var startup = new Startup();
                //        app.ConfigureAppBuilder(builder => startup.Configuration(builder));
                //    });
                //});

                x.SetDescription("Municipal Tax Rate service host");
                x.SetDisplayName("Municipal tax rate service");
                x.SetServiceName("Municipal.Tax.Rate.Service");
            });

            var exitCode = (int)Convert.ChangeType(rc, rc.GetTypeCode());
            Environment.ExitCode = exitCode;

            //string baseAddress = "http://localhost:9000/";

            //using (WebApp.Start<Startup>(url: baseAddress))
            //{
            //    Console.WriteLine("Hit <Enter> to stop service.");
            //    Console.ReadLine();
            //}
        }
    }
}