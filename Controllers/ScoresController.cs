using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CsvTopScorerApi.Data;
using CsvTopScorerApi.Models;
using System.Collections.Generic;
using System.Linq;

namespace TopScorersApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScoresController : ControllerBase
    {
        private readonly ScoreContext _context;

        // Inject database context
        public ScoresController(ScoreContext context)
        {
            _context = context;
        }

        // POST api/scores - Adds a new score to the database
        [HttpPost]
        public IActionResult AddScore([FromBody] Score score)
        {
            _context.Scores.Add(score);
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetScore), new { firstName = score.FirstName, secondName = score.SecondName }, score);
        }

        // GET api/scores/{firstName} {secondName} - Retrieves a score based on first and second names
        [HttpGet("{firstName} {secondName}")]
        public IActionResult GetScore(string firstName, string secondName)
        {
            var score = _context.Scores
                .FirstOrDefault(s => s.FirstName == firstName && s.SecondName == secondName);
            if (score == null)
            {
                return NotFound();
            }
            return Ok(score);
        }

        // GET api/scores/top-scorers - Retrieves the top scorers from the database
        [HttpGet("top-scorers")]
        public IActionResult GetTopScorers()
        {
            var topScore = _context.Scores.Max(s => s.ScoreValue);
            var topScorers = _context.Scores
                .Where(s => s.ScoreValue == topScore)
                .OrderBy(s => s.FirstName + " " + s.SecondName)
                .ToList();

            return Ok(topScorers);
        }
    }
}
