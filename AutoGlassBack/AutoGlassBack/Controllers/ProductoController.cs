using AutoGlassBack.DTO;
using AutoGlassBack.Models;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AutoGlassBack.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductoController : ControllerBase
    {
        private readonly AplicationDbContext _context;
        private readonly IMapper mapper;

        public ProductoController(AplicationDbContext context, IMapper mapper)
        {
            _context = context;
            this.mapper = mapper;
        }

        // GET api/<ProductoController>/5
        /// <summary>
        /// Recuperar los registros por codigo producto
        /// </summary>
        /// <param name="CodigoProducto"></param>
        /// <returns></returns>
        [HttpGet("{CodigoProducto}")]
        public async Task<IActionResult> Get(int CodigoProducto)
        {
            try
            {
                var result = _context.Producto.First(id => id.Codigo_producto == CodigoProducto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Lista de los productos registrados
        /// </summary>
        /// <returns></returns>
        // GET: api/<ProductoController>
        [HttpGet]
        public async Task<IActionResult> GetProductos()
        {
            try
            {
                var Lista = _context.Producto.ProjectTo<ProductoDTO>(mapper.ConfigurationProvider).ToListAsync();
                   
                return Ok(Lista.Result.FindAll(r => r.Estado_producto == "Activo"));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //GET: api/<ProductoController>
        //[HttpGet]
        //public async Task<IActionResult> Get()
        //{
        //    try
        //    {
        //        var listaProductos = await _context.Producto.ToListAsync();
        //        return Ok(listaProductos);
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //}

        // POST api/<ProductoController>

        /// <summary>
        /// Insertar producto nuevo
        /// </summary>
        /// <param name="value"></param>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Producto producto)
        {
            try
            {
                var FechaFabrica = DateTime.Parse(producto.Fecha_fabrica.ToString());
                var FechaValida = DateTime.Parse(producto.Fecha_valida.ToString());
                if (FechaFabrica >= FechaValida) return BadRequest(new { mensaje = "Incorrecta la fecha fabricación o fecha vencimiento" });

                _context.Add(producto);
                await _context.SaveChangesAsync();
                return Ok(producto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT api/<ProductoController>/5
        /// <summary>
        /// Editar producto
        /// </summary>
        /// <param name="id"></param>
        /// <param name="producto"></param>
        [HttpPut("{CodigoProducto}")]
        public async Task<IActionResult> Put(int CodigoProducto, [FromBody] Producto producto)
        {
            try
            {
                if (CodigoProducto != producto.Codigo_producto) return NotFound();

                var FechaFabrica = DateTime.Parse(producto.Fecha_fabrica.ToString());
                var FechaValida = DateTime.Parse(producto.Fecha_valida.ToString());
                if ( FechaFabrica >= FechaValida) return BadRequest(new { mensaje = "Incorrecta la fecha fabricación o fecha vencimiento" });

                _context.Update(producto);
                await _context.SaveChangesAsync();

                return Ok(new { mensaje = "Producto actualizado exitosamente" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // DELETE api/<ProductoController>/5
        /// <summary>
        /// Borrar un producto
        /// </summary>
        /// <param name="CodigoProducto"></param>
        /// <returns></returns>
        [HttpDelete("{CodigoProducto}")]
        public async Task<IActionResult> Delete(int CodigoProducto)
        {
            try
            {
                var resut = await _context.Producto.FindAsync(CodigoProducto);
                
                if (resut == null) return NotFound();
                resut.Estado_producto = "Inactivo";
                _context.Update(resut);
                //_context.Remove(resut);
                await _context.SaveChangesAsync();

                return Ok(new { mensaje = "Producto eliminado correctamente" });

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
