using DataLayer.DataContexts;
using DataLayer.DTOs;
using DataLayer.Services;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace WebApi.Controllers
{
    /// <summary>
    /// Контроллтер для авторизации
    /// </summary>
    [Route("api/v1/")]
    [ApiController]
    public class AccountController(TokenService service, AppDbContext context) : ControllerBase
    {
        [HttpPost("SignIn")]
        [SwaggerOperation(
            Summary = "Получение JWT-токена",
            Description = "Метод для генерации JWT-токена, принимает учетные данные пользователя, при успешной авторизации возварщает JWT-токен.")]
        [SwaggerResponse(StatusCodes.Status201Created, "Успешная авторизация. Возврат JWT-токена.", Type = typeof(string))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Пользователь не найден. Возврат сообщения об ошибке.", Type = typeof(ApiErrorDto))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Неверные параметры. Возврат сообщения об ошибке.", Type = typeof(ApiErrorDto))]
        [SwaggerResponse(StatusCodes.Status403Forbidden, "Доступ запрещен. Возврат сообщения об ошибке.", Type = typeof(ApiErrorDto))]
        public IActionResult Login(
            [SwaggerRequestBody("Учетные данные пользователя", Required = true)] LoginDto employee)
        {
            if (string.IsNullOrWhiteSpace(employee.Email))
                return BadRequest(new ApiErrorDto("Почта не указана", 1000));
            if (string.IsNullOrWhiteSpace(employee.Password))
                return BadRequest(new ApiErrorDto("Пароль не указан", 1001));

            var dbEmployee = context.Employees.FirstOrDefault(u => u.Email == employee.Email);
            if (dbEmployee == null)
                return NotFound(new ApiErrorDto("Пользователь не найден", 1002));
            if (dbEmployee.Password != employee.Password)
                return StatusCode(403, new ApiErrorDto("Доступ запрещён", 1003));
            return Ok(service.GenerateToken(dbEmployee));
        }
    }
}
