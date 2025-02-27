using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NepHubAPI.Data;
using NepHubAPI.Dtos.Quiziz;
using NepHubAPI.Models;

namespace NepHubAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuizizController(NepHubContext context) : ControllerBase
    {
        private readonly NepHubContext _context = context;

         [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            var list =  await _context.Quiziz.ToListAsync();
            return Ok(list);
        }

        //get by id
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var question = await _context.Quiziz.FindAsync(id);

            if (question == null) {
                return NotFound();
            }
            return Ok(question);
        }

        [HttpPost]
        public async Task<IActionResult> CreateQuiz([FromBody] CreateQuizizDto quizDto)
        {
            if (quizDto == null)
            {
                return BadRequest();
            }

            var quiz = new Quiziz
            {
                Question = quizDto.Question,
                Options = quizDto.Options,
                CorrectAnswer = quizDto.CorrectAnswer,
                Category = quizDto.Category
            };

            await _context.Quiziz.AddAsync(quiz);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = quiz.Id }, quiz);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateQuiz([FromRoute] int id, [FromBody] UpdateQuizizDto updatedDto)
        {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            if (updatedDto == null) {
                return BadRequest();
            }

            var dbData = await _context.Quiziz.FindAsync(id);

            if (dbData == null) 
            {
                return BadRequest();
            }

                dbData.Question = updatedDto.Question;
                dbData.Options = updatedDto.Options;
                dbData.CorrectAnswer = updatedDto.CorrectAnswer;     
                dbData.Category = updatedDto.Category;
     

            _context.Quiziz.Update(dbData);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteQuiz([FromRoute] int id) {
            var quiz = await _context.Quiziz.FindAsync(id);
            if (quiz == null) {
                return NotFound();
            }

            _context.Quiziz.Remove(quiz);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
