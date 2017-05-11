using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OwnersPets.Models;

namespace OwnersPets.Controllers
{
	public class HomeController : Controller
	{
		public ActionResult Index()
		{
			ViewBag.Title = "Home Page";

			using(var context = new AppDbContext())
			{
				var owner = new Owner("Andrij");
				owner.Pets = new List<Pet>() { new Pet("may"), new Pet("gav") };
				
				context.Owners.Add(owner);
				context.SaveChanges();
			}
			return View();
		}
	}
}
