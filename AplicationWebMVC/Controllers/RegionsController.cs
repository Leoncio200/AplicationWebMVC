using App.Models;
using ASP.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace AplicationWebMVC.Controllers
{
    public class RegionsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RegionsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public ActionResult Index()
        {
            var regions = _context.regions.ToList();
            ViewData["Region_ID"] = new SelectList(_context.regions, "REGION_ID", "REGION_NAME");
            return View(regions);
        }

        public async Task<IActionResult> Create(string? id, string? modo)
        {
            string? vModo = modo;
            Regions? Reg = new Regions();

            if (string.IsNullOrEmpty(id) || id == "0")
            {
                return View("Create", Reg);

            }
            else
            {
                Reg = await _context.regions.FindAsync(id);
                Regions regionmodel = new Regions();
                regionmodel.REGION_NAME = Reg.REGION_NAME;
                regionmodel.REGION_ID = Reg.REGION_ID;
                return View("Edit", regionmodel);
            }

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Regions region, string id)
        {

            if (ModelState.IsValid)
            {
                if (id == "add")
                {

                    await _context.regions.AddAsync(region);
                    await _context.SaveChangesAsync();

                    TempData["mensaje"] = "La region se guardo correctamente";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    _context.regions.Update(region);
                    await _context.SaveChangesAsync();

                    TempData["mensaje"] = "Cambios guardados correctamente";
                    return RedirectToAction(nameof(Index), new { id = "" });
                }
            }

            return View(region);
        }

        [HttpGet]
        public ActionResult Delete(string id)
        {
            var reg = _context.regions.Find(id);
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
        public async Task<IActionResult> Delete(Regions country, string id)
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
