using Microsoft.AspNetCore.Mvc;
using VisorFotografias.Models;
using VisorFotografias.Services;

namespace VisorFotografias.Controllers;

public class FotosController : Controller
{
    private readonly RepositorioFotos _repo;

    public FotosController(RepositorioFotos repo)
    {
        _repo = repo;
    }

    private void Aviso(string mensaje, bool error = false)
    {
        TempData["Mensaje"] = mensaje;
        TempData["Error"] = error;
    }

    // GET /Fotos?orden=atras  -> recorrido desde el final; por defecto desde el inicio.
    public IActionResult Index(string? orden)
    {
        bool atras = orden == "atras";
        ViewBag.Atras = atras;
        var fotos = atras ? _repo.Lista.MostrarAtras() : _repo.Lista.MostrarAdelante();
        return View(fotos.ToList());
    }

    // GET /Fotos/Visor?nombre=x -> muestra la foto y los botones Anterior/Siguiente (Prev/Next del nodo).
    public IActionResult Visor(string? nombre)
    {
        Nodo? nodo = string.IsNullOrWhiteSpace(nombre)
            ? _repo.Lista.Primero
            : _repo.Lista.ObtenerNodo(nombre) ?? _repo.Lista.Primero;
        return View(nodo);
    }

    [HttpGet]
    public IActionResult Insertar() => View();

    private static readonly string[] ExtensionesPermitidas = { ".jpg", ".jpeg", ".png", ".gif", ".webp", ".svg" };

    // Recibe el archivo subido (opcional). Si no se sube archivo, se usa el nombre escrito
    // y se asume que la imagen ya esta en wwwroot/fotos.
    [HttpPost]
    public async Task<IActionResult> Insertar(string? nombre, string posicion, IFormFile? archivo)
    {
        if (archivo != null && archivo.Length > 0)
            nombre = Path.GetFileName(archivo.FileName); // GetFileName evita rutas maliciosas (../)

        nombre = (nombre ?? "").Trim();
        if (nombre.Length == 0) { Aviso("Sube una imagen o escribe el nombre del archivo.", true); return View(); }
        if (!ExtensionesPermitidas.Contains(Path.GetExtension(nombre).ToLowerInvariant()))
        { Aviso("Formato no permitido. Usa jpg, png, gif, webp o svg.", true); return View(); }
        if (_repo.Lista.Buscar(nombre) != null) { Aviso("Esa fotografía ya está en la lista.", true); return View(); }

        if (archivo != null && archivo.Length > 0)
            await _repo.GuardarArchivo(nombre, archivo);
        else if (!_repo.ExisteArchivo(nombre))
        { Aviso($"'{nombre}' no existe en wwwroot/fotos. Súbela con el selector de archivo.", true); return View(); }

        var foto = new Foto(RepositorioFotos.RutaFotos, nombre);
        if (posicion == "inicio") _repo.Lista.InsertarInicio(foto);
        else _repo.Lista.InsertarFinal(foto);
        _repo.Guardar();
        Aviso("Fotografía insertada.");
        return RedirectToAction(nameof(Index));
    }

    // GET /Fotos/Buscar?nombre=x
    public IActionResult Buscar(string? nombre)
    {
        ViewBag.Nombre = nombre;
        Foto? encontrada = string.IsNullOrWhiteSpace(nombre) ? null : _repo.Lista.Buscar(nombre.Trim());
        return View(encontrada);
    }

    [HttpGet]
    public IActionResult Modificar(string nombre)
    {
        Foto? foto = _repo.Lista.Buscar(nombre);
        if (foto == null) { Aviso("No se encontró la fotografía.", true); return RedirectToAction(nameof(Index)); }
        return View(foto);
    }

    [HttpPost]
    public IActionResult Modificar(string nombre, string nuevoNombre)
    {
        nuevoNombre = (nuevoNombre ?? "").Trim();
        if (nuevoNombre.Length == 0) { Aviso("El nuevo nombre no puede estar vacío.", true); return RedirectToAction(nameof(Modificar), new { nombre }); }
        if (!string.Equals(nombre, nuevoNombre, StringComparison.OrdinalIgnoreCase) && _repo.Lista.Buscar(nuevoNombre) != null)
        { Aviso("Ya existe una fotografía con ese nombre.", true); return RedirectToAction(nameof(Modificar), new { nombre }); }

        if (_repo.Lista.Modificar(nombre, nuevoNombre)) { _repo.Guardar(); Aviso("Fotografía modificada."); }
        else Aviso("No se encontró la fotografía.", true);
        return RedirectToAction(nameof(Index));
    }

    // posicion: "inicio" | "final" | (vacío) -> eliminar por nombre
    [HttpPost]
    public IActionResult Eliminar(string? nombre, string? posicion)
    {
        if (posicion == "inicio") _repo.Lista.EliminarInicio();
        else if (posicion == "final") _repo.Lista.EliminarFinal();
        else if (!_repo.Lista.Eliminar(nombre ?? "")) { Aviso("No se encontró la fotografía.", true); return RedirectToAction(nameof(Index)); }

        _repo.Guardar();
        Aviso("Fotografía eliminada.");
        return RedirectToAction(nameof(Index));
    }
}
