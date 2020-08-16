using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Escola.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Escola.Controllers
{
    public class ProfessoresController : Controller
    {
        private readonly EscolaContext _context;

        public ProfessoresController(EscolaContext context)
        {
            _context = context;
        }

        // GET: Professores
        public async Task<IActionResult> Index()
        {
            return View(await _context.Professores.ToListAsync());
        }
          
    
        public IActionResult ListarAlunos(int id, string nome)
        {            
            Professores professor = new Professores();
            professor.Id = id;
            professor.Nome = nome.TrimEnd();
            return RedirectToAction("Index", "Alunos", new { id = id, nome  = nome } );
        }

        // GET: Professores/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var professores = await _context.Professores
                .FirstOrDefaultAsync(m => m.Id == id);
            if (professores == null)
            {
                return NotFound();
            }

            return View(professores);
        }

        // GET: Professores/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Professores/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nome")] Professores professores)
        {
            if (ModelState.IsValid)
            {
                _context.Add(professores);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(professores);
        }

        // GET: Professores/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var professores = await _context.Professores.FindAsync(id);
            if (professores == null)
            {
                return NotFound();
            }
            return View(professores);
        }

        // POST: Professores/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nome")] Professores professores)
        {
            if (id != professores.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(professores);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProfessoresExists(professores.Id))
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
            return View(professores);
        }

        // GET: Professores/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var professores = await _context.Professores
                .FirstOrDefaultAsync(m => m.Id == id);
            if (professores == null)
            {
                return NotFound();
            }

            return View(professores);
        }

        // POST: Professores/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var professores = await _context.Professores.FindAsync(id);
            _context.Professores.Remove(professores);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProfessoresExists(int id)
        {
            return _context.Professores.Any(e => e.Id == id);
        }
    }
}
