using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReadmoreWeb.Data;
using ReadmoreWeb.Data.Models;

namespace ReadmoreWeb.Controllers
{
    public class BooksController : Controller
    {
        private readonly ReadmoreDbContext _context;

        public BooksController(ReadmoreDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var books = await _context.Books.ToListAsync();
            return View(books);
        }

        [HttpGet]
        public async Task<IActionResult> Search(string query)
        {
            query = (query ?? string.Empty).Trim().ToLower();

            var booksQuery = _context.Books.AsQueryable();

            if (!string.IsNullOrWhiteSpace(query))
            {
                booksQuery = booksQuery.Where(b =>
                    (b.Title != null && b.Title.ToLower().Contains(query)) ||
                    (b.Author != null && b.Author.ToLower().Contains(query))
                );
            }

            var books = await booksQuery.ToListAsync();
            return PartialView("_BooksTable", books);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var book = await _context.Books.FirstOrDefaultAsync(b => b.Id == id);
            if (book == null) return NotFound();

            return View(book);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Author,Price,PublishedDate")] Book book)
        {
            if (!ModelState.IsValid) return View(book);

            _context.Add(book);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var book = await _context.Books.FindAsync(id);
            if (book == null) return NotFound();

            return View(book);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Author,Price,PublishedDate")] Book book)
        {
            if (id != book.Id) return NotFound();
            if (!ModelState.IsValid) return View(book);

            try
            {
                _context.Update(book);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookExists(book.Id)) return NotFound();
                throw;
            }

            return RedirectT
