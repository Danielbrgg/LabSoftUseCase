using Clinica.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Clinica.Controllers
{
    [Authorize]
    public class ConsultaController : Controller
    {
        private readonly DbClinicaContext _context;

        public ConsultaController(DbClinicaContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var pacienteId = HttpContext.Session.GetInt32("PacienteId");

            if (pacienteId == null)
            {
                await HttpContext.SignOutAsync();
                return RedirectToAction("Login", "Account");
            }

            var consultas = await _context.Consulta
                .Include(c => c.Medico)
                .Include(c => c.Paciente)
                .Where(c => c.PacienteId == pacienteId)
                .OrderByDescending(c => c.DataHora)
                .ToListAsync();

            return View(consultas);
        }

        public async Task<IActionResult> Details(int? id)
        {
            var pacienteId = HttpContext.Session.GetInt32("PacienteId");

            if (id == null || pacienteId == null)
                return RedirectToAction("Index");

            var consulta = await _context.Consulta
                .Include(c => c.Medico)
                .Include(c => c.Paciente)
                .FirstOrDefaultAsync(c =>
                    c.Codigo == id &&
                    c.PacienteId == pacienteId);

            if (consulta == null)
                return NotFound();

            return View(consulta);
        }

        public IActionResult Create()
        {
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("DataHora,StatusConsulta,MedicoId")] Consulta consulta)
        {
            return RedirectToAction("Index");
        }

        public IActionResult Edit(int? id)
        {
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int? id)
        {
            return RedirectToAction("Index");
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            return RedirectToAction("Index");
        }
    }
}
