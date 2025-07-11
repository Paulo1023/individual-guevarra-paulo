using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client.Kerberos;
using Nexus.Core;
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
            if (id == null)
            {
                return NotFound();
            }

            var supplier = await _context.Suppliers.FirstOrDefaultAsync(m => m.Id == id);

            if (supplier == null)
            {
                return NotFound();
            }

            return View(supplier);
        }

        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatePost(Supplier supplier)
        {
            _context.Suppliers.Add(supplier);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

    }
}
