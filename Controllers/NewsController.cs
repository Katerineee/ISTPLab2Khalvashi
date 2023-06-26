using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LAB2ISTPP.Models;

namespace LAB2ISTPP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewsController : ControllerBase
    {
        private readonly NewsContext _context;

        public NewsController(NewsContext context)
        {
            _context = context;
        }

        // GET: api/News
        [HttpGet]
        public async Task<ActionResult<IEnumerable<News>>> GetNews()
        {
          if (_context.News == null)
          {
              return NotFound();
          }
            return await _context.News.ToListAsync();
        }

        // GET: api/News/5
        [HttpGet("{id}")]
        public async Task<ActionResult<News>> GetNews(int id)
        {
          if (_context.News == null)
          {
              return NotFound();
          }
            var news = await _context.News.FindAsync(id);

            if (news == null)
            {
                return NotFound();
            }

            return news;
        }

        // PUT: api/News/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutNews(int id, News updatedNews)
        {
            var news = await _context.News.Include(n => n.Category).FirstOrDefaultAsync(n => n.NewsId == id);

            if (news == null)
            {
                return NotFound();
            }

            news.Title = updatedNews.Title;
            news.Content = updatedNews.Content;
            news.CreatedAt = updatedNews.CreatedAt;

            // Обновление связанной категории, если она указана
            if (updatedNews.Category != null)
            {
                // Проверка существования категории в базе данных
                var category = await _context.Categories.FirstOrDefaultAsync(c => c.Name == updatedNews.Category.Name);
                if (category == null)
                {
                    // Если категория не найдена, создаем новую
                    category = new Category { Name = updatedNews.Category.Name };
                    _context.Categories.Add(category);
                }
                news.Category = category;
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!NewsExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }


        // POST: api/News
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<News>> PostNews(News news)
        {
            if (news.Category != null)
            {
                // Проверяем, существует ли категория в базе данных по имени
                var category = await _context.Categories.FirstOrDefaultAsync(c => c.Name == news.Category.Name);
                if (category == null)
                {
                    // Если категория не найдена, возвращаем ошибку
                    return BadRequest("Указанная категория не существует.");
                }
                news.CategoryId = category.CategoryId; // Присваиваем идентификатор существующей категории
                news.Category = null; // Очищаем связь с объектом категории, чтобы избежать создания новой категории
            }

            // Преобразование даты и времени в UTC
            news.CreatedAt = news.CreatedAt.ToUniversalTime();

            _context.News.Add(news);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetNews", new { id = news.NewsId }, news);
        }

        // DELETE: api/News/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNews(int id)
        {
            if (_context.News == null)
            {
                return NotFound();
            }
            var news = await _context.News.FindAsync(id);
            if (news == null)
            {
                return NotFound();
            }

            _context.News.Remove(news);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool NewsExists(int id)
        {
            return (_context.News?.Any(e => e.NewsId == id)).GetValueOrDefault();
        }
    }
}
