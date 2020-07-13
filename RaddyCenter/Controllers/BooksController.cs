using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaddyCenter.Data.DataAccess;
using RaddyCenter.Data.Models;
using RaddyCenter.Models;

namespace RaddyCenter.Controllers
{
    [Authorize(Roles ="Admin,Moderator")]
    public class BooksController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        private readonly string _imageFolderPath = "/images/";
        private readonly string _imagesFolder = "/images/";

        public BooksController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            this._context = context;
            this._userManager = userManager;

            _imagesFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");

        }

        public async Task<IActionResult> Index()
        {
            var books = await _context
                .Books
                .Where(book => book.Disabled == false)
                .Select(book => new BookViewModel()
                {
                    Id=book.Id,
                    Author=book.Author,
                    Title=book.Title,
                    Available=book.Available,
                    CoverImagePath=_imageFolderPath + book.CoverImagePath,
                    Description = book.Description,
                    Quantity = book.Quantity,
                    Year = book.Year
                }
                ).ToListAsync();

            return View(books);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateBookViewModel model)
        {
            if (ModelState.IsValid)
            {
                // 1024 * 1024 1MB
                if (model.CoverImage.Length > 1024 * 1024)
                {
                    ModelState.AddModelError("CoverImage", "Image size must be under 1MB");
                    return View(model);
                }

                if (!model.CoverImage.ContentType.Contains("image"))
                {
                    ModelState.AddModelError("CoverImage", "Cover photo must be a png or jpg");
                    return View(model);
                }

                var ext = Path.GetExtension(model.CoverImage.FileName);

                var name = Guid.NewGuid();

                var book = new Book();
                book.Author = model.Author;
                book.Title = model.Title;
                book.Available = model.Quantity;
                book.CoverImagePath = name+ext;
                book.Description = model.Description;
                book.Quantity = model.Quantity;
                book.Year = model.Year;


                while (await _context.Books.AnyAsync(c => c.CoverImagePath == book.CoverImagePath))
                {
                    name = Guid.NewGuid();
                    book.CoverImagePath = name + ext;
                }

                var path = Path.Combine(_imagesFolder, book.CoverImagePath);

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await model.CoverImage.CopyToAsync(stream);
                }

                await _context.Books.AddAsync(book);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }
    
    }
}
