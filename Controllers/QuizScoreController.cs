using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NepHubAPI.Data;
using NepHubAPI.Dtos.Quiziz;
using NepHubAPI.Models;

namespace NepHubAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuizScoreController : ControllerBase
    {
                private readonly NepHubContext _context;

     public QuizScoreController(NepHubContext context)
        {
            _context = context;
        }


        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var entity = await _context.QuizScores.ToListAsync();
            return Ok(entity);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var item = await _context.QuizScores.FindAsync(id);
            if (item is null)
            {
                return NotFound();
            }
            return Ok(item);
        }

        [HttpPost]
        public async Task<IActionResult> AddPrimeMinister(ScoreDto scoreDto)
        {
            var newScore = new QuizScore
            {
                Score = scoreDto.Score,
                QuizId = scoreDto.QuizId,
                UserId = scoreDto.UserId,
                ScoredDate = scoreDto.ScoredDate
            };
            await _context.QuizScores.AddAsync(newScore);
            await _context.SaveChangesAsync();
            return Created();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEntity([FromRoute] int id, ScoreDto scoreDto)
        {
            var dbData = await _context.QuizScores.FindAsync(id);

            if (dbData is null)
            {
                return NotFound();
            }
            dbData.Score = scoreDto.Score;
            dbData.ScoredDate = scoreDto.ScoredDate;

            await _context.QuizScores.AddAsync(dbData);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEntity([FromRoute] int id)
        {
            var score = await _context.QuizScores.FindAsync(id);
            if (score == null)
            {
                return NotFound();
            }

            _context.QuizScores.Remove(score);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    };
}
