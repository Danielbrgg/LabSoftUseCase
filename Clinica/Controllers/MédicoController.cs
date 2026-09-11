using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Clinica.Models;

namespace Clinica.Controllers
{
    [Authorize]
    public class MédicoController : Controller
    {
        public IActionResult Index()
        {
            return RedirectToAction("Index", "Consulta");
        }

        public IActionResult Details(int? id)
        {
            return RedirectToAction("Index", "Consulta");
        }

        public IActionResult Create()
        {
            return RedirectToAction("Index", "Consulta");
        }

        public IActionResult Edit(int? id)
        {
            return RedirectToAction("Index", "Consulta");
        }

        public IActionResult Delete(int? id)
        {
            return RedirectToAction("Index", "Consulta");
        }
    }
}
