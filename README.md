# stashbox-web-webapi [![Build status](https://ci.appveyor.com/api/projects/status/3c4fv3c94f9cpfa1/branch/master?svg=true)](https://ci.appveyor.com/project/pcsajtai/stashbox-web-webapi/branch/master) [![NuGet Version](https://buildstats.info/nuget/Stashbox.Web.WebApi)](https://www.nuget.org/packages/Stashbox.Web.WebApi/)
ASP.NET Web API integration for [Stashbox](https://github.com/z4kn4fein/stashbox)

## Registering in Global.asax
```c#
public class WebApiApplication : System.Web.HttpApplication
{
    protected void Application_Start()
    {
        GlobalConfiguration.Configure(WebApiConfig.Register);
        StashboxConfig.RegisterStashbox(GlobalConfiguration.Configuration, this.ConfigureServices);
    }

    private void ConfigureServices(IStashboxContainer container)
    {
        container.RegisterType<IService1, Service1>();
        //etc...
    }
}
```
## Registering in WebApiConfig.cs
```c#
public static class WebApiConfig
{
    public static void Register(HttpConfiguration config)
    {
        //etc...

        config.UseStashbox(ConfigureServices);
    }

    private static void ConfigureServices(IStashboxContainer container)
    {
        container.RegisterType<IService1, Service1>();
        //etc...
    }
}
```
## Owin
```c#
public class Startup
{
    public void Configuration(IAppBuilder app)
    {
        var httpConfiguration = new HttpConfiguration();
        //etc...

        httpConfiguration.UseStashbox(container => 
        {
            container.RegisterType<IService1, Service1>();
            //etc...
        });

        app.UseWebApi(httpConfiguration);
    }
}
```
