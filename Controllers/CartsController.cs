using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ShoppingCart.Data;
using ShoppingCart.Models;

namespace ShoppingCart.Controllers
{
    // use Authorize keyword: need to login to see the cart 
    [Authorize] 
    public class CartsController : Controller
    {
        private readonly ShoppingCartContext _context;

        public CartsController(ShoppingCartContext context)
        {
            _context = context;
        }

        // GET: Carts
        public async Task<IActionResult> Index()
        {
            return _context.Cart != null ?
                        View(await _context.Cart.ToListAsync()) :
                        Problem("Entity set 'ShoppingCartContext.Cart'  is null.");
        }

        // GET: Carts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Cart == null)
            {
                return NotFound();
            }

            var cart = await _context.Cart
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cart == null)
            {
                return NotFound();
            }

            return View(cart);
        }

        // only give access of GET Carts/Create URL to admin
        [Authorize(Roles = "Admin")]

        // GET: Carts/Create
        public IActionResult Create()
        {
            return View();
        }

        // only give access of POST Carts/Create URL to admin
        [Authorize(Roles = "Admin")]

        // POST: Carts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ItemId,Name,Price,Quantity,ImageUrl")] Cart cart)
        {
            if (ModelState.IsValid)
            {
                _context.Add(cart);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(cart);
        }

        // GET: Carts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Cart == null)
            {
                return NotFound();
            }

            var cart = await _context.Cart.FindAsync(id);
            if (cart == null)
            {
                return NotFound();
            }
            return View(cart);
        }

        // POST: Carts/UpdateQuantity
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateQuantity(int itemId, int quantity)
        {
            var cartItem = await _context.Cart.FindAsync(itemId);

            if (cartItem == null)
            {
                return NotFound();
            }

            cartItem.Quantity = quantity;

            _context.Update(cartItem);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }


        // POST: Carts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ItemId,Name,Price,Quantity,ImageUrl")] Cart cart)
        {
            if (id != cart.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cart);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CartExists(cart.Id))
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
            return View(cart);
        }

        // GET: Carts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Cart == null)
            {
                return NotFound();
            }

            var cart = await _context.Cart
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cart == null)
            {
                return NotFound();
            }

            return View(cart);
        }

        // POST: Carts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Cart == null)
            {
                return Problem("Entity set 'ShoppingCartContext.Cart'  is null.");
            }
            var cart = await _context.Cart.FindAsync(id);
            if (cart != null)
            {
                _context.Cart.Remove(cart);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CartExists(int id)
        {
          return (_context.Cart?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        // Method for "Add to Cart" button in Items Index page
        [HttpPost]
        public async Task<IActionResult> AddToCart(int itemId, int quantity)
        {
            var selectedItem = await _context.Item.FindAsync(itemId);

            if (selectedItem == null)
            {
                return NotFound();
            }

            if (quantity > selectedItem.Quantity)
            {
                TempData["Message"] = "Cannot add more than available inventory.";
                return RedirectToAction("Index", "Items");
            }

            var cartItem = new Cart
            {
                ItemId = selectedItem.Id,
                Name = selectedItem.Name,
                Price = selectedItem.Price,
                Quantity = quantity,
                ImageUrl = selectedItem.ImageUrl
            };

            _context.Cart.Add(cartItem);
            await _context.SaveChangesAsync();

            // set TempData message to notify the user that the item is added to the cart
            TempData["Message"] = "Item added to cart.";

            // redirect the user back to the Items Index page
            return RedirectToAction("Index", "Items");
        }

        // empty the cart when checkout
        // POST: Carts/Checkout
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Checkout()
        {
            if (_context.Cart != null)
            {
                foreach (var item in _context.Cart)
                {
                    var originalItem = await _context.Item.FindAsync(item.ItemId);
                    if (originalItem != null)
                    {
                        originalItem.Quantity -= item.Quantity;
                        _context.Entry(originalItem).State = EntityState.Modified;
                    }
                }

                _context.Cart.RemoveRange(_context.Cart);
                await _context.SaveChangesAsync();
            }

            // set TempData message to notify the user that the item is added to the cart
            TempData["Message"] = "Order Successful!";

            return RedirectToAction(nameof(Index));
        }

    }
}
