using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Google.Apis.Services;
using Google.Apis.Util;
using Google.Apis.YouTube.v3;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RemitanoDevTask.Models;
using RemitanoDevTask.ViewModels;

namespace RemitanoDevTask.Controllers
{
    [Authorize]
    public class MoviesController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public MoviesController(AppDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Movies
        public async Task<IActionResult> Index()
        {
            return View(await _context.Movies.ToListAsync());
        }

        // GET: Movies/Details/5
        public async Task<IActionResult> Details(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _context.Movies
                .FirstOrDefaultAsync(m => m.MovieId == id);
            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }

        // GET: Movies/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Movies/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MovieViewModel movieViewModel)
        {
            if (ModelState.IsValid)
            {
                var id = movieViewModel.VideoUrl.Split("v=")[1];

                var youtubeService = new YouTubeService(new BaseClientService.Initializer()
                {
                    ApiKey = "AIzaSyBQgrct6TZsFCro103WWwXLcKl6_aL28H0",
                    ApplicationName = this.GetType().ToString()
                });

                var searchListRequest = youtubeService.Videos.List("snippet,contentDetails,statistics"); searchListRequest.Id = id;

                // Call the search.list method to retrieve results matching the specified query term.
                var searchListResponse = await searchListRequest.ExecuteAsync();

                IdentityUser user = await _userManager.FindByIdAsync(_userManager.GetUserId(HttpContext.User));

                //var aaa = _context.Movies.LastOrDefault().MovieId;

                if (searchListResponse.Items.Count != 0)
                {
                    var movie = new Movie()
                    {
                        //MovieId = _context.Movies.LastOrDefault().MovieId + 1,
                        Id = searchListResponse.Items[0].Id,
                        Title = searchListResponse.Items[0].Snippet.Title,
                        Description = searchListResponse.Items[0].Snippet.Description,
                        PublishedAt = searchListResponse.Items[0].Snippet.PublishedAt,
                        ChannelTitle = searchListResponse.Items[0].Snippet.ChannelTitle,
                        SharedBy = user.ToString(),
                        VideoUrl = movieViewModel.VideoUrl,
                        LikeCount = (int)searchListResponse.Items[0].Statistics.LikeCount,
                        DislikeCount = (int)searchListResponse.Items[0].Statistics.DislikeCount,
                        CommentCount = (int)searchListResponse.Items[0].Statistics.CommentCount
                    };
                    _context.Add(movie);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    ModelState.AddModelError("VideoUrl", "This video does not exist.");
                    return View(movieViewModel);
                }
                return RedirectToAction("Index", "Home");
            }
            return View(movieViewModel);
        }

        // GET: Movies/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _context.Movies.FindAsync(id);
            if (movie == null)
            {
                return NotFound();
            }
            return View(movie);
        }

        // POST: Movies/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MovieId,Title,Description,PublishedAt,ChannelTitle,VideoUrl,LikeCount,DislikeCount,CommentCount")] Movie movie)
        {
            if (id != movie.MovieId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(movie);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MovieExists(movie.MovieId))
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
            return View(movie);
        }

        // GET: Movies/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _context.Movies
                .FirstOrDefaultAsync(m => m.MovieId == id);
            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }

        // POST: Movies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var movie = await _context.Movies.FindAsync(id);
            _context.Movies.Remove(movie);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MovieExists(int id)
        {
            return _context.Movies.Any(e => e.MovieId == id);
        }
    }
}
