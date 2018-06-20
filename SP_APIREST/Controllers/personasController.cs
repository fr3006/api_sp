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
using SP_APIREST.Models;

namespace SP_APIREST.Controllers
{
    public class personasController : ApiController
    {
        private personasEntities db = new personasEntities();

        // GET: api/personas
        public IEnumerable<getpersona_Result> Getpersona()
        {
            return db.getpersona().AsEnumerable();
        }

        // GET: api/personas/5
        [ResponseType(typeof(persona))]
        public IHttpActionResult Getpersona(int id)
        {
            var persona = db.getPersonasId(id);
            if (persona == null)
            {
                return NotFound();
            }

            return Ok(persona);
        }

        // PUT: api/personas/5
        [ResponseType(typeof(void))]
        public int Putpersona(int id, persona persona)
        {
          

          
            if (id != persona.cedula)
            {
                return db.putPersona(id, persona.nombres , persona.pais);
            }

            db.Entry(persona).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!personaExists(id.ToString()))
                {
                    return 0;
                }
                else
                {
                    throw;
                }
            }

            return 1;
        }

        // POST: api/personas
        [ResponseType(typeof(persona))]
        public IHttpActionResult Postpersona(persona persona)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.postPersona(persona.cedula,persona.pais,persona.pais);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (personaExists(persona.nombres))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = persona.nombres }, persona);
        }

        // DELETE: api/personas/5
        [ResponseType(typeof(persona))]
        public IHttpActionResult Deletepersona(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }

            db.deletePersona(id);
            db.SaveChanges();

            return Ok(1);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool personaExists(string id)
        {
            return db.persona.Count(e => e.nombres == id) > 0;
        }
    }
}