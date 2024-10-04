using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using SignalRDenemesi.Hubs;
using SignalRDenemesi.Models;
using System.Diagnostics;

namespace SignalRDenemesi.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext _context;
        public HomeController(ILogger<HomeController> logger, AppDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult PersonAdd()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> PersonAdd(Person model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _context.People.AddAsync(model);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
                catch (Exception)
                {
                    return RedirectToAction("PersonAddError");
                }
               
            }
            return View();
        }
       
    }
}
