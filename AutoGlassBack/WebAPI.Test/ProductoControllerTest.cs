using AutoGlassBack;
using AutoGlassBack.Controllers;
using AutoGlassBack.Models;
using AutoGlassBack.Utilidades;
using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace WebAPI.Test
{
    public class ProductoControllerTest
    {
        protected AplicationDbContext Conexion(string nombDb)
        {
            var opciones = new DbContextOptionsBuilder<AplicationDbContext>()
                           .UseInMemoryDatabase(nombDb).Options;

            var dbContext = new AplicationDbContext(opciones);

            return dbContext;
        }

        protected IMapper ConfigurationAutoMapper()
        {
            var config = new MapperConfiguration(options =>
            {
                var geometryFactory = NtsGeometryServices.Instance.CreateGeometryFactory(srid: 4326);
                options.AddProfile(new AutoMapperProfiles());
            });

            return config.CreateMapper();
        }

        [Fact]
        public async Task GetProductosActivos()
        {
            //Preparacion
            var nombreBd = Guid.NewGuid().ToString();
            var contexto = Conexion(nombreBd);
            var mapper = ConfigurationAutoMapper();

            contexto.Producto.Add(new Producto()
            {
                Codigo_producto = 1,
                Descripcion_producto = "parabrisa trasero",
                Estado_producto = "Activo",
                Fecha_fabrica = DateTime.Now,
                Fecha_valida = DateTime.Now.AddMonths(1),
                Codigo_proveedor = 1,
                Descripcion_proveedor = "Induservice",
                Telefono_proveedor = "3102228738"
            });
            contexto.Producto.Add(new Producto()
            {
                Codigo_producto = 2,
                Descripcion_producto = "Vidrio lateral",
                Estado_producto = "Inactivo",
                Fecha_fabrica = DateTime.Now,
                Fecha_valida = DateTime.Now.AddDays(1),
                Codigo_proveedor = 2,
                Descripcion_proveedor = "Autoglass",
                Telefono_proveedor = "456789"
            });
            contexto.Producto.Add(new Producto()
            {
                Codigo_producto = 3,
                Descripcion_producto = "parabrisa delantero",
                Estado_producto = "Activo",
                Fecha_fabrica = DateTime.Now,
                Fecha_valida = DateTime.Now.AddDays(1),
                Codigo_proveedor = 3,
                Descripcion_proveedor = "Autoglass",
                Telefono_proveedor = "456789"
            });

            await contexto.SaveChangesAsync();

            var contexto2 = Conexion(nombreBd);

            //Prueba
            var controller = new ProductoController(contexto2, mapper);
            var respuesta = await controller.GetProductos();

            //Verificacion
            respuesta.Should().NotBeNull();
            respuesta.GetType().Name.Should().Be("OkObjectResult");
            ObjectResult objectResult = (ObjectResult)respuesta;
            objectResult.StatusCode.Should().Be(200);
        }

        [Fact]
        public async Task GetProductoById()
        {
            //preparacion
            var nombreBd = Guid.NewGuid().ToString();
            var contexto = Conexion(nombreBd);
            var mapper = ConfigurationAutoMapper();

            contexto.Producto.Add(new Producto()
            {
                Codigo_producto = 1,
                Descripcion_producto = "parabrisa trasero",
                Estado_producto = "Activo",
                Fecha_fabrica = DateTime.Now,
                Fecha_valida = DateTime.Now.AddMonths(1),
                Codigo_proveedor = 1,
                Descripcion_proveedor = "Induservice",
                Telefono_proveedor = "3102228738"
            });
            contexto.Producto.Add(new Producto()
            {
                Codigo_producto = 2,
                Descripcion_producto = "Vidrio lateral",
                Estado_producto = "Inactivo",
                Fecha_fabrica = DateTime.Now,
                Fecha_valida = DateTime.Now.AddDays(1),
                Codigo_proveedor = 2,
                Descripcion_proveedor = "Autoglass",
                Telefono_proveedor = "456789"
            });
            contexto.Producto.Add(new Producto()
            {
                Codigo_producto = 3,
                Descripcion_producto = "parabrisa delantero",
                Estado_producto = "Activo",
                Fecha_fabrica = DateTime.Now,
                Fecha_valida = DateTime.Now.AddDays(1),
                Codigo_proveedor = 3,
                Descripcion_proveedor = "Autoglass",
                Telefono_proveedor = "456789"
            });

            await contexto.SaveChangesAsync();

            var contexto2 = Conexion(nombreBd);

            //Prueba
            var controller = new ProductoController(contexto2, mapper);

            var respuesta = await controller.GetCodProducto(1);

            //Verificacion
            Assert.NotNull(respuesta);
            ObjectResult objectResult = (ObjectResult)respuesta;
            objectResult.Value.Should().NotBeNull();
            objectResult.StatusCode.Should().Be(200);
        }

        [Fact]
        public async Task InsertarProducto()
        {
            //preparacion
            var nombreBd = Guid.NewGuid().ToString();
            var mapper = ConfigurationAutoMapper();

            var producto = new Producto()
            {
                Codigo_producto = 4,
                Descripcion_producto = "sunroof",
                Estado_producto = "Activo",
                Fecha_fabrica = DateTime.Now,
                Fecha_valida = DateTime.Now.AddMonths(10),
                Codigo_proveedor = 1,
                Descripcion_proveedor = "Induservice",
                Telefono_proveedor = "3102228738"
            };

            var contexto = Conexion(nombreBd);

            //Prueba
            var controller = new ProductoController(contexto, mapper);
            var respuesta = await controller.InsertarProducto(producto);

            //Verificacion
            Assert.NotNull(respuesta);
            ObjectResult objectResult = (ObjectResult)respuesta;
            objectResult.StatusCode.Should().Be(200);
        }

        [Fact]
        public async Task Insert_Producto_ValFecha()
        {
            //preparacion
            var nombreBd = Guid.NewGuid().ToString();
            var contexto = Conexion(nombreBd);
            var mapper = ConfigurationAutoMapper();

            var producto = new Producto()
            {
                Codigo_producto = 1,
                Descripcion_producto = "parabrisa trasero",
                Estado_producto = "Activo",
                Fecha_fabrica = DateTime.Now.AddDays(1),
                Fecha_valida = DateTime.Now,
                Codigo_proveedor = 1,
                Descripcion_proveedor = "Induservice",
                Telefono_proveedor = "3102228738"
            };
            contexto.Producto.Add(producto);

            await contexto.SaveChangesAsync();

            var contexto2 = Conexion(nombreBd);

            //Prueba
            var controller = new ProductoController(contexto2, mapper);
            var respuesta = await controller.InsertarProducto(producto);

            //Verificacion
            Assert.NotNull(respuesta);
            ObjectResult objectResult = (ObjectResult)respuesta;
            var rtaMsj = JsonConvert.DeserializeObject<Dictionary<string, object>>(objectResult.Value.ToString());
            rtaMsj["mensaje"].Should().Be("Incorrecta la fecha fabricación o fecha vencimiento");
            objectResult.StatusCode.Should().Be(400);
        }

        [Fact]
        public async Task EditarProducto()
        {
            //preparacion
            var nombreBd = Guid.NewGuid().ToString();
            var contexto = Conexion(nombreBd);
            var mapper = ConfigurationAutoMapper();

            var producto = new Producto()
            {
                Codigo_producto = 4,
                Descripcion_producto = "sunroof",
                Estado_producto = "Activo",
                Fecha_fabrica = DateTime.Now,
                Fecha_valida = DateTime.Now.AddMonths(5),
                Codigo_proveedor = 1,
                Descripcion_proveedor = "Autoglass",
                Telefono_proveedor = "78965432"
            };
            contexto.Producto.Add(producto);

            await contexto.SaveChangesAsync();

            var contexto2 = Conexion(nombreBd);

            //Prueba
            var controller = new ProductoController(contexto2, mapper);
            var respuesta = await controller.EditarProductoByCodigo(4, producto);

            //Verificacion
            Assert.NotNull(respuesta);
            ObjectResult objectResult = (ObjectResult)respuesta;
            var rtaMsj = JsonConvert.DeserializeObject<Dictionary<string, object>>(objectResult.Value.ToString());
            rtaMsj["mensaje"].Should().Be("Producto actualizado exitosamente");
            objectResult.StatusCode.Should().Be(200);
        }

        [Fact]
        public async Task EditarProductoValFecha()
        {
            //preparacion
            var nombreBd = Guid.NewGuid().ToString();
            var contexto = Conexion(nombreBd);
            var mapper = ConfigurationAutoMapper();

            var producto = new Producto()
            {
                Codigo_producto = 4,
                Descripcion_producto = "sunroof",
                Estado_producto = "Activo",
                Fecha_fabrica = DateTime.Now,
                Fecha_valida = DateTime.Now,
                Codigo_proveedor = 1,
                Descripcion_proveedor = "Autoglass",
                Telefono_proveedor = "78965432"
            };
            contexto.Producto.Add(producto);

            await contexto.SaveChangesAsync();

            var contexto2 = Conexion(nombreBd);

            //Prueba
            var controller = new ProductoController(contexto2, mapper);
            var respuesta = await controller.EditarProductoByCodigo(4, producto);

            //Verificacion
            Assert.NotNull(respuesta);

            ObjectResult objectResult = (ObjectResult)respuesta;
            var rtaMsj = JsonConvert.DeserializeObject<Dictionary<string, object>>(objectResult.Value.ToString());
            rtaMsj["mensaje"].Should().Be("Incorrecta la fecha fabricación o fecha vencimiento");
            objectResult.StatusCode.Should().Be(400);
        }

        [Fact]
        public async Task EliminarActualizarEstadoProducto()
        {
            //preparacion
            var nombreBd = Guid.NewGuid().ToString();
            var contexto = Conexion(nombreBd);
            var mapper = ConfigurationAutoMapper();

            var producto = new Producto()
            {
                Codigo_producto = 5,
                Descripcion_producto = "sunroof",
                Estado_producto = "Activo",
                Fecha_fabrica = DateTime.Now,
                Fecha_valida = DateTime.Now.AddMonths(5),
                Codigo_proveedor = 1,
                Descripcion_proveedor = "Autoglass",
                Telefono_proveedor = "78965432"
            };
            contexto.Producto.Add(producto);

            await contexto.SaveChangesAsync();

            var contexto2 = Conexion(nombreBd);

            //Prueba
            var controller = new ProductoController(contexto2, mapper);
            var respuesta = await controller.BorrarProducto(5);

            //Verificacion
            Assert.NotNull(respuesta);
            ObjectResult objectResult = (ObjectResult)respuesta;
            var rtaMsj = JsonConvert.DeserializeObject<Dictionary<string, object>>(objectResult.Value.ToString());
            rtaMsj["mensaje"].Should().Be("Producto eliminado correctamente");
            //objectResult.StatusCode.Should().Be(200);
        }
    }
}