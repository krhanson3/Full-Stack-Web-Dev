using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Fall2025_Project3_krhanson3.Models;
using Fall2025_Project3_krhanson3.Data;
using Fall2025_Project3_krhanson3.Models.ViewModels;
using Fall2025_Project3_krhanson3.Helpers;



namespace Fall2025_Project3_krhanson3.Controllers
{
    public class ActorsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly OpenAiApi _openAiApi;

        public ActorsController(ApplicationDbContext context, OpenAiApi openAiApi)
        {
            _context = context;
            _openAiApi = openAiApi;
        }

        // GET: Actors
        public async Task<IActionResult> Index()
        {
            var actors = await _context.Actors.ToListAsync();
            return View(actors);
        }

        // GET: Actors/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var actor = await _context.Actors
                        .Include(a => a.MovieActors)
                            .ThenInclude(ma => ma.Movie)
                        .Include(a => a.Tweets)  
                        .FirstOrDefaultAsync(a => a.ActorId == id);

            if (actor == null) return NotFound();

            // --- Generate AI tweets ---
            var (sentimentAvg, tweets) = await _openAiApi.GenerateTweetsForActor(actor.Name);

            // --- Save to database ---
            // 1. Remove old tweets
            if (actor.Tweets != null)
                _context.Tweets.RemoveRange(actor.Tweets);

            // 2. Add new ones
            foreach (var t in tweets)
            {
                actor.Tweets.Add(new Tweets
                {
                    ActorId = actor.ActorId,
                    User = t.User,
                    Text = t.Text,
                    Sentiment = t.Sentiment
                });
            }

            // 3. Save sentiment average
            actor.SentimentAverage = sentimentAvg;

            await _context.SaveChangesAsync();

            // --- Build ViewModel ---
            var viewModel = new ActorViewModel
            {
                ActorId = actor.ActorId,
                Name = actor.Name,
                Gender = actor.Gender,
                Age = actor.Age,
                IMDBUrl = actor.IMDBUrl,
                Photo = actor.Photo,

                Movies = actor.MovieActors?.Select(ma => new MovieInfo
                {
                    MovieId = ma.Movie.MovieId,
                    Title = ma.Movie.Title
                }).ToList() ?? new List<MovieInfo>(),

                SentimentAverage = actor.SentimentAverage,

                Tweets = actor.Tweets.Select(t => new TweetsInfo
                {
                    Id = t.TweetId,
                    User = t.User,
                    Text = t.Text,
                    Sentiment = t.Sentiment
                }).ToList()
            };

            return View(viewModel);
        }


        // GET: Actors/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Actors/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Gender,Age,IMDBUrl")] Actors actor, IFormFile? PhotoFile)
        {
            if (ModelState.IsValid)
            {
                if (PhotoFile != null && PhotoFile.Length > 0)
                {
                    using var ms = new MemoryStream();
                    await PhotoFile.CopyToAsync(ms);
                    actor.Photo = ms.ToArray();
                    actor.PhotoSRI = SRIHelper.ComputeSRI(actor.Photo);
                }

                if (!string.IsNullOrEmpty(actor.IMDBUrl))
                {
                    actor.IMDBUrlSRI = await SRIHelper.ComputeSRIFromUrlAsync(actor.IMDBUrl);
                }

                _context.Add(actor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(actor);
        }

        // GET: Actors/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var actor = await _context.Actors.FindAsync(id);
            if (actor == null) return NotFound();

            return View(actor);
        }

        // POST: Actors/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ActorId,Name,Gender,Age,IMDBUrl")] Actors updatedActor, IFormFile? PhotoFile)
        {
            if (id != updatedActor.ActorId) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    var existingActor = await _context.Actors.FindAsync(id);
                    if (existingActor == null) return NotFound();

                    // Update editable fields
                    existingActor.Name = updatedActor.Name;
                    existingActor.Gender = updatedActor.Gender;
                    existingActor.Age = updatedActor.Age;
                    existingActor.IMDBUrl = updatedActor.IMDBUrl;

                    // Update photo only if a new one was uploaded
                    if (PhotoFile != null && PhotoFile.Length > 0)
                    {
                        using var ms = new MemoryStream();
                        await PhotoFile.CopyToAsync(ms);
                        existingActor.Photo = ms.ToArray();
                    }

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Actors.Any(e => e.ActorId == updatedActor.ActorId))
                        return NotFound();
                    else
                        throw;
                }

                return RedirectToAction(nameof(Index));
            }

            // Re-display existing photo if validation fails
            var actorWithPhoto = await _context.Actors.AsNoTracking().FirstOrDefaultAsync(a => a.ActorId == id);
            if (actorWithPhoto != null)
                updatedActor.Photo = actorWithPhoto.Photo;

            return View(updatedActor);
        }

        // GET: Actors/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var actor = await _context.Actors.FirstOrDefaultAsync(a => a.ActorId == id);
            if (actor == null) return NotFound();

            return View(actor);
        }

        // POST: Actors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var actor = await _context.Actors.FindAsync(id);
            if (actor != null)
            {
                _context.Actors.Remove(actor);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
