using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        public async Task<ActionResult<Supplier>> PostSupplier(Supplier supplier)
        {
            if (supplier == null)
            {
                return BadRequest("Supplier cannot be null.");
            }

            _context.Suppliers.Add(supplier);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetSupplier), new { id = supplier.Id }, supplier);
        }
    }
}
