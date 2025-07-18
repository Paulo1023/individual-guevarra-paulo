using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client.Kerberos;
using Nexus.Core;
using Nexus.Web.Data;
using Nexus.Web.Filters;
using System.Text.Json;

namespace Nexus.Web.Controllers
{
    [SessionAuthorize]
    public class SuppliersController : Controller
    {
        //private readonly ApplicationDbContext _context; //store context object from ApplicationDbContext this will give access to the tables
        private readonly IHttpClientFactory _httpClientFactory;


        // sets the _context value
        //public SuppliersController(ApplicationDbContext context)
        //{
        //    _context = context;
        //}
        public SuppliersController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public void AddAuthorizationHeader(HttpClient client)
        {
            var token = HttpContext.Session.GetString("JWToken");
            if (!string.IsNullOrEmpty(token))
            {
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            }
        }

        //get Suppliers data
        public async Task<IActionResult> Index()
        {
            //var suppliers = await _context.Suppliers.ToListAsync();


            var client = _httpClientFactory.CreateClient("NexusApiClient");
            var response = await client.GetAsync("api/suppliers");

            List<Supplier> suppliers = new();

            if (response.IsSuccessStatusCode)
            {
                var responseStream = await response.Content.ReadAsStreamAsync();
                suppliers = await JsonSerializer.DeserializeAsync<List<Supplier>>(responseStream, new
                    JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }

            return View(suppliers);
        }

        //public async Task<IActionResult> Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var supplier = await _context.Suppliers.FirstOrDefaultAsync(m => m.Id == id);

        //    if (supplier == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(supplier);
        //}

        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,ContactPerson,Email")] Supplier supplier)
        {
            //_context.Suppliers.Add(supplier);
            //await _context.SaveChangesAsync();

            if (ModelState.IsValid)
            {
                var client = _httpClientFactory.CreateClient("NexusApiClient");

                var token = HttpContext.Session.GetString("JWToken");

                //if (!string.IsNullOrEmpty(token))
                //{
                //    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                //}
                AddAuthorizationHeader(client);

                var response = await client.PostAsJsonAsync("api/suppliers", supplier);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));

                }else
                {
                    ModelState.AddModelError(string.Empty, "Failed to create supplier. API returned an error.");
                }
            }

            return View(supplier);
        }


        public async Task<IActionResult> Edit(int id)
        {
            var client = _httpClientFactory.CreateClient("NexusApiClient");
            AddAuthorizationHeader(client);

            var response = await client.GetAsync($"api/suppliers/{id}");

            if (!response.IsSuccessStatusCode)
            {
                return NotFound();
            }

            var supplier = await response.Content.ReadFromJsonAsync<Supplier>();

            if (supplier == null)
            {
                return NotFound();
            }

            return View(supplier);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [BindAttribute("Id,Name,ContactPerson,Email")] Supplier supplier)
        {
            if (id != supplier.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var client = _httpClientFactory.CreateClient("NexusApiClient");
                AddAuthorizationHeader(client);

                var response = await client.PutAsJsonAsync($"api/suppliers/{id}", supplier);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Failed to update supplier. API returned an error.");
                }

            }

            return View(supplier);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var client = _httpClientFactory.CreateClient("NexusApiClient");
            AddAuthorizationHeader(client);

            var response = await client.GetAsync($"api/suppliers/{id}");

            if (!response.IsSuccessStatusCode)
            {
                return NotFound();
            }

            var supplier = await response.Content.ReadFromJsonAsync<Supplier>();

            if (supplier == null)
            {
                return NotFound();
            }

            return View(supplier);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id, [BindAttribute("Id,Name,ContactPerson,Email")] Supplier supplier)
        {
            if (id != supplier.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var client = _httpClientFactory.CreateClient("NexusApiClient");
                AddAuthorizationHeader(client);

                var response = await client.DeleteAsync($"api/suppliers/{id}");

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Failed to delte supplier. API returned an error.");
                }

            }

            return View(supplier);
        }
    }
}
