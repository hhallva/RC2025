using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServiceLayer.Data;
using ServiceLayer.Dtos;
using ServiceLayer.DTOs;
using ServiceLayer.Models;

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
        public async Task<ActionResult<IEnumerable<DocumentDto>>> GetDocumentsAsync()
        {
            return await _context.Documents
                .Select(m => new MaterialDto
                .Select(m => new DocumentDto
                {
                    Id = m.DocumentId,
                    Title = m.Name,
                    CreateDate = m.CreateDate,
                    ConfirmDate = m.ConfirmDate,
                    CreatedDate = m.CreateDate,
                    ApprovedDate = m.ConfirmDate,
                    Category = m.Category,
                    HasComments = m.Comments.Count != 0
                }).ToListAsync();
        }

        [HttpGet("Document/{id}/Comments")]
        public async Task<ActionResult<IEnumerable<CommentDto>>> GetDocumentComments(int id)
        {
            var comments = await _context.Comments
                .Include(c => c.Employee)
                .ThenInclude(e => e.Position)
                .Where(c => c.DocumentId == id)
                .ToListAsync();
            
            var commentDtos = comments.Select(c => c.ToDto()).ToList();

            return Ok(commentDtos);
        }

        [HttpPost("Document/{id}/Comments")]
        public async Task<ActionResult<CommentDto>> PostComment(int id, CommentDto commentDto)
        {
            var comment = new Comment
            {
                DocumentId = id,
                Text = commentDto.Text,
                CreatedDate = DateTime.Now,
                UpdatedDate = null,
                EmployeeId = commentDto.Author.Id
            };

            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();

            await _context.Comments
                .Include(c => c.Employee)
                .ThenInclude(e => e.Position)
                .Where(c => c.DocumentId == id)
                .ToListAsync();

            commentDto = comment.ToDto();

            return CreatedAtAction("GetComment", new { id = comment.CommentId }, commentDto);
        }

        [HttpGet("Comments/{id}")]
        public async Task<ActionResult<CommentDto>> GetComment(int id)
        {
            var comment = await _context.Comments
                .Include(c => c.Employee)
                .ThenInclude(e => e.Position)
                .FirstOrDefaultAsync(c => c.CommentId == id);

            return Ok(comment?.ToDto());
            return Created("", commentDto);
        }
    }
}
