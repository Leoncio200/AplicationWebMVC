using ASP.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using App.Models;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace ASP.Controllers
{
    public class CustomersController : Controller
    {

        private readonly ApplicationDbContext _context;

        public CustomersController(ApplicationDbContext context)
        {
            _context = context;
        }
        // GET: CustomersController
        public ActionResult Index()
        {
            IEnumerable<Customers> listaCostumer = _context.customers.Take(15).ToList();
            return View("Index",listaCostumer);
        }

        // GET: CustomersController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: CustomersController/Create
        public ActionResult Create()
        {
            return View("Create");
        }

        // POST: CustomersController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Customers customers)
        {
            if (ModelState.IsValid)
            {
                _context.customers.Add(customers);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View();
        }

        // GET: CustomersController/Edit/5
        public ActionResult Edit(int id)
        {
            if(id == null || id == 0)
            {
                return NotFound();
            }
            var cust = _context.customers.Find(id);
            return View("Edit", cust);
        }

        // POST: CustomersController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Customers customer)
        {
            if (ModelState.IsValid)
            {
                _context.customers.Update(customer);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View();
        }

        // GET: CustomersController/Delete/5
        public ActionResult Delete(int id)
        {
            var customer = _context.customers.Find(id);
            return View("Delete", customer);
        }

        // POST: CustomersController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            var reg = _context.customers.Find(id);
            if (reg == null)
            {
                TempData["error"] = "Algo salió mal... inténtalo de nuevo.";
                return RedirectToAction(nameof(Delete));
            }
            else
            {

                _context.customers.Remove(reg);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index), new { id = "" });
            }
        }
    }
}
