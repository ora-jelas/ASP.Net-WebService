# ASP.Net-WebService
# Using a Factory

/*
// Within Startup.cs:
services.AddSingleton<IAFactory, AFactory>();
            //services.AddTransient<IAFactory, AFactory>();
            //services.AddTransient<AClass>();
services.AddScoped<AClass>();
            //services.AddTransient<IAClass>(x => x.GetRequiredService<IAFactory>().GetObject());
*/

using System;
using Microsoft.Extensions.DependencyInjection;

namespace ANamespace
{
    public interface IAFactory
    {
        //IAClass GetObject();
IAClass GetObject(Flag flag);
    }

    public class AFactory : IAFactory
    {
        //private readonly AClass aObject;
        //private readonly IConfigQuery configQuery;
//private readonly IServiceProvider serviceProvider;
private readonly IServiceScopeFactory serviceScopeFactory;

        //public AFactory(IConfigQuery query, AClass aObject/*, AAnotherClass aAnotherObject*/)
//public AFactory(IServiceProvider serviceProvider)
public AFactory(IServiceScopeFactory serviceScopeFactory)
        {
            //this.configQuery = query;
            //this.aObject = aObject;
            //this.aAnotherObject = aAnotherObject;
//this.serviceProvider = serviceProvider
this.serviceScopeFactory = serviceScopeFactory;
        }

        //public IAClass GetObject()
public IAClass GetObject(Flag flag)
        {
            //var flag = configQuery.GetSomething();

            if (flag != null && flag == "Y")
            {
                //return aObject;

//var serviceScopeFactory = serviceProvider.GetRequiredService<IServiceScopeFactory>();
using (var scope = serviceScopeFactory.CreateScope())
{
return scope.ServiceProvider.GetRequiredService<AClass>();
}
            }
            return null;
        }
    }
}
