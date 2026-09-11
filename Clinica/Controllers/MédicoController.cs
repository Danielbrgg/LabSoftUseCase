using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Clinica.Models;

namespace Clinica.Controllers
{
    public class MédicoController : Controller
    {
        private readonly DbClinicaContext _context;

        public MédicoController(DbClinicaContext context)
        {
            _context = context;
        }

        // GET: Médico
        public async Task<IActionResult> Index()
        {
            return View(await _context.Médicos.ToListAsync());
        }

        // GET: Médico/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var médico = await _context.Médicos
                .FirstOrDefaultAsync(m => m.Codigo == id);
            if (médico == null)
            {
                return NotFound();
            }

            return View(médico);
        }

        // GET: Médico/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Médico/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Codigo,Nome,Crm,Especialidade")] Médico médico)
        {
            if (ModelState.IsValid)
            {
                _context.Add(médico);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(médico);
        }

        // GET: Médico/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var médico = await _context.Médicos.FindAsync(id);
            if (médico == null)
            {
                return NotFound();
            }
            return View(médico);
        }

        // POST: Médico/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Codigo,Nome,Crm,Especialidade")] Médico médico)
        {
            if (id != médico.Codigo)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(médico);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MédicoExists(médico.Codigo))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(médico);
        }

        // GET: Médico/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var médico = await _context.Médicos
                .FirstOrDefaultAsync(m => m.Codigo == id);
            if (médico == null)
            {
                return NotFound();
            }

            return View(médico);
        }

        // POST: Médico/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var médico = await _context.Médicos.FindAsync(id);
            if (médico != null)
            {
                _context.Médicos.Remove(médico);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MédicoExists(int id)
        {
            return _context.Médicos.Any(e => e.Codigo == id);
        }
    }
}
