using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QuizManager.Data;
using QuizManager.Models;

namespace QuizManager.Controllers
{
    public class AnswerOptionsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AnswerOptionsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: AnswerOptions
        private async Task<IActionResult> Index()
        {
            return View(await _context.AnswerOption.ToListAsync());
        }

        // GET: AnswerOptions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var answerOption = await _context.AnswerOption
                .FirstOrDefaultAsync(m => m.AnswerOptionId == id);
            if (answerOption == null)
            {
                return NotFound();
            }

            return View(answerOption);
        }

        // GET: AnswerOptions/Create
        private IActionResult Create()
        {
            return View();
        }

        // POST: AnswerOptions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        private async Task<IActionResult> Create([Bind("AnswerOptionId,QuestionId,AnswerText,Correct")] AnswerOption answerOption)
        {
            if (ModelState.IsValid)
            {
                _context.Add(answerOption);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(answerOption);
        }

        // GET: AnswerOptions/Edit/5
        [Authorize(Roles = "Edit")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var answerOption = await _context.AnswerOption.FindAsync(id);
            if (answerOption == null)
            {
                return NotFound();
            }
            return View(answerOption);
        }

        // POST: AnswerOptions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Edit")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AnswerOptionId,QuestionId,AnswerText,Correct")] AnswerOption answerOption)
        {
            if (id != answerOption.AnswerOptionId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(answerOption);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AnswerOptionExists(answerOption.AnswerOptionId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Edit", "Questions", new { id = answerOption.QuestionId });
            }
            return View(answerOption);
        }

        // GET: AnswerOptions/Delete/5
        [Authorize(Roles = "Edit")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var answerOption = await _context.AnswerOption
                .FirstOrDefaultAsync(m => m.AnswerOptionId == id);
            if (answerOption == null)
            {
                return NotFound();
            }

            return View(answerOption);
        }

        // POST: AnswerOptions/Delete/5
        [Authorize(Roles = "Edit")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var answerOption = await _context.AnswerOption.FindAsync(id);

            if (!ModelState.IsValid)
            {
                return View(answerOption);
            }

            _context.AnswerOption.Remove(answerOption);
            await _context.SaveChangesAsync();
            return RedirectToAction("Edit", "Questions", new { id = answerOption.QuestionId });
        }

        private bool AnswerOptionExists(int id)
        {
            return _context.AnswerOption.Any(e => e.AnswerOptionId == id);
        }
    }
}
