using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CRUDAPI.Models;
using CRUDAPI.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CRUDAPI.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriaController : ControllerBase
    {
        private readonly Contexto _contexto;
        private readonly CategoriaService _categoriaService;

        public CategoriaController(Contexto contexto, CategoriaService categoriaService)
        {
            _contexto = contexto;
            _categoriaService = categoriaService;
        }

        // GET: api/Categoria
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Categoria>>> GetCategorias()
        {
            return await _contexto.Categorias.ToListAsync();
        }

        // GET: api/Categoria/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Categoria>> GetCategoria(long id)
        {
            var categoria = await _contexto.Categorias.FindAsync(id);

            if (categoria == null)
            {
                return NotFound();
            }

            return categoria;
        }

        // PUT: api/Categoria/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCategoria(long id, Categoria categoria)
        {
            if (id != categoria.Id)
            {
                return BadRequest();
            }

            _contexto.Entry(categoria).State = EntityState.Modified;

            try
            {
                categoria = await _categoriaService.ValidarCategoria(categoria);
                await _contexto.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_categoriaService.CategoriaExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return NoContent();
        }

        // POST: api/Categoria
        [HttpPost]
        public async Task<ActionResult<Categoria>> PostCategoria(Categoria categoria)
        {
            try
            {
                categoria = await _categoriaService.ValidarCategoria(categoria);

                _contexto.Categorias.Add(categoria);
                await _contexto.SaveChangesAsync();

                return CreatedAtAction(nameof(GetCategoria), new { id = categoria.Id }, categoria);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // DELETE: api/Categoria/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategoria(long id)
        {
            var categoria = await _contexto.Categorias.FindAsync(id);
            if (categoria == null)
            {
                return NotFound();
            }

            _contexto.Categorias.Remove(categoria);
            await _contexto.SaveChangesAsync();

            return NoContent();
        }
    }
}
