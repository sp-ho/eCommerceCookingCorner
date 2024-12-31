using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ShoppingCart.Data;
using ShoppingCart.Models;

namespace ShoppingCart.Controllers
{
    // use Authorize keyword: need to login to see the list of items 
    [Authorize]
    public class ItemsController : Controller
    {
        private readonly ShoppingCartContext _context;

        public ItemsController(ShoppingCartContext context)
        {
            _context = context;
        }

        // GET: Items
        public async Task<IActionResult> Index()
        {
              return _context.Item != null ? 
                          View(await _context.Item.ToListAsync()) :
                          Problem("Entity set 'ShoppingCartContext.Item'  is null.");
        }

        // GET: Items/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Item == null)
            {
                return NotFound();
            }

            var item = await _context.Item
                .FirstOrDefaultAsync(m => m.Id == id);
            if (item == null)
            {
                return NotFound();
            }

            return View(item);
        }

        // only give access of GET Items/Create URL to admin
        [Authorize(Roles = "Admin")]

        // GET: Items/Create
        public IActionResult Create()
        {
            var categories = new List<SelectListItem>
            {
                new SelectListItem { Text = "Pans", Value = "Pans" },
                new SelectListItem { Text = "Pots", Value = "Pots" },
                new SelectListItem { Text = "Woks", Value = "Woks" },
                new SelectListItem { Text = "Utensils", Value = "Utensils" },
                new SelectListItem { Text = "Coffee & Tea", Value = "Coffee & Tea" },
                new SelectListItem { Text = "Dinnerware", Value = "Dinnerware" }
            };
            ViewBag.CategoryList = new SelectList(categories, "Value", "Text");


            return View(); // open view of Create in the browser
        }

        // only give access of POST Items/Create URL to admin
        [Authorize(Roles = "Admin")]

        // POST: Items/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Price,Quantity,ImageUrl,Category,IsClearance,IsBestSeller")] Item item)
        {
            if (ModelState.IsValid)
            {
                _context.Add(item);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            var categories = new List<SelectListItem>
            {
                new SelectListItem { Text = "Pans", Value = "Pans" },
                new SelectListItem { Text = "Pots", Value = "Pots" },
                new SelectListItem { Text = "Woks", Value = "Woks" },
                new SelectListItem { Text = "Utensils", Value = "Utensils" },
                new SelectListItem { Text = "Coffee & Tea", Value = "Coffee & Tea" },
                new SelectListItem { Text = "Dinnerware", Value = "Dinnerware" }
            };
            ViewBag.CategoryList = new SelectList(categories, "Value", "Text", item.Category);

            return View(item);
        }

        // GET: Items/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Item == null)
            {
                return NotFound();
            }

            var item = await _context.Item.FindAsync(id);
            if (item == null)
            {
                return NotFound();
            }

            var categories = new List<SelectListItem>
            {
                new SelectListItem { Text = "Pans", Value = "Pans" },
                new SelectListItem { Text = "Pots", Value = "Pots" },
                new SelectListItem { Text = "Woks", Value = "Woks" },
                new SelectListItem { Text = "Utensils", Value = "Utensils" },
                new SelectListItem { Text = "Coffee & Tea", Value = "Coffee & Tea" },
                new SelectListItem { Text = "Dinnerware", Value = "Dinnerware" }
            };
            ViewBag.CategoryList = new SelectList(categories, "Value", "Text", item.Category);

            return View(item);
        }

        // POST: Items/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Price,Quantity,ImageUrl,Category,IsClearance,IsBestSeller")] Item item)
        {
            if (id != item.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(item);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ItemExists(item.Id))
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
            var categories = new List<SelectListItem>
            {
                new SelectListItem { Text = "Pans", Value = "Pans" },
                new SelectListItem { Text = "Pots", Value = "Pots" },
                new SelectListItem { Text = "Woks", Value = "Woks" },
                new SelectListItem { Text = "Utensils", Value = "Utensils" },
                new SelectListItem { Text = "Coffee & Tea", Value = "Coffee & Tea" },
                new SelectListItem { Text = "Dinnerware", Value = "Dinnerware" }
            };

            ViewBag.CategoryList = categories;

            return View(item);
        }

        // GET: Items/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Item == null)
            {
                return NotFound();
            }

            var item = await _context.Item
                .FirstOrDefaultAsync(m => m.Id == id);
            if (item == null)
            {
                return NotFound();
            }

            return View(item);
        }

        // POST: Items/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Item == null)
            {
                return Problem("Entity set 'ShoppingCartContext.Item'  is null.");
            }
            var item = await _context.Item.FindAsync(id);
            if (item != null)
            {
                _context.Item.Remove(item);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ItemExists(int id)
        {
          return (_context.Item?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        public async Task<IActionResult> Pans()
        {
            var items = await _context.Item.Where(i => i.Category == "Pans").ToListAsync();
            return View(items);
        }

        public async Task<IActionResult> Pots()
        {
            var items = await _context.Item.Where(i => i.Category == "Pots").ToListAsync();
            return View(items);
        }

        public async Task<IActionResult> Woks()
        {
            var items = await _context.Item.Where(i => i.Category == "Woks").ToListAsync();
            return View(items);
        }

        public async Task<IActionResult> Utensils()
        {
            var items = await _context.Item.Where(i => i.Category == "Utensils").ToListAsync();
            return View(items);
        }

        public async Task<IActionResult> CoffeeTea()
        {
            var items = await _context.Item.Where(i => i.Category == "Coffee & Tea").ToListAsync();
            return View(items);
        }

        public async Task<IActionResult> Dinnerware()
        {
            var items = await _context.Item.Where(i => i.Category == "Dinnerware").ToListAsync();
            return View(items);
        }

        public async Task<IActionResult> BestSellers()
        {
            var bestSellers = await _context.Item.Where(i => i.IsBestSeller).ToListAsync();
            return View(bestSellers);
        }

        public async Task<IActionResult> Clearance()
        {
            var clearanceItems = await _context.Item.Where(i => i.IsClearance).ToListAsync();
            return View(clearanceItems);
        }
    }
}
