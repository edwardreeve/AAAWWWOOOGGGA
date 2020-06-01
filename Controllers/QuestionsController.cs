using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
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
        public async Task<IActionResult> Create([Bind("QuestionId,QuizId,QuestionText,Position,AnswerOptions")] Question question)
        {
            if (!ModelState.IsValid)
            {
                return View(question);
            }

            //Need to delete any blank questions. Work down the list, as otherwise the
            //count of the answers drops lower than the iterating index following a deletion
            var answerOptions = question.AnswerOptions;

            for (var i = answerOptions.Count - 1; i >= 0; i--)
            {
                var answerOption = answerOptions[i];
               
                if (answerOption.Correct == false && answerOption.AnswerText == null)
                {
                    answerOptions.RemoveAt(i);
                }
            }

            _context.Add(question);
            await _context.SaveChangesAsync();
            TempData.Clear();
            return RedirectToAction("Details", "Quizzes", new { id = question.QuizId });
        }

        // GET: Questions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            /*If this action has been reached via the 'AddAnswerOption' method, it will let us know 
           using TempData if that action would have resulted in too many answers*/
            if (TempData.Count > 0)
            {
                if (TempData["CannotAddAnswers"] != null)
                {
                    ModelState.AddModelError("AnswerOptionsError", "Questions can't have more than 5 Answer options");
                }

                if (TempData["CannotDeleteAnswers"] != null)
                {
                    ModelState.AddModelError("AnswerOptionsError", "Questions can't have less than 3 Answer options");
                }

                TempData.Clear();
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
        public async Task<IActionResult> Edit(int id, [Bind("QuestionId," +
                                                            "QuizId," +
                                                            "QuestionText," +
                                                            "Position," +
                                                            "AnswerOptions," +
                                                            "AnswerOptions.AnswerOption.Correct")] Question question)
        {
            if (id != question.QuestionId)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View(question);
            }

            //Need to delete any blank questions. Work down the list, as otherwise the
            //count of the answers drops lower than the iterating index following a deletion
            var answerOptions = question.AnswerOptions;
            var answersToDelete = new List<AnswerOption>();
            for (var i = answerOptions.Count - 1; i >= 0; i--)
            {
                var answerOption = answerOptions[i];

                if (answerOption.Correct == false && answerOption.AnswerText == null)
                {
                    answerOptions.RemoveAt(i);
                    answersToDelete.Add(answerOption);
                }
            }

            if (answersToDelete.Any())
            {
                _context.AnswerOption.RemoveRange(answersToDelete);
                await _context.SaveChangesAsync();
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

        public async Task<IActionResult> AddAnswerOption(int id)
        {
            //Retrieve Question
            var questions = await _context.Question
                .Where(question => question.QuestionId == id)
                .Include(question => question.AnswerOptions)
                .ToListAsync();

            var selectedQuestion = questions.FirstOrDefault();

            /*Check if it already has the max number of answers, add error to TempData if so.
            Edit action checks for temp data and adds validation errors if populated*/  
            if (selectedQuestion.AnswerOptions.Count >= 5)
            {
                TempData["CannotAddAnswers"] = true;
                TempData.Save();
                return RedirectToAction("Edit", new { id = id, Question = selectedQuestion });
            }

            //Create new blank answer
            var newAnswer = new AnswerOption()
            {
                AnswerText = "",
                QuestionId = id
            };

            //Add to question and save in DB
            selectedQuestion.AnswerOptions.Add(newAnswer);
            _context.Question.Update(selectedQuestion);
            await _context.SaveChangesAsync();

            //Reload the edit question view
            return RedirectToAction("Edit", new {id = id});
        }

        public async Task<IActionResult> DeleteAnswerOption(int answerId, int questionId)
        {
            //Retrieve Question
            var questions = await _context.Question
                .Where(question => question.QuestionId == questionId)
                .Include(question => question.AnswerOptions)
                .ToListAsync();

            var selectedQuestion = questions.FirstOrDefault();

            /*Check if it already has the min number of questions, add error to TempData if so.
            Edit action checks for temp data and adds validation errors if populated */
            if (selectedQuestion.AnswerOptions.Count <= 3)
            {
                TempData["CannotDeleteAnswers"] = true;
                TempData.Save();
                return RedirectToAction("Edit", new { id = questionId });
            }

            //Redirect for confirmation of delete
            return RedirectToAction("Delete", "AnswerOptions", new {id = answerId});
        }

        private bool QuestionExists(int id)
        {
            return _context.Question.Any(e => e.QuestionId == id);
        }
    }
}
