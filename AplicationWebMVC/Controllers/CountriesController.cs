using App.Models;
using ASP.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace AplicationWebMVC.Controllers
{
    public class CountriesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CountriesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: CountriesController
        public ActionResult Index()
        {
            var countries = _context.countries.ToList();
            ViewData["Region_ID"] = new SelectList(_context.regions, "REGION_ID", "REGION_NAME");
            return View(countries);
        }


        // GET: CountriesController/Create
        public async Task<IActionResult> Create(string? id, string? modo)
        {
            string? vModo = modo;
            Countries? Reg = new Countries();

            if (string.IsNullOrEmpty(id) || id == "0")
            {
                return View("Create", Reg);

            }
            else
            {
                Reg = await _context.countries.FindAsync(id);
                Countries countrymodel = new Countries();
                countrymodel.COUNTRY_NAME = Reg.COUNTRY_NAME;
                countrymodel.COUNTRY_ID = Reg.COUNTRY_ID;
                countrymodel.REGION_ID = Reg.REGION_ID;
                return View("Edit", countrymodel);
            }



        }

        // POST: CountriesController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Countries country, string id)
        {

            if (ModelState.IsValid)
            {
                if (id == "add")
                {

                    await _context.countries.AddAsync(country);
                    await _context.SaveChangesAsync();

                    TempData["mensaje"] = "El Pais se guardo correctamente";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    _context.countries.Update(country);
                    await _context.SaveChangesAsync();

                    TempData["mensaje"] = "Cambios guardados correctamente";
                    return RedirectToAction(nameof(Index), new { id = "" });
                }
            }

            return View(country);
        }
        /*
        // GET: CountriesController/Create
        public async Task<IActionResult> Delete(string id)
        {
            var reg = _context.countries.Find(id);
            return View("Delete", reg);
        }*/

        [HttpGet]
        public ActionResult Delete(string id)
        {
            var reg = _context.countries.Find(id);
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
        public async Task<IActionResult> Delete(Countries country, string id)
        {
            var reg = _context.countries.Find(id);
            if (reg == null)
            {
                TempData["error"] = "Algo salió mal... inténtalo de nuevo.";
                return RedirectToAction(nameof(Delete));
            }
            else
            {

                _context.countries.Remove(reg);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index), new { id = "" });
            }
        }
    }
}
