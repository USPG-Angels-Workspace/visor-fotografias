// Punto de entrada de la aplicacion (top-level statements).

using VisorFotografias.Services;

var builder = WebApplication.CreateBuilder(args);

// Habilita el patron MVC (Controllers + Views con Razor).
builder.Services.AddControllersWithViews();

// La lista doblemente enlazada vive en memoria mientras corre la app, por eso
// el repositorio se registra como Singleton (una sola instancia compartida).
builder.Services.AddSingleton<RepositorioFotos>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

app.UseStaticFiles(); // Sirve wwwroot/ (incluye las fotografias en wwwroot/fotos).

app.UseRouting();

// Ruta por defecto: /Controlador/Accion/id. Sin nada en la URL cae en Fotos/Index.
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Fotos}/{action=Index}/{id?}");

app.Run();
