using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Piscine.Data;
using Piscine.Models;

namespace Piscine.Controllers
{
    [Authorize(Policy = "Adminpolicy")]
    public class TypeReservationsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TypeReservationsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: TypeReservations

        [Authorize]
        public async Task<IActionResult> Index()
        {
            var type = await _context.typeReservations.ToListAsync();
            return View(type);
        }

        // GET: TypeReservations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var typeReservation = await _context.typeReservations.FirstOrDefaultAsync(m => m.Id == id);
            if (typeReservation == null)
            {
                return NotFound();
            }

            return View(typeReservation);
        }

        
        public IActionResult Create()
        {
            return View();
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] TypeReservation typeReservation)
        {
            if (ModelState.IsValid)
            {
                _context.Add(typeReservation);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(typeReservation);
        }

        
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var typeReservation = await _context.typeReservations.FindAsync(id);
            if (typeReservation == null)
            {
                return NotFound();
            }
            return View(typeReservation);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] TypeReservation typeReservation)
        {
            if (id != typeReservation.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(typeReservation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TypeReservationExists(typeReservation.Id))
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
            return View(typeReservation);
        }

        
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var typeReservation = await _context.typeReservations
                .FirstOrDefaultAsync(m => m.Id == id);
            if (typeReservation == null)
            {
                return NotFound();
            }

            return View(typeReservation);
        }

        // POST: TypeReservations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var typeReservation = await _context.typeReservations.FindAsync(id);
            _context.typeReservations.Remove(typeReservation);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TypeReservationExists(int id)
        {
            return _context.typeReservations.Any(e => e.Id == id);
        }
    }
}
