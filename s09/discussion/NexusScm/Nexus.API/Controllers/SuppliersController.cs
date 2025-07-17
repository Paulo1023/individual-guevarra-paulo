using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client.Kerberos;
using Nexus.API.Data;
using Nexus.Core;

namespace Nexus.API.Controllers
{
    [Route("api/[controller]")] //set route
    [ApiController]
    public class SuppliersController : ControllerBase
    {
        private readonly ApplicationDbContext _context; //store context object from ApplicationDbContext this will give access to the tables

        // sets the _context value
        public SuppliersController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Supplier>>> GetSuppliers()
        {
            //throw new Exception("This is a deliberate test exception!");

            //list all
            return await _context.Suppliers.ToListAsync();
        }

        [HttpGet("{id}")] // adds id in the url
        public async Task<ActionResult<Supplier>> GetSupplier(int id)
        {
            //needs paramater for find async 
            var supplier = await _context.Suppliers.FindAsync(id);

            if (supplier == null)
            {
                return NotFound();
            }

            return Ok(supplier); // ok returns Http status code 200 with the supplier 
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Supplier>> PostSupplier(Supplier supplier)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Suppliers.Add(supplier);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetSupplier), new { id = supplier.Id }, supplier);
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<ActionResult<Supplier>> PutSupplier(int id, Supplier supplier)
        {

            var existingSupplier = await _context.Suppliers.FindAsync(id);
            if (existingSupplier == null)
            {
                return NotFound("Supplier not found");
            }

            // update columns
            existingSupplier.Name = supplier.Name;
            existingSupplier.ContactPerson = supplier.ContactPerson;
            existingSupplier.Email = supplier.Email;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Suppliers.Any(s => s.Id == id))
                {
                    return NotFound("Supplier no longer exists.");
                }
                else
                {
                    throw;
                }
                
            }
            return Ok("Update Successful!");
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<ActionResult<Supplier>> DeleteSupplier(int id)
        {
            var supplier = await _context.Suppliers.FindAsync(id);

            if (supplier == null)
            {
                return NotFound("Supplier not found.");
            }

            _context.Suppliers.Remove(supplier);
            await _context.SaveChangesAsync();

            return Ok("Deletion Successful!");
        }
    }
}
