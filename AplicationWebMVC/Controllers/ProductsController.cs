using App.Models;
using ASP.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;

namespace AplicationWebMVC.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProductsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: CountriesController
        public ActionResult Index()
        {
            var products = _context.products.Take(20).ToList();
            return View(products);
        }


        // GET: CountriesController/Create
        public async Task<IActionResult> Create(int? id, string? modo)
        {
            string? vModo = modo;
            Products? Reg = new Products();

            if (id == null || id == 0)
            {
                return View("Create", Reg);

            }
            else
            {
                Reg = await _context.products.FindAsync(id);
                Products productsmodel = new Products();
                productsmodel.PRODUCT_ID = Reg.PRODUCT_ID;
                productsmodel.PRODUCT_NAME = Reg.PRODUCT_NAME;
                productsmodel.DESCRIPTION = Reg.DESCRIPTION;
                productsmodel.STANDARD_COST = Reg.STANDARD_COST;
                productsmodel.LIST_PRICE = Reg.LIST_PRICE;
                productsmodel.CATEGORY_ID = Reg.CATEGORY_ID;
                return View("Edit", productsmodel);
            }



        }

        // POST: CountriesController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Products product, string id)
        {

            if (ModelState.IsValid)
            {
                if (id == "add")
                {

                    await _context.products.AddAsync(product);
                    await _context.SaveChangesAsync();

                    TempData["mensaje"] = "El Producto se guardo correctamente";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    _context.products.Update(product);
                    await _context.SaveChangesAsync();

                    TempData["mensaje"] = "Cambios guardados correctamente";
                    return RedirectToAction(nameof(Index), new { id = "" });
                }
            }

            return View(product);
        }
        /*
        // GET: CountriesController/Create
        public async Task<IActionResult> Delete(string id)
        {
            var reg = _context.countries.Find(id);
            return View("Delete", reg);
        }*/

        [HttpGet]
        public ActionResult Delete(int id)
        {
            var reg = _context.products.Find(id);
            if (reg == null)
            {
                return RedirectToAction(nameof(Index), new { id = "" });
            }
            else
            {

                return View("Delete", reg);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Products product, int id)
        {
            var reg = _context.products.Find(id);
            if (reg == null)
            {
                TempData["error"] = "Algo salió mal... inténtalo de nuevo.";
                return RedirectToAction(nameof(Delete));
            }
            else
            {

                _context.products.Remove(reg);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index), new { id = "" });
            }
        }
    }
}
