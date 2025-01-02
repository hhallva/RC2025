using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Servicelayer.Data;
using Servicelayer.Dtos;
using Servicelayer.Models;

namespace WebApi.Controllers
{
    [Route("api/v1/")]
    [ApiController]
    public class DocumentsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public DocumentsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("Documents")]
        public async Task<ActionResult<IEnumerable<MaterialDto>>> GetDocumentsAsync()
        {
            return await _context.Materials
                .Select(m => new MaterialDto
                {
                    Id = m.MaterialId,
                    Title = m.Name,
                    CreateDate = m.CreateDate,
                    ConfirmDate = m.ConfirmDate,
                    Category = m.Category,
                    HasCommnet = m.Comments.Count != 0
                }).ToListAsync();
        }

        [HttpGet("Document/{id}/Comments")]
        public async Task<ActionResult<IEnumerable<CommentDto>>> GetDocumentComments(int id)
        {
            var comments = await _context.MaterialComments
                .Include(c => c.Employee)
                .ThenInclude(e => e.Position)
                .Where(c => c.MaterialId == id)
                .ToListAsync();
            
            var commentDtos = comments.Select(c => c.ToDto()).ToList();

            return Ok(commentDtos);
        }

        [HttpPost("Document/{id}/Comments")]
        public async Task<ActionResult<CommentDto>> PostComment(int id, CommentDto commentDto)
        {
            var comment = new MaterialComment
            {
                MaterialId = id,
                Comment = commentDto.Text,
                EmployeeId = commentDto.Author.Id
            };

            _context.MaterialComments.Add(comment);
            await _context.SaveChangesAsync();

            await _context.MaterialComments
                .Include(c => c.Employee)
                .ThenInclude(e => e.Position)
                .Where(c => c.MaterialId == id)
                .ToListAsync();

            commentDto = comment.ToDto();

            return CreatedAtAction("GetComment", new { id = comment.CommentId }, commentDto);
        }

        [HttpGet("Comments/{id}")]
        public async Task<ActionResult<CommentDto>> GetComment(int id)
        {
            var comment = await _context.MaterialComments
                .Include(c => c.Employee)
                .ThenInclude(e => e.Position)
                .FirstOrDefaultAsync(c => c.CommentId == id);

            return Ok(comment?.ToDto());
        }
    }
}
