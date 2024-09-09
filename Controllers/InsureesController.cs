using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Car_Insurance_VS22.Data;
using Car_Insurance_VS22.Models;
using System.ComponentModel.DataAnnotations;

namespace Car_Insurance_VS22.Controllers
{
    public class InsureesController : Controller
    {
        private readonly QuoteContext _context;

        public 
            InsureesController(QuoteContext context)
        {
            _context = context;
        }

        //method to calculate a quote
        private decimal CalculateQuote(Insuree insuree)
        {
            Double Quote = 50;
            var today = DateTime.Today;
            var age = today.Year - insuree.DateOfBirth.Year;
            //how age affects quote
            if (age <= 18) { Quote += 100; }
            if (age > 18 && age < 26) { Quote += 50; }
            if (age >= 26) { Quote += 25; }

            //how car year affects quote
            int carYearNum = int.Parse(insuree.CarYear);
            if (carYearNum < 2000) { Quote += 25; }
            if (carYearNum > 2015) { Quote += 25; }

            //how car make & model affects quote
            if (insuree.CarMake.ToLower().Equals("porsche")) { Quote += 25; }
            if (insuree.CarModel.ToLower().Contains("carrera")) { Quote += 25; }

            //how speeding tickets affect quote
            int numOfTickets = int.Parse(insuree.SpeedingTickets);
            if (numOfTickets > 0) { Quote += (numOfTickets * 10); }

            //how DUI affects quote
            int numOfDUI = int.Parse(insuree.DUI);
            if (numOfDUI > 0) { Quote *= 1.25; }

            //how full coverage affects quote
            if (insuree.CoverageType.ToLower().Equals("full")) { Quote *= 1.5; }

            return (decimal)Quote;
        }

        // GET: Insurees/Admin
        public async Task<IActionResult> Admin()
        {
            var insurees = await _context.Insuree.ToListAsync();
            var insureeQuotes = insurees.Select(i => new InsureeQuoteAdminModel
            {
                FirstName = i.FirstName,
                LastName = i.LastName,
                EmailAddress = i.EmailAddress,
                Quote = CalculateQuote(i)
            }).ToList();

            return View(insureeQuotes);
        }


        // GET: Insurees
        public async Task<IActionResult> Index()
        {
            return View(await _context.Insuree.ToListAsync());
        }

        // GET: Insurees/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var insuree = await _context.Insuree
                .FirstOrDefaultAsync(m => m.Id == id);
            if (insuree == null)
            {
                return NotFound();
            }

            return View(insuree);
        }

        // GET: Insurees/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Insurees/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FirstName,LastName,EmailAddress,DateOfBirth,CarYear,CarMake,CarModel,DUI,SpeedingTickets,CoverageType,Quote")] Insuree insuree)
        {



            if (ModelState.IsValid)
            {
                insuree.Id = Guid.NewGuid();
                insuree.Quote = (double)CalculateQuote(insuree);
                _context.Add(insuree);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Success), new { id = insuree.Id });
            }
            return View(insuree);
        }

        // GET: Insurees/Success/5
        public async Task<IActionResult> Success(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var insuree = await _context.Insuree.FirstOrDefaultAsync(m => m.Id == id);
            if (insuree == null)
            {
                return NotFound();
            }

            return View(insuree);
        }

        // GET: Insurees/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var insuree = await _context.Insuree.FindAsync(id);
            if (insuree == null)
            {
                return NotFound();
            }
            return View(insuree);
        }

        // POST: Insurees/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,FirstName,LastName,EmailAddress,DateOfBirth,CarYear,CarMake,CarModel,DUI,SpeedingTickets,CoverageType,Quote")] Insuree insuree)
        {
            if (id != insuree.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(insuree);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InsureeExists(insuree.Id))
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
            return View(insuree);
        }

        // GET: Insurees/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var insuree = await _context.Insuree
                .FirstOrDefaultAsync(m => m.Id == id);
            if (insuree == null)
            {
                return NotFound();
            }

            return View(insuree);
        }

        // POST: Insurees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var insuree = await _context.Insuree.FindAsync(id);
            if (insuree != null)
            {
                _context.Insuree.Remove(insuree);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool InsureeExists(Guid id)
        {
            return _context.Insuree.Any(e => e.Id == id);
        }
    }
}
