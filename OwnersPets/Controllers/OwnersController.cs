using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using OwnersPets.Models;

namespace OwnersPets.Controllers
{
	public class OwnersController : ApiController
	{
		private AppDbContext db = new AppDbContext();

		// GET: api/Owners
		//[Route("{pageSize:int}/{pageNumber:int}/{orderBy:alpha?}")]
		public IHttpActionResult GetOwners(int pageSize, int pageNumber)
		//public IQueryable<Owner> GetOwners()
		{
			var totalCount =db.Owners.Count();
			var totalPages = Math.Ceiling((double)totalCount / pageSize);
			var ownersQuery = db.Owners.Include(p =>p.Pets);
			ownersQuery = ownersQuery.OrderBy(c => c.OwnerName);

			foreach (var item in ownersQuery)
			{
				if (item.Pets!=null)
				{
					item.Count = item.Pets.Count();
				}
			}

			var owners = ownersQuery.Skip((pageNumber - 1) * pageSize)
									.Take(pageSize)
									.ToList();

			var result = new
			{
				TotalCount = totalCount,
				totalPages = totalPages,
				Owners = owners
			};
			//return result;

			return Ok(result);


			//var result = db.Owners.Include(p =>p.Pets);
			//foreach (var item in result)
			//{
			//	if (item.Pets!=null)
			//	{
			//		item.Count = item.Pets.Count();
			//	}
			//}
		}

		public IQueryable<Owner> GetOwners()
		{
			var result = db.Owners.Include(p => p.Pets);
			foreach (var item in result)
			{
				if (item.Pets != null)
				{
					item.Count = item.Pets.Count();
				}
			}
			return result;
		}
		// GET: api/Owners/5
		[ResponseType(typeof(Owner))]
		public IHttpActionResult GetOwner(int id)
		{
			Owner owner = db.Owners.Include(p => p.Pets)
				.Where(o => o.OwnerId == id).FirstOrDefault();

			if (owner == null)
			{
				return NotFound();
			}

			return Ok(owner);
		}

		// PUT: api/Owners/5
		[ResponseType(typeof(void))]
		public IHttpActionResult PutOwner(int id, Owner owner)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			if (id != owner.OwnerId)
			{
				return BadRequest();
			}

			db.Entry(owner).State = EntityState.Modified;

			try
			{
				db.SaveChanges();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!OwnerExists(id))
				{
					return NotFound();
				}
				else
				{
					throw;
				}
			}

			return StatusCode(HttpStatusCode.NoContent);
		}

		// POST: api/Owners
		[ResponseType(typeof(Owner))]
		public IHttpActionResult PostOwner(Owner owner)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			// make sure, that returns same owner
			db.Owners.Add(owner);
			db.SaveChanges();

			return CreatedAtRoute("DefaultApi", new { id = owner.OwnerId }, owner);
		}

		// DELETE: api/Owners/5
		[ResponseType(typeof(Owner))]
		public IHttpActionResult DeleteOwner(int id)
		{
			Owner owner = db.Owners.Include(p => p.Pets)
				.Where(o => o.OwnerId == id).FirstOrDefault();
			if (owner == null)
			{
				return NotFound();
			}
			if (owner.Pets!=null)
			{
				db.Pets.RemoveRange(owner.Pets);
			}
			db.Owners.Remove(owner);
			db.SaveChanges();

			return Ok(owner);
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				db.Dispose();
			}
			base.Dispose(disposing);
		}

		private bool OwnerExists(int id)
		{
			return db.Owners.Count(e => e.OwnerId == id) > 0;
		}
	}
}