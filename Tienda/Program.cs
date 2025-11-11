using Microsoft.EntityFrameworkCore;
using Tienda.Models;
using Tienda.Repositories;
using Tienda.Services;

var builder = WebApplication.CreateBuilder(args);

// ✅ Agrega los servicios ANTES de Build()

// MVC
builder.Services.AddControllersWithViews();

// ✅ Conexión a la base de datos
builder.Services.AddDbContext<TiendadbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        new MySqlServerVersion(new Version(8, 0, 29))
    ));

// ✅ Inyección de dependencias (Repositorios y Servicios)
builder.Services.AddScoped<ICategoriaRepository, CategoriaRepository>();
builder.Services.AddScoped<ICategoriaService, CategoriaService>();
builder.Services.AddScoped<IProductoRepository, ProductoRepository>();
builder.Services.AddScoped<IProductoService, ProductoService>();



// ✅ Ahora sí, construimos la app
var app = builder.Build();

//Permite Usar  Imagen
app.UseStaticFiles();

// Pipeline HTTP
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

app.UseRouting();

app.UseAuthorization();

// Rutas
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Categoria}/{action=Index}/{id?}"
);

app.Run();
