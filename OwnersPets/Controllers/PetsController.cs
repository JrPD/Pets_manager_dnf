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
using Newtonsoft.Json;

namespace OwnersPets.Controllers
{
	public class PetsController : ApiController
	{
		//public string ConvertToJson(dynamic names)
		//{
		//	return	JsonConvert.SerializeObject(names,	Formatting.None,
		//		new JsonSerializerSettings()
		//		{
		//			ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
		//		});
		//}
		private AppDbContext db = new AppDbContext();

		// GET: api/Pets
		public IQueryable<Pet> GetPets()
		{
			return db.Pets;
		}

		// GET: api/Pets/5
		[ResponseType(typeof(Pet))]
		public IHttpActionResult GetPet(int id)
		{
			Pet pet = db.Pets.Find(id);
			if (pet == null)
			{
				return NotFound();
			}

			return Ok(pet);
		}

		// PUT: api/Pets/5
		[ResponseType(typeof(void))]
		public IHttpActionResult PutPet(int id, Pet pet)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			if (id != pet.PetId)
			{
				return BadRequest();
			}

			db.Entry(pet).State = EntityState.Modified;

			try
			{
				db.SaveChanges();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!PetExists(id))
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

		// POST: api/Pets
		[ResponseType(typeof(Pet))]
		public IHttpActionResult PostPet(Pet pet)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}
			Owner owner = db.Owners.Include(p => p.Pets)
				.Where(o => o.OwnerId == pet.OwnerId.OwnerId).FirstOrDefault();
			pet.OwnerId = owner;
			//owner.Pets.Add(pet);
			db.Pets.Add(pet);
			//	db.Entry(owner).State = System.Data.Entity.EntityState.Modified;     
			//owner.Pets.Add(pet);
			//db.Pets.Add(pet);
			db.SaveChanges();

			return CreatedAtRoute("DefaultApi", new { id = pet.PetId }, pet);
		}

		// DELETE: api/Pets/5
		[ResponseType(typeof(Pet))]
		public IHttpActionResult DeletePet(int id)
		{
			Pet pet = db.Pets.Find(id);
			if (pet == null)
			{
				return NotFound();
			}

			db.Pets.Remove(pet);
			db.SaveChanges();

			return Ok(pet);
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				db.Dispose();
			}
			base.Dispose(disposing);
		}

		private bool PetExists(int id)
		{
			return db.Pets.Count(e => e.PetId == id) > 0;
		}
	}
}