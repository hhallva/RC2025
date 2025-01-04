using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServiceLayer.DataContexts;
using ServiceLayer.DTOs;
using ServiceLayer.Models;

namespace WebApi.Controllers
{
    /// <summary>
    /// Контроллер для работы с документами
    /// </summary>
    [Route("api/v1/")]
    [ApiController]
    [Authorize]
    public class DocumentsController(AppDbContext context) : ControllerBase
    {
        /// <summary>
        /// GET: /api/v1/Documents
        /// Получение списка всех документов
        /// </summary>
        /// <returns>Список документов</returns>
        /// <response code="200">Успешное получение списка</response>
        /// <response code="404">Документы не найдены</response>
        [HttpGet("Documents")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DocumentDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiErrorDto))]
        public async Task<ActionResult<IEnumerable<DocumentDto>>> GetDocumentsAsync()
        {
            var documents = await context.Documents
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
        /// </summary>
        /// <param name="id">Id документа, у которого хотим получить комментарии</param>
        /// <returns>Список комментариев</returns>
        /// <response code="200">Успешное получение списка комментариев</response>
        /// <response code="400">Неверный параметр</response>
        /// <response code="404">Не найдены комментарии для документа</response>
        [HttpGet("Document/{id:int}/Comments")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CommentDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiErrorDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiErrorDto))]
        public async Task<ActionResult<IEnumerable<CommentDto>>> GetDocumentCommentsAsync(int id)
        {
            if (id <= 0)
                return BadRequest(new ApiErrorDto("Недопустимый идентификатор документа. ID должен быть положительным числом.", 2000));

            var comments = await context.Comments
                .Include(c => c.Employee)
                .ThenInclude(e => e.Position)
                .Where(c => c.DocumentId == id)
                .ToListAsync();

            if (!comments.Any())
                return NotFound(new ApiErrorDto("Не найдены комментарии для документа", 2001));

            var commentDtos = comments.Select(c => c.ToDto()).ToList();
            return commentDtos;
        }

        /// <summary>
        /// POST: /api/v1/Document/{id}/Comments
        /// Создание комментария для конктерного документа
        /// </summary>
        /// <param name="id">Id документа, которому хотим оставить комментарий</param>
        /// <param name="commentDto">Комментарий</param>
        /// <returns>Созданный комментарий</returns>
        /// <response code="201">Успешное создание комментария</response>
        /// <response code="400">Неправильные параметры</response>       
        [HttpPost("Document/{id}/Comments")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(CommentDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiErrorDto))]
        public async Task<ActionResult<CommentDto>> PostCommentAsync(int id, CommentDto commentDto)
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

                context.Comments.Add(comment);
                await context.SaveChangesAsync();

                await context.Comments
                    .Include(c => c.Employee)
                    .ThenInclude(e => e.Position)
                    .Where(c => c.DocumentId == id)
                    .ToListAsync();

                commentDto = comment.ToDto();

                return Created("", commentDto);
            }
            catch
            {
                return BadRequest(new ApiErrorDto("Ошибка при сохранении комментария", 2002));
            }
        }
    }
}
