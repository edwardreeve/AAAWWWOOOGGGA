using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QuizManager.Data;
using QuizManager.Models;

namespace QuizManager.Controllers
{
    public class QuestionsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public QuestionsController(ApplicationDbContext context)
        {
            _context = context;
        }

        //Making private as not needed yet in current app implementation
        private async Task<IActionResult> Index()
        {
            return View(await _context.Question.ToListAsync());
        }
        
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var question = await _context.Question
                .FirstOrDefaultAsync(m => m.QuestionId == id);
            if (question == null)
            {
                return NotFound();
            }

            return View(question);
        }

        // GET: Questions/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Questions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("QuestionId,QuizId,QuestionText,Position")] Question question)
        {
            if (question.AnswerOptions.Count < 3 && question.AnswerOptions.Count > 5)
            {
                ModelState.AddModelError("AnswerOptionsError", "Please enter between 3 and 5 answer options");
            }

            if (!question.AnswerOptions.Any(answer => answer.Correct))
            {
                ModelState.AddModelError("AnswerOptionsError", "At least one answer option must be marked as Correct");
            }

            if (!ModelState.IsValid)
            {
                return View(question);
            }

            _context.Add(question);
            await _context.SaveChangesAsync();
            return RedirectToAction("Details", "Quizzes", new { id = question.QuizId });
        }

        // GET: Questions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var questions = await _context.Question
                .Where(question => question.QuestionId == id)
                .Include(question => question.AnswerOptions)
                .ToListAsync();

            var selectedQuestion = questions.FirstOrDefault();
            if (selectedQuestion == null)
            {
                return NotFound();
            }
            return View(selectedQuestion);
        }

        // POST: Questions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("QuestionId,QuizId,QuestionText,Position,AnswerOptions,AnswerOptions.AnswerOption.Correct")] Question question)
        {
            if (id != question.QuestionId)
            {
                return NotFound();
            }

            if (question.AnswerOptions.Count < 3 || question.AnswerOptions.Count > 5)
            {
                ModelState.AddModelError("AnswerOptionsError", "Please enter between 3 and 5 answer options");
            }

            if (!question.AnswerOptions.Any(answer => answer.Correct))
            {
                ModelState.AddModelError("AnswerOptionsError", "At least one answer option must be marked as Correct");
            }

            if (!ModelState.IsValid)
            {
                return View(question);
            }

            try
            {
                _context.Update(question);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!QuestionExists(question.QuestionId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction("Details", "Quizzes", new { id = question.QuizId});
        }

        // GET: Questions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var question = await _context.Question
                .FirstOrDefaultAsync(m => m.QuestionId == id);
            if (question == null)
            {
                return NotFound();
            }

            return View(question);
        }

        // POST: Questions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var question = await _context.Question.FindAsync(id);
            _context.Question.Remove(question);
            await _context.SaveChangesAsync();
            return RedirectToAction("Details", "Quizzes", new { id = question.QuizId });
        }

        private bool QuestionExists(int id)
        {
            return _context.Question.Any(e => e.QuestionId == id);
        }
    }
}
