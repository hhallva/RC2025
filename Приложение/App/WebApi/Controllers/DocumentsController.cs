using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServiceLayer.Data;
using ServiceLayer.DTOs;
using ServiceLayer.Models;
using WebApi.DTOs;

namespace WebApi.Controllers
{
    [Route("api/v1/")]
    [ApiController]
    [Authorize]
    public class DocumentsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public DocumentsController(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// GET: /api/v1/Documents
        /// Получение списка всех документов
        /// </summary>
        /// <returns>Список объектов DocumentDto</returns>
        /// <response code="200">Успешное получение списка</response>
        /// <response code="404">Документы не найдены</response>
        [HttpGet("Documents")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DocumentDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiErrorDto))]
        public async Task<ActionResult<IEnumerable<DocumentDto>>> GetDocumentsAsync()
        {
            var documents = await _context.Documents
                .Select(m => new DocumentDto
                {
                    Id = m.DocumentId,
                    Title = m.Name,
                    CreatedDate = m.CreateDate,
                    ApprovedDate = m.ConfirmDate,
                    Category = m.Category,
                    HasComments = m.Comments.Count != 0
                }).ToListAsync();

            if (!documents.Any())
                return NotFound(new ApiErrorDto("Документы не найдены", 1001));
            return documents;
        }

        /// <summary>
        /// GET: /api/v1/Document/{id}/Comments
        /// Получение списка комментариев у конктерного документа
        /// </summary>\
        /// <param name="id">Id документа, у которого хотим получить комментарии</param>
        /// <returns>Список комментариев типа CommentDto</returns>
        /// <response code="200">Успешное получение списка</response>
        /// <response code="400">Неправильный параметр</response>
        /// <response code="404">Не найдены комментарии для документа</response>
        [HttpGet("Document/{id}/Comments")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CommentDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiErrorDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiErrorDto))]
        public async Task<ActionResult<IEnumerable<CommentDto>>> GetDocumentComments(int id)
        {
            if (id <= 0)
                return BadRequest(new ApiErrorDto("Недопустимый идентификатор документа. ID должен быть положительным числом.", 2000));

            var comments = await _context.Comments
            .Include(c => c.Employee)
            .ThenInclude(e => e.Position)
            .Where(c => c.DocumentId == id)
            .ToListAsync();

            var commentDtos = comments.Select(c => c.ToDto()).ToList();

            if (!commentDtos.Any())
                return NotFound(new ApiErrorDto("Не найдены комментарии для документа", 2001));
            return Ok(commentDtos);
        }

        /// <summary>
        /// POST: /api/v1/Document/{id}/Comments
        /// Создание комментария для конктерного документа
        /// </summary>
        /// <param name="id">Id документа, которому хотим оставить комменатрий</param>
        /// <param name="commentDto">Данные о комменатрие</param>
        /// <returns>Статус создания и id созданного комментария</returns>
        /// <response code="200">Успешное создание комментария</response>
        /// <response code="400">Неправильные параметры</response>       
        [HttpPost("Document/{id}/Comments")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CommentDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiErrorDto))]
        public async Task<ActionResult<CommentDto>> PostComment(int id, CommentDto commentDto)
        {
            try
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

                return Created("", commentDto);
            }
            catch (DbUpdateException)
            {
                return BadRequest(new ApiErrorDto("Ошибка при сохранении комментария", 2002));
            }
        }
    }
}
