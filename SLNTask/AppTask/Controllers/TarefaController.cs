using AppTask.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace AppTask.Controllers
{
    public class TarefaController : Controller
    {
        private readonly DbTasksZeroContext _context;

        public TarefaController(DbTasksZeroContext context)
        {
            _context = context;
        }

        // GET: Tarefa
        public async Task<IActionResult> Index()
        {
            var tarefas = _context.Tarefas
                .Include(t => t.Funcionario);

            return View(await tarefas.ToListAsync());
        }

        // GET: Tarefa/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tarefa = await _context.Tarefas
                .Include(t => t.Funcionario)
                .FirstOrDefaultAsync(t => t.Codigo == id);

            if (tarefa == null)
            {
                return NotFound();
            }

            return View(tarefa);
        }

        // GET: Tarefa/Create
        public IActionResult Create()
        {
            ViewData["FuncionarioId"] = new SelectList(
                _context.Funcionarios,
                "Codigo",
                "Nome"
            );

            return View();
        }

        // POST: Tarefa/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Tarefa tarefa)
        {
            // Valores padrão caso sejam deixados vazios
            if (string.IsNullOrWhiteSpace(tarefa.StatusTarefa))
            {
                tarefa.StatusTarefa = "Pendente";
            }

            if (string.IsNullOrWhiteSpace(tarefa.Prazo))
            {
                tarefa.Prazo = "Não definido";
            }

            if (tarefa.DataPlanejada == default)
            {
                ModelState.AddModelError(
                    "DataPlanejada",
                    "Informe a data planejada."
                );
            }

            if (tarefa.FuncionarioId <= 0)
            {
                ModelState.AddModelError(
                    "FuncionarioId",
                    "Selecione um funcionário."
                );
            }

            if (ModelState.IsValid)
            {
                _context.Tarefas.Add(tarefa);

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            ViewData["FuncionarioId"] = new SelectList(
                _context.Funcionarios,
                "Codigo",
                "Nome",
                tarefa.FuncionarioId
            );

            return View(tarefa);
        }

        // GET: Tarefa/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tarefa = await _context.Tarefas.FindAsync(id);

            if (tarefa == null)
            {
                return NotFound();
            }

            ViewData["FuncionarioId"] = new SelectList(
                _context.Funcionarios,
                "Codigo",
                "Nome",
                tarefa.FuncionarioId
            );

            return View(tarefa);
        }

        // POST: Tarefa/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id,
            Tarefa tarefa)
        {
            if (id != tarefa.Codigo)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tarefa);

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TarefaExists(tarefa.Codigo))
                    {
                        return NotFound();
                    }

                    throw;
                }

                return RedirectToAction(nameof(Index));
            }

            ViewData["FuncionarioId"] = new SelectList(
                _context.Funcionarios,
                "Codigo",
                "Nome",
                tarefa.FuncionarioId
            );

            return View(tarefa);
        }

        // GET: Tarefa/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tarefa = await _context.Tarefas
                .Include(t => t.Funcionario)
                .FirstOrDefaultAsync(t => t.Codigo == id);

            if (tarefa == null)
            {
                return NotFound();
            }

            return View(tarefa);
        }

        // POST: Tarefa/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tarefa = await _context.Tarefas.FindAsync(id);

            if (tarefa != null)
            {
                _context.Tarefas.Remove(tarefa);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool TarefaExists(int id)
        {
            return _context.Tarefas.Any(e => e.Codigo == id);
        }
    }
}