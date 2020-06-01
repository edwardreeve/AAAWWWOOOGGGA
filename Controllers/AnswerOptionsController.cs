using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuizManager.Data;

namespace QuizManager.Controllers
{
    public class AnswerOptionsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AnswerOptionsController(ApplicationDbContext context)
        {
            _context = context;
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
    }
}
