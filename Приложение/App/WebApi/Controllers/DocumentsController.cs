using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Servicelayer.Data;
using Servicelayer.DTOs;

namespace WebApi.Controllers
{
    [Route("api/v1/Documents")]
    [Route("api/v1/")]
    [ApiController]
    public class DocumentsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public DocumentsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [HttpGet("Documents")]
        public async Task<ActionResult<IEnumerable<MaterialDTO>>> GetMaterials()
        {
            try
            {
                return await _context.Materials
                    .Select(m => new MaterialDTO
                    {
                        MaterialId = m.MaterialId,
                        Name = m.Name,
                        CreateDate = m.CreateDate,
                        ConfirmDate = m.ConfirmDate,
                        Category = m.Category,
                        HasCommnets = (m.MaterialComments == null) ? true : false,
                    }).ToListAsync();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Внутренняя ошибка сервера: " + ex.Message);
            }
        }

        [HttpGet("Documents/{documentId}/Comments")]
        public async Task<ActionResult<IEnumerable<CommentDTO>>> GetMaterialsComments(int documentId)
        {
            return await _context.MaterialComments
                .Select(o => new CommentDTO
                {
                    CommentId = o.CommentId,
                    MaterialId = o.Material.MaterialId,
                    Comment = o.Comment,
                    CreateDate = o.Material.CreateDate,
                    ConfirmDate = o.Material.ConfirmDate,
                    Author = new AuthorCommentDTO
                    {
                        AuthorId = o.Employee.EmployeeId,
                        Name = $"{o.Employee.Name} {o.Employee.Surname}",
                        Position = o.Employee.Position.Name
                    }
                }).Where(c => c.MaterialId == documentId).ToListAsync();
        }
        //// GET: api/Materials/5
        //[HttpGet("{id}")]
        //public async Task<ActionResult<Material>> GetMaterial(int id)
        //{
        //    var material = await _context.Materials.FindAsync(id);

        //    if (material == null)
        //    {
        //        return NotFound();
        //    }

        //    return material;
        //}

        //// PUT: api/Materials/5
        //// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutMaterial(int id, Material material)
        //{
        //    if (id != material.MaterialId)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(material).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!MaterialExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return NoContent();
        //}

        //// POST: api/Materials
        //// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPost]
        //public async Task<ActionResult<Material>> PostMaterial(Material material)
        //{
        //    _context.Materials.Add(material);
        //    await _context.SaveChangesAsync();

        //    return CreatedAtAction("GetMaterial", new { id = material.MaterialId }, material);
        //}

        //// DELETE: api/Materials/5
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteMaterial(int id)
        //{
        //    var material = await _context.Materials.FindAsync(id);
        //    if (material == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.Materials.Remove(material);
        //    await _context.SaveChangesAsync();

        //    return NoContent();
        //}

        //private bool MaterialExists(int id)
        //{
        //    return _context.Materials.Any(e => e.MaterialId == id);
        //}
    }
}
