using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Fall2025_Project3_krhanson3.Data;
using Fall2025_Project3_krhanson3.Models;

namespace Fall2025_Project3_krhanson3.Controllers
{
    public class MovieActorsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MovieActorsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: MovieActors
        public async Task<IActionResult> Index()
        {
            var movieActors = _context.MovieActor
                .Include(m => m.Actor)
                .Include(m => m.Movie);
            return View(await movieActors.ToListAsync());
        }

        // GET: MovieActors/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var movieActor = await _context.MovieActor
                .Include(m => m.Actor)
                .Include(m => m.Movie)
                .FirstOrDefaultAsync(m => m.MovieActorId == id);

            if (movieActor == null)
                return NotFound();

            return View(movieActor);
        }

        // GET: MovieActors/Create
        public IActionResult Create()
        {
            // Show movie titles and actor names instead of IDs
            ViewData["MovieId"] = new SelectList(_context.Movies, "MovieId", "Title");
            ViewData["ActorId"] = new SelectList(_context.Actors, "ActorId", "Name");
            return View();
        }

        // POST: MovieActors/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MovieActorId,MovieId,ActorId")] MovieActor movieActor)
        {
            // Check for duplicates before saving
            bool exists = await _context.MovieActor
                .AnyAsync(ma => ma.MovieId == movieActor.MovieId && ma.ActorId == movieActor.ActorId);

            if (exists)
            {
                ModelState.AddModelError("", "This movie and actor pair already exists.");
            }

            if (ModelState.IsValid)
            {
                _context.Add(movieActor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // Rebuild dropdowns with Title and Name
            ViewData["MovieId"] = new SelectList(_context.Movies, "MovieId", "Title", movieActor.MovieId);
            ViewData["ActorId"] = new SelectList(_context.Actors, "ActorId", "Name", movieActor.ActorId);
            return View(movieActor);
        }

        // GET: MovieActors/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var movieActor = await _context.MovieActor.FindAsync(id);
            if (movieActor == null)
                return NotFound();

            // Use title/name here too
            ViewData["MovieId"] = new SelectList(_context.Movies, "MovieId", "Title", movieActor.MovieId);
            ViewData["ActorId"] = new SelectList(_context.Actors, "ActorId", "Name", movieActor.ActorId);
            return View(movieActor);
        }

        // POST: MovieActors/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MovieActorId,MovieId,ActorId")] MovieActor movieActor)
        {
            if (id != movieActor.MovieActorId)
                return NotFound();

            // Check for duplicates again (excluding current record)
            bool exists = await _context.MovieActor
                .AnyAsync(ma => ma.MovieId == movieActor.MovieId && ma.ActorId == movieActor.ActorId && ma.MovieActorId != id);

            if (exists)
            {
                ModelState.AddModelError("", "This movie and actor pair already exists.");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(movieActor);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.MovieActor.Any(e => e.MovieActorId == id))
                        return NotFound();
                    else
                        throw;
                }
            }

            // Rebuild dropdowns if validation fails
            ViewData["MovieId"] = new SelectList(_context.Movies, "MovieId", "Title", movieActor.MovieId);
            ViewData["ActorId"] = new SelectList(_context.Actors, "ActorId", "Name", movieActor.ActorId);
            return View(movieActor);
        }

        // GET: MovieActors/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var movieActor = await _context.MovieActor
                .Include(m => m.Actor)
                .Include(m => m.Movie)
                .FirstOrDefaultAsync(m => m.MovieActorId == id);

            if (movieActor == null)
                return NotFound();

            return View(movieActor);
        }

        // POST: MovieActors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var movieActor = await _context.MovieActor.FindAsync(id);
            if (movieActor != null)
                _context.MovieActor.Remove(movieActor);

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MovieActorExists(int id)
        {
            return _context.MovieActor.Any(e => e.MovieActorId == id);
        }
    }
}
