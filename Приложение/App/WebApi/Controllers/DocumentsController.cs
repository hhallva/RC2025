using DataLayer.DataContexts;
using DataLayer.DTOs;
using DataLayer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;

namespace WebApi.Controllers
{
    /// <summary>
    /// Контроллер для работы с документами
    /// </summary>
    [Route("api/v1/")]
    [ApiController]
    //[Authorize]
    public class DocumentsController(AppDbContext context) : ControllerBase
    {
        [HttpGet("Documents")]
        [SwaggerOperation(
            Summary = "Получение списка документов ",
            Description = "Метод для получения списка документов из БД.")]
        [SwaggerResponse(StatusCodes.Status200OK, "Успешное получение списка. Возврат списка документов.", Type = typeof(IEnumerable<DocumentDto>))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Документы не найдены. Возврат сообщения об ошибке.", Type = typeof(ApiErrorDto))]
        public async Task<ActionResult<IEnumerable<DocumentDto>>> GetDocumentsAsync()
        {
            var documents = await context.Documents
                .Select(m => new DocumentDto
                {
                    Id = m.DocumentId,
                    Title = m.Name,
                    CreatedDate = m.CreatedDate,
                    ApprovedDate = m.ApprovedDate,
                    Category = m.Category,
                    HasComments = m.Comments.Count != 0
                }).ToListAsync();

            if (!documents.Any())
                return NotFound(new ApiErrorDto("Документы не найдены", 1001));
            return documents;
        }

        [HttpGet("Document/{id}/Comments")]
        [SwaggerOperation(
            Summary = "Получение списка комментариев ",
            Description = "Метод для получения списка комментариев относящихся к документу из БД.")]
        [SwaggerResponse(StatusCodes.Status200OK, "Успешное получение списка. Возврат списка комментариев.", Type = typeof(IEnumerable<CommentDto>))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Комментарии не найдены. Возврат сообщения об ошибке.", Type = typeof(ApiErrorDto))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Неверные параметры. Возврат сообщения об ошибке.", Type = typeof(ApiErrorDto))]
        public async Task<ActionResult<IEnumerable<CommentDto>>> GetDocumentCommentsAsync(
            [SwaggerParameter("Id документа у которого хотим получить комментарии", Required = true)] int id)
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

        [HttpPost("Document/{id}/Comments")]
        [SwaggerOperation(
             Summary = "Создание комментария",
             Description = "Метод для создания комментария к документу из БД, принимает Id документа и данные комментария, возвращает созданный комментарий")]
        [SwaggerResponse(StatusCodes.Status200OK, "Успешное создание комментария. Возврат созданного комментария.", Type = typeof(CommentDto))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Неверные параметры. Возврат сообщения об ошибке.", Type = typeof(ApiErrorDto))]
        public async Task<ActionResult<CommentDto>> PostCommentAsync(
             [SwaggerParameter("Id документа у которого хотим оставить комментарий", Required = true)] int id,
             [SwaggerRequestBody("Данные комментария", Required = true)] CommentDto commentDto)
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
