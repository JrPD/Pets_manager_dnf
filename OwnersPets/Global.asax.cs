using OwnersPets.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace OwnersPets
{
	public class WebApiApplication : System.Web.HttpApplication
	{
		protected void Application_Start()
		{
			//HttpConfiguration config = GlobalConfiguration.Configuration;

			//config.Formatters.JsonFormatter
			//			.SerializerSettings
			//			.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
			AreaRegistration.RegisterAllAreas();
			GlobalConfiguration.Configure(WebApiConfig.Register);
			FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
			RouteConfig.RegisterRoutes(RouteTable.Routes);
			BundleConfig.RegisterBundles(BundleTable.Bundles);
		}

		public static OwnerDataRepository RepoOwners
		{
			get
			{
				if (!HttpContext.Current.Items.Contains("_EntityContext"))
				{
					HttpContext.Current.Items.Add("_EntityContext", new OwnerDataRepository());
				}
				return HttpContext.Current.Items["_EntityContext"] as OwnerDataRepository;
			}
		}

		public static PetDataRepository RepoPets
		{
			get
			{
				if (!HttpContext.Current.Items.Contains("_EntityContext2"))
				{
					HttpContext.Current.Items.Add("_EntityContext2", new PetDataRepository());
				}
				return HttpContext.Current.Items["_EntityContext2"] as PetDataRepository;
			}
		}

		protected virtual void Application_BeginRequest()
		{
			HttpContext.Current.Items["_EntityContext"] = new OwnerDataRepository();
			if (HttpContext.Current.Request.Url.AbsolutePath.Contains("pets"))
			{
				HttpContext.Current.Items["_EntityContext2"] = new PetDataRepository();
			}
		}

		protected virtual void Application_EndRequest()
		{
			var entityContext = HttpContext.Current.Items["_EntityContext"] as OwnerDataRepository;
			var entityContext2 = HttpContext.Current.Items["_EntityContext2"] as PetDataRepository;

			if (entityContext != null)
				entityContext.Dispose();
			if (entityContext2 != null)
				entityContext2.Dispose();
		}
	}
}
