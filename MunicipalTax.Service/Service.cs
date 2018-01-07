using Microsoft.Owin.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topshelf;

namespace MunicipalTax.Service
{
    public class Service
    {
        private IDisposable _instance;

        public bool Start()
        {
            string baseAddress = "http://localhost:9000/";

            _instance = WebApp.Start<Startup>(url: baseAddress);
            return true;
        }

        public bool Stop()
        {
            _instance.Dispose();
            _instance = null;
            return true;
        }
    }
}
