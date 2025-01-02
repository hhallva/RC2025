using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Servicelayer.Data;
using Servicelayer.Dtos;

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
                    MaterialId = m.MaterialId,
                    Name = m.Name,
                    CreateDate = m.CreateDate,
                    ConfirmDate = m.ConfirmDate,
                    Category = m.Category,
                    HasCommnets = (m.MaterialComments == null) ? true : false,
                }).ToListAsync();
        }

        [HttpGet("Documents/{documentId}/Comments")]
        public async Task<ActionResult<IEnumerable<CommentDto>>> GetCommentsByDocumentIdAsync(int documentId)
        {
            return await _context.MaterialComments
                .Select(c => new CommentDto
                {
                    CommentId = c.CommentId,
                    MaterialId = c.Material.MaterialId,
                    Comment = c.Comment,
                    CreateDate = c.Material.CreateDate,
                    ConfirmDate = c.Material.ConfirmDate,
                    Author = new AuthorCommentDto
                    {
                        AuthorId = c.Employee.EmployeeId,
                        Name = $"{c.Employee.Name} {c.Employee.Surname}",
                        Position = c.Employee.Position.Name
                    }
                }).Where(c => c.MaterialId == documentId).ToListAsync();
        }
    }
}
