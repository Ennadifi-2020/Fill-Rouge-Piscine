using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Piscine.Data;
using Piscine.Models;
using Piscine.Models.contract;

namespace Piscine.Controllers
{
    [Authorize]
    public class ReservationsController : Controller
    {
        private readonly IReservationRepository _reservatrepo;
        private readonly IReservationTypeRepository _reservatTypeRepo;
        private readonly ApplicationDbContext _context;
        private UserManager<Client> _UserManager;


        public ReservationsController(ApplicationDbContext context , UserManager<Client> userManager , IReservationRepository reservationRepo, IReservationTypeRepository reservationTypeRepo)
        {
            _reservatrepo = reservationRepo;
            _reservatTypeRepo = reservationTypeRepo;
            _context = context;
            _UserManager = userManager;
        }

        // GET: Reservations
        /*   public async Task<IActionResult> Index()
           {
               ViewBag.role = new IdentityRole();
               var applicationDbContext = _context.reservations.Include(r => r.Client).Include(r => r.TypeReservation);
               return View(await applicationDbContext.ToListAsync());
           }*/

        [Authorize(Policy = "Clientpolicy")]

        public async Task<ActionResult> Index()
        {
            var userr = await _UserManager.GetUserAsync(User);
             
            if (User.IsInRole("Admin"))
            {
                var reservation = _reservatrepo.GetAll().OrderBy(x => x.ClientId);
                return View(reservation);
            }
            else
            {
                var reservation = _reservatrepo.GetAll().Where(r => r.ClientId == userr.Id);
                return View(reservation);

            }


        }

        public async Task<IActionResult> Accepter(int id)
        {
            var resr = _context.reservations.Find(id);
            if (resr.Status != "Accepter")
            {
                
                resr.Status = "Accepter";
                _context.Update(resr);
                await _context.SaveChangesAsync();
            
            }

            return RedirectToAction("index");
        }



        public IActionResult Refuser(int id)
        {
            var resr = _context.reservations.Find(id);

            if (resr.Status != "Declined")
            {
                

                resr.Status = "Refuser";
                _context.Update(resr);
                _context.SaveChanges();
               

            }

            return RedirectToAction("index");

        }

        // GET: Reservations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reservation = await _context.reservations
                .Include(r => r.Client)
                .Include(r => r.TypeReservation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (reservation == null)
            {
                return NotFound();
            }

            return View(reservation);
        }

        // GET: Reservations/Create
        public IActionResult Create()
        {
            ViewData["ClientId"] = new SelectList(_context.clients, "Id", "Id");
            ViewData["TypeName"] = new SelectList(_context.typeReservations, "Id", "Name");
            ViewBag.type = _context.typeReservations.ToList();
            return View();
        }

        // POST: Reservations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
     
        public  IActionResult Create(Reservation reservation)
        {
            /*   if (ModelState.IsValid)
               {
                   _context.Add(reservation);
                   await _context.SaveChangesAsync();
                   return RedirectToAction(nameof(Index));
               }
               ViewData["ClientId"] = new SelectList(_context.clients, "Id", "Id", reservation.ClientId);
               ViewData["TypeReservationId"] = new SelectList(_context.typeReservations, "Id", "Id", reservation.TypeReservationId);
               return View(reservation);*/
            var userId = _UserManager.GetUserId(HttpContext.User);

            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {

                reservation.Client = _UserManager.FindByIdAsync(userId).Result;
            }


            this._context.reservations.Add(reservation);
            this._context.SaveChanges();


            /*    if (ModelState.IsValid)
                {

                    TempData["msg"] = " your request is added ";


                }*/
            return RedirectToAction(nameof(Index));

        }

        // GET: Reservations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reservation = await _context.reservations.FindAsync(id);
            if (reservation == null)
            {
                return NotFound();
            }
            ViewData["ClientId"] = new SelectList(_context.clients, "Id", "Id", reservation.ClientId);
            ViewData["TypeName"] = new SelectList(_context.typeReservations, "Id", "Name", reservation.TypeReservationId);
            return View(reservation);
        }

        // POST: Reservations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Date,Status,ClientId,TypeReservationId")] Reservation reservation)
        {
            if (id != reservation.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(reservation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReservationExists(reservation.Id))
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
            ViewData["ClientId"] = new SelectList(_context.clients, "Id", "Id", reservation.ClientId);
            ViewData["TypeReservationId"] = new SelectList(_context.typeReservations, "Id", "Id", reservation.TypeReservationId);
            return View(reservation);
        }

        // GET: Reservations/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reservation = await _context.reservations
                .Include(r => r.Client)
                .Include(r => r.TypeReservation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (reservation == null)
            {
                return NotFound();
            }

            return View(reservation);
        }

        // POST: Reservations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var reservation = await _context.reservations.FindAsync(id);
            _context.reservations.Remove(reservation);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ReservationExists(int id)
        {
            return _context.reservations.Any(e => e.Id == id);
        }
    }
}
