using Microsoft.AspNetCore.Mvc;
using ServiceLayer.Data;
using ServiceLayer.DTOs;
using ServiceLayer.Services;
using WebApi.DTOs;

namespace WebApi.Controllers
{
    /// <summary>
    /// Контроллтер для авторизации
    /// </summary>
    [Route("api/v1/")]
    [ApiController]
    public class AccountController(TokenService tokenService, AppDbContext context) : ControllerBase
    {
        /// <summary>
        /// POST: /api/v1/SignIn
        /// Вход в учетную запись, генерация JWT-токена
        /// </summary>
        /// <param name="employee">Объект содержащий email и пароль пользователя</param>
        /// <returns>JWT-токен</returns>
        /// <response code="200">Успешная авторизация</response>
        /// <response code="400">Неверные параметры</response>
        /// <response code="403">Доступ запрещён</response>
        /// <response code="404">Пользователь не найден</response>
        [HttpPost("SignIn")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiErrorDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiErrorDto))]
        [ProducesResponseType(StatusCodes.Status403Forbidden, Type = typeof(ApiErrorDto))]
        public IActionResult Login(LoginDto employee)
        {
            if (string.IsNullOrWhiteSpace(employee.Email))
                return BadRequest(new ApiErrorDto("Почта не указана", 3000));
            if (string.IsNullOrWhiteSpace(employee.Password))
                return BadRequest(new ApiErrorDto("Пароль не указан", 3001));

            var dbEmployee = context.Employees.FirstOrDefault(u => u.Email == employee.Email);
            if (dbEmployee is null)
                return NotFound(new ApiErrorDto("Пользователь не найден", 3002));
            if (dbEmployee.Password != employee.Password)
                return StatusCode(403, new ApiErrorDto("Доступ запрещён", 3003));
            return Ok(tokenService.GenerateToken(dbEmployee));
        }
    }
}
