using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Fall2025_Project3_krhanson3.Data;
using Fall2025_Project3_krhanson3.Models;

namespace Fall2025_Project3_krhanson3.Controllers
{
    public class MoviesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MoviesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Movies
        public async Task<IActionResult> Index()
        {
            var movies = await _context.Movies.ToListAsync();
            return View(movies);
        }

        // GET: Movies/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var movie = await _context.Movies.FirstOrDefaultAsync(m => m.MovieId == id);
            if (movie == null) return NotFound();

            return View(movie);
        }

        // GET: Movies/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Movies/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,Genre,ReleaseYear,IMDBUrl")] Movies movie, IFormFile? PosterFile)
        {
            if (ModelState.IsValid)
            {
                if (PosterFile != null && PosterFile.Length > 0)
                {
                    using var ms = new MemoryStream();
                    await PosterFile.CopyToAsync(ms);
                    movie.Poster = ms.ToArray();
                }

                _context.Add(movie);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(movie);
        }

        // GET: Movies/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var movie = await _context.Movies.FindAsync(id);
            if (movie == null) return NotFound();

            return View(movie);
        }

        // POST: Movies/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MovieId,Title,Genre,ReleaseYear,IMDBUrl")] Movies updatedMovie, IFormFile? PosterFile)
        {
            if (id != updatedMovie.MovieId) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    var existingMovie = await _context.Movies.FindAsync(id);
                    if (existingMovie == null) return NotFound();

                    // Update fields
                    existingMovie.Title = updatedMovie.Title;
                    existingMovie.Genre = updatedMovie.Genre;
                    existingMovie.ReleaseYear = updatedMovie.ReleaseYear;
                    existingMovie.IMDBUrl = updatedMovie.IMDBUrl;

                    // Only update poster if new one is uploaded
                    if (PosterFile != null && PosterFile.Length > 0)
                    {
                        using var ms = new MemoryStream();
                        await PosterFile.CopyToAsync(ms);
                        existingMovie.Poster = ms.ToArray();
                    }

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Movies.Any(e => e.MovieId == updatedMovie.MovieId))
                        return NotFound();
                    else
                        throw;
                }

                return RedirectToAction(nameof(Index));
            }

            // Reload existing image on validation fail
            var movieWithPoster = await _context.Movies.AsNoTracking().FirstOrDefaultAsync(m => m.MovieId == id);
            if (movieWithPoster != null)
                updatedMovie.Poster = movieWithPoster.Poster;

            return View(updatedMovie);
        }

        // GET: Movies/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var movie = await _context.Movies.FirstOrDefaultAsync(m => m.MovieId == id);
            if (movie == null) return NotFound();

            return View(movie);
        }

        // POST: Movies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var movie = await _context.Movies.FindAsync(id);
            if (movie != null)
            {
                _context.Movies.Remove(movie);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool MoviesExists(int id)
        {
            return _context.Movies.Any(e => e.MovieId == id);
        }
    }
}
