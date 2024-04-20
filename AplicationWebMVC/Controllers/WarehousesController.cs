using App.Models;
using ASP.Data;
using Microsoft.AspNetCore.Mvc;

namespace AplicationWebMVC.Controllers
{
    public class WarehousesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public WarehousesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: CountriesController
        public ActionResult Index()
        {
            var warehouse = _context.warehouses.ToList();
            return View(warehouse);
        }


        // GET: CountriesController/Create
        public async Task<IActionResult> Create(int? id, string? modo)
        {
            string? vModo = modo;
            Warehouses? Reg = new Warehouses();

            if (id == null || id == 0)
            {
                return View("Create", Reg);

            }
            else
            {
                Reg = await _context.warehouses.FindAsync(id);
                Warehouses warehousemodel = new Warehouses();
                warehousemodel.WAREHOUSE_ID = Reg.WAREHOUSE_ID;
                warehousemodel.WAREHOUSE_NAME = Reg.WAREHOUSE_NAME;
                warehousemodel.LOCATION_ID = Reg.LOCATION_ID;
                return View("Edit", warehousemodel);
            }



        }

        // POST: CountriesController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Warehouses warehouse, string id)
        {

            if (ModelState.IsValid)
            {
                if (id == "add")
                {

                    await _context.warehouses.AddAsync(warehouse);
                    await _context.SaveChangesAsync();

                    TempData["mensaje"] = "El Producto se guardo correctamente";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    _context.warehouses.Update(warehouse);
                    await _context.SaveChangesAsync();

                    TempData["mensaje"] = "Cambios guardados correctamente";
                    return RedirectToAction(nameof(Index), new { id = "" });
                }
            }

            return View(warehouse);
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
            var reg = _context.warehouses.Find(id);
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
        public async Task<IActionResult> Delete(Warehouses warehouse, int id)
        {
            var reg = _context.warehouses.Find(id);
            if (reg == null)
            {
                TempData["error"] = "Algo salió mal... inténtalo de nuevo.";
                return RedirectToAction(nameof(Delete));
            }
            else
            {

                _context.warehouses.Remove(reg);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index), new { id = "" });
            }
        }
    }
}
