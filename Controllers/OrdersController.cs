using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LappyShop.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace LappyShop.Controllers
{
    [Authorize]
    public class OrdersController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public OrdersController(AppDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Order
        public async Task<IActionResult> Index()
        {
            var userid = _userManager.GetUserId(HttpContext.User);
            List<Order> orders = await _context.orders.ToListAsync();

            if (!User.IsInRole("Admin"))
            {
                orders = orders.Where(u => u.user_id == userid).ToList();
            }

            List<Orderdetails> orderDetails = new List<Orderdetails>();

            for (int i = 0; i < orders.Count; i++)
            {
                var laptop = _context.products.FirstOrDefault(m => m.ID == orders[i].laptop_id);
                var user = await _context.Users.FirstOrDefaultAsync(m => m.Id == orders[i].user_id);
                Orderdetails orderDetail = new Orderdetails
                {
                    Product_name = laptop.name,
                    Product_ImageUrl = laptop.image,
                    Product_Price = laptop.price,
                    User_id = userid,
                    UserName = user.UserName,
                    UserEmail = user.Email,
                    UserPhoneNumber = user.PhoneNumber,
                    Order_id = orders[i].Order_ID
                };
                orderDetails.Add(orderDetail);
            }
            ViewData["detailsOfOrder"] = orderDetails;

            return View(orders);
        }

        // GET: Order/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orderModel = await _context.orders
                .FirstOrDefaultAsync(m => m.Order_ID == id);
            var laptop = _context.products.FirstOrDefault(l => l.ID == orderModel.laptop_id);
            ViewData["laptop"] = laptop;
            if (orderModel == null)
            {
                return NotFound();
            }

            return View(orderModel);
        }

        // GET: Order/Create
        public async Task<IActionResult> Create(int id)
        {
            var userid = _userManager.GetUserId(HttpContext.User);
            var laptop = _context.products.FirstOrDefault(m => m.ID == id);
            var user = await _userManager.GetUserAsync(User);
            Orderdetails orderDetails = new Orderdetails
            {
                Product_id = laptop.ID,
                Product_name = laptop.name,
                Product_ImageUrl = laptop.image,
                Product_Price = laptop.price,
                User_id = userid,
                UserName = user.UserName,
                UserEmail = user.Email
            };
            ViewData["details"] = orderDetails;
            return View();
        }

        // POST: Order/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Order orderModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(orderModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(orderModel);
        }

        // GET: Order/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orderModel = await _context.orders.FindAsync(id);
            if (orderModel == null)
            {
                return NotFound();
            }
            return View(orderModel);
        }

        // POST: Order/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("ID,laptop_id,user_id")] Order orderModel)
        {
            if (id != orderModel.Order_ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(orderModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderModelExists(orderModel.Order_ID))
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
            return View(orderModel);
        }

        // GET: Order/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orderModel = await _context.orders
                .FirstOrDefaultAsync(m => m.Order_ID == id);
            var laptop = _context.products.FirstOrDefault(l => l.ID == orderModel.laptop_id);
            ViewData["laptop"] = laptop;
            if (orderModel == null)
            {
                return NotFound();
            }

            return View(orderModel);
        }

        // POST: Order/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var orderModel = await _context.orders.FindAsync(id);
            _context.orders.Remove(orderModel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrderModelExists(string id)
        {
            return _context.orders.Any(e => e.Order_ID == id);
        }
    }
}
