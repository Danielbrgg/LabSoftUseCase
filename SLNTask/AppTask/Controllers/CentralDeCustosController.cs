using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AppTask.Models;

namespace AppTask.Controllers
{
    public class CentralDeCustosController : Controller
    {
        private readonly DbTasksZeroContext _context;

        public CentralDeCustosController(DbTasksZeroContext context)
        {
            _context = context;
        }

        // GET: CentralDeCustos
        public async Task<IActionResult> Index()
        {
            return View(await _context.CentralDeCustos.ToListAsync());
        }

        // GET: CentralDeCustos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var centralDeCusto = await _context.CentralDeCustos
                .FirstOrDefaultAsync(m => m.Codigo == id);
            if (centralDeCusto == null)
            {
                return NotFound();
            }

            return View(centralDeCusto);
        }

        // GET: CentralDeCustos/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: CentralDeCustos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Codigo,NomeCentral,ValorMetaAnual")] CentralDeCusto centralDeCusto)
        {
            if (ModelState.IsValid)
            {
                _context.Add(centralDeCusto);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(centralDeCusto);
        }

        // GET: CentralDeCustos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var centralDeCusto = await _context.CentralDeCustos.FindAsync(id);
            if (centralDeCusto == null)
            {
                return NotFound();
            }
            return View(centralDeCusto);
        }

        // POST: CentralDeCustos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Codigo,NomeCentral,ValorMetaAnual")] CentralDeCusto centralDeCusto)
        {
            if (id != centralDeCusto.Codigo)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(centralDeCusto);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CentralDeCustoExists(centralDeCusto.Codigo))
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
            return View(centralDeCusto);
        }

        // GET: CentralDeCustos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var centralDeCusto = await _context.CentralDeCustos
                .FirstOrDefaultAsync(m => m.Codigo == id);
            if (centralDeCusto == null)
            {
                return NotFound();
            }

            return View(centralDeCusto);
        }

        // POST: CentralDeCustos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var centralDeCusto = await _context.CentralDeCustos.FindAsync(id);
            if (centralDeCusto != null)
            {
                _context.CentralDeCustos.Remove(centralDeCusto);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CentralDeCustoExists(int id)
        {
            return _context.CentralDeCustos.Any(e => e.Codigo == id);
        }
    }
}
