using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Drawing;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Konsthuset.Data;
using Konsthuset.Models;
using Microsoft.AspNetCore.Authorization;

namespace Konsthuset.Controllers
{
    public class ArtController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;

        private string? wwwRootPath;

        public ArtController(ApplicationDbContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
            wwwRootPath = _hostEnvironment.WebRootPath;
        }

        // GET: Art
        [Authorize]
        public async Task<IActionResult> Index()
        {
            return _context.Artworks != null ?
                        View(await _context.Artworks.ToListAsync()) :
                        Problem("Entity set 'ApplicationDbContext.Artworks'  is null.");
        }

        //GET: Art with no editing options
        public async Task<IActionResult> List()
        {
            return _context.Artworks != null ?
            View(await _context.Artworks.ToListAsync()) :
            Problem("Entity set 'ApplicationDbContext.Artworks'  is null.");
        }

        //GET: Artist from Artwork
        public async Task<IActionResult> Artistname()
        {
            var artistContext = _context.Artworks.Include(c => c.ArtistName);
            return _context.Artworks != null ?
            View(await _context.Artworks.ToListAsync()) :
            Problem("Entity set 'ApplicationDbContext.Artworks'  is null.");
        }

        // GET: Art/Details/5
        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Artworks == null)
            {
                return NotFound();
            }

            var artwork = await _context.Artworks
                .FirstOrDefaultAsync(m => m.Id == id);
            if (artwork == null)
            {
                return NotFound();
            }

            return View(artwork);
        }

        // GET: Art/Create
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Art/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create([Bind("Id,ArtName,ArtYear,ArtistName,ArtTechnique,ArtPrice,ArtWidth,ArtHeight,AltText,ImageFile")] Artwork artwork)
        {
            if (ModelState.IsValid)
            {
                //attached image or not
                if (artwork.ImageFile != null)
                {
                    //save image in wwwroot w 2 new variables to make unique filenames
                    string fileName = Path.GetFileNameWithoutExtension(artwork.ImageFile.FileName);
                    string extension = Path.GetExtension(artwork.ImageFile.FileName);

                    //remove space and add timestamp    
                    artwork.ImageName = fileName = fileName.Replace(" ", String.Empty) + DateTime.Now.ToString("yymmssfff") + extension;
                    //where to store image    
                    string path = Path.Combine(wwwRootPath + "/imageupload/", fileName);
                    //store image
                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        await artwork.ImageFile.CopyToAsync(fileStream);
                    }
                }
                else
                {
                    artwork.ImageName = null;
                }

                _context.Add(artwork);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(artwork);
        }

        // GET: Art/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Artworks == null)
            {
                return NotFound();
            }

            var artwork = await _context.Artworks.FindAsync(id);
            if (artwork == null)
            {
                return NotFound();
            }
            return View(artwork);
        }

        // POST: Art/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ArtName,ArtYear,ArtistName,ArtTechnique,ArtPrice,ArtWidth,ArtHeight,AltText,ImageName")] Artwork artwork)
        {
            if (id != artwork.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(artwork);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ArtworkExists(artwork.Id))
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
            return View(artwork);
        }

        // GET: Art/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Artworks == null)
            {
                return NotFound();
            }

            var artwork = await _context.Artworks
                .FirstOrDefaultAsync(m => m.Id == id);
            if (artwork == null)
            {
                return NotFound();
            }

            return View(artwork);
        }

        // POST: Art/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Artworks == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Artworks'  is null.");
            }
            var artwork = await _context.Artworks.FindAsync(id);
            /*delete image from wwwroot */
            var imagePath = Path.Combine(_hostEnvironment.WebRootPath, "imageupload", artwork.ImageName);
            if (System.IO.File.Exists(imagePath))
                System.IO.File.Delete(imagePath);
            if (artwork != null)
            {
                _context.Artworks.Remove(artwork);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ArtworkExists(int id)
        {
            return (_context.Artworks?.Any(e => e.Id == id)).GetValueOrDefault();
        }

    }
}
