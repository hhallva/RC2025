using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DataLayer.DataContexts;
using DataLayer.Models;

namespace WebApi.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class PositionsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public PositionsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/v1/Positions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Position>>> GetPositionsAsync() 
            => await _context.Positions.ToListAsync();
    }
}
