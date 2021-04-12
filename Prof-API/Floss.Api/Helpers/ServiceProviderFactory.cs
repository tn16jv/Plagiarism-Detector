using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Floss.Api.Helpers
{
    public static class ServiceProviderFactory
    {

        public static IServiceCollection ServicesCollection { get; set; }
        public static IServiceProvider ServiceProvider { get {return ServicesCollection.BuildServiceProvider(); } }
      
    }
}
