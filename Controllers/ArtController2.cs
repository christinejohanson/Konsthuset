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

namespace Konsthuset.Controllers
{
    public class ArtController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;

        //private string? wwwRootPath;
        /*imagesize settings
        private int ImageLargeWidth = 1000;
        private int ImageLargeHeight = 800; */

        public ArtController(ApplicationDbContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            this._hostEnvironment = hostEnvironment;
            //wwwRootPath = _hostEnvironment.WebRootPath;
        }

        // GET: Art
        public async Task<IActionResult> Index()
        {
            return _context.Artworks != null ?
                        View(await _context.Artworks.ToListAsync()) :
                        Problem("Entity set 'ApplicationDbContext.Artworks'  is null.");
        }

        // GET: Art/Details/5
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
        public IActionResult Create()
        {
            return View();
        }

        // POST: Art/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ArtName,ArtYear,ArtistName,ArtTechnique,ArtPrice,ArtWidth,ArtHeight,ImageFile")] Artwork artwork)
        {
            if (ModelState.IsValid)
            {

                string wwwRootPath = _hostEnvironment.WebRootPath;

                //save image in wwwroot w 2 new variables to make unique filenames
                string fileName = Path.GetFileNameWithoutExtension(artwork.ImageFile.FileName);
                string extension = Path.GetExtension(artwork.ImageFile.FileName);

                //add timestamp    
                artwork.ImageName = fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                //where to store image    
                string path = Path.Combine(wwwRootPath + "/imageupload/", fileName);
                //store image
                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    await artwork.ImageFile.CopyToAsync(fileStream);
                }
                /*create miniatures
                CreateImageFiles(fileName); */


                _context.Add(artwork);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(artwork);
        }

        // GET: Art/Edit/5
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
        public async Task<IActionResult> Edit(int id, [Bind("Id,ArtName,ArtYear,ArtistName,ArtTechnique,ArtPrice,ArtWidth,ArtHeight,ImageFile")] Artwork artwork)
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
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Artworks == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Artworks'  is null.");
            }
            var artwork = await _context.Artworks.FindAsync(id);
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

        /*method for imageupload 
        private void CreateImageFiles(string filename)
        {
            //create large image
            using (var img = Image.FromFile(Path.Combine(wwwRootPath + "/imageupload/", filename)))
            {
                img.SaveAs(Path.Combine(wwwRootPath + "/imageupload/", "large_" + filename));
            }
        } */
    }
}
