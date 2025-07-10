using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client.Kerberos;
using Nexus.Web.Data;

namespace Nexus.Web.Controllers
{
    public class SuppliersController : Controller
    {
        private readonly ApplicationDbContext _context; //store context object from ApplicationDbContext this will give access to the tables

        // sets the _context value
        public SuppliersController(ApplicationDbContext context)
        {
            _context = context;
        }

        //get Suppliers data
        public async Task<IActionResult> Index()
        {
            var supppliers = await _context.Suppliers.ToListAsync();

            return View(supppliers);
            
        }

        public async Task<IActionResult> Details(int? id)
        {
            //check if param id is null then return status 404
            if (id == null)
            {
                return NotFound();
            }

            var supplier = await _context.Suppliers.FirstOrDefaultAsync(m => m.Id == id);

            //check if the linq returns null then return status 404
            if (supplier == null)
            {
                return NotFound();
            }

            return View(supplier);
        }


    }
}
