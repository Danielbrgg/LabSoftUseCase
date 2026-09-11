using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Clinica.Models;

namespace Clinica.Controllers
{
    [Authorize]
    public class PacienteController : Controller
    {
        private readonly DbClinicaContext _context;

        public PacienteController(DbClinicaContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return RedirectToAction("Index", "Consulta");
        }

        public IActionResult Details(int? id)
        {
            return RedirectToAction("Index", "Consulta");
        }

        [AllowAnonymous]
        public IActionResult Create()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("Codigo,Nome,Cpf,Telefone,DataNascimento")] Paciente paciente)
        {
            if (ModelState.IsValid)
            {
                _context.Add(paciente);
                await _context.SaveChangesAsync();
                return RedirectToAction("Login", "Account");
            }

            return View(paciente);
        }

        public IActionResult Edit(int? id)
        {
            return RedirectToAction("Index", "Consulta");
        }

        public IActionResult Delete(int? id)
        {
            return RedirectToAction("Index", "Consulta");
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            return RedirectToAction("Index", "Consulta");
        }
    }
}
