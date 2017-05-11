using OwnersPets.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Script.Serialization;
using Newtonsoft.Json;

namespace OwnersPets.Controllers
{
	public class Owners2Controller : ApiController
	{
		AppDbContext db = new AppDbContext();

		// GET api/values
		public IEnumerable<Owner> Get()
		{
			var names = db.Owners
						   //.Include(b => b.Pets)
						   .ToList();
			return names;
			//foreach (var item in names)
			//{
			//	item.Count = item.Pets.Count();
			//}
			//var list = JsonConvert.SerializeObject(names, Formatting.None,
			//	new JsonSerializerSettings()
			//	{
			//		ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
			//	});

			////return Content(list, "application/json");
			//return list;
		}

		// GET api/values/5
		public IHttpActionResult Get(int id)
		{
			Owner owner = db.Owners
						  //.Include(p => p.Pets)
						  .Where(item => item.OwnerId == id)
						  .FirstOrDefault();

			if (owner == null)
			{
				return NotFound();
			}

			return Ok(owner);
		}

		// POST api/values
		public void Post([FromBody]string value)
		{
			if (string.IsNullOrEmpty(value))
			{
				Owner owner = new Owner(value);
				db.Owners.Add(owner);
				db.SaveChanges();
			}
		}

		// PUT api/values/5
		public void Put(int id, [FromBody]string value)
		{
		}

		// DELETE api/values/5
		public void Delete(int id)
		{
			//var item = db
		}
	}
}
