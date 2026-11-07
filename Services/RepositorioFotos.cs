using VisorFotografias.Models;

namespace VisorFotografias.Services;

/// <summary>
/// Conecta la lista doblemente enlazada con el archivo de texto Data/fotos.txt
/// (una linea por fotografia: solo el nombre del archivo). La carpeta de las
/// imagenes es wwwroot/fotos.
/// </summary>
public class RepositorioFotos
{
    public const string RutaFotos = "fotos/";

    private readonly string _archivoTxt;
    private readonly string _carpetaFotos;

    public ListaDoble Lista { get; } = new ListaDoble();

    public RepositorioFotos(IWebHostEnvironment env)
    {
        _archivoTxt = Path.Combine(env.ContentRootPath, "Data", "fotos.txt");
        _carpetaFotos = Path.Combine(env.WebRootPath, "fotos");
        Directory.CreateDirectory(_carpetaFotos);
        Cargar();
    }

    /// <summary>Lee los nombres del txt y los inserta al final de la lista (respeta el orden del archivo).</summary>
    private void Cargar()
    {
        if (!File.Exists(_archivoTxt)) return;

        foreach (string linea in File.ReadAllLines(_archivoTxt))
        {
            string nombre = linea.Trim();
            if (nombre.Length == 0) continue;                 // ignora lineas vacias
            if (Lista.Buscar(nombre) != null) continue;       // ignora repetidas
            Lista.InsertarFinal(new Foto(RutaFotos, nombre));
        }
    }

    /// <summary>Vuelca la lista (de inicio a fin) al txt para que los cambios persistan.</summary>
    public void Guardar()
    {
        File.WriteAllLines(_archivoTxt, Lista.MostrarAdelante().Select(f => f.Nombre));
    }

    /// <summary>Indica si el archivo de imagen existe fisicamente en wwwroot/fotos.</summary>
    public bool ExisteArchivo(string nombre) => File.Exists(Path.Combine(_carpetaFotos, nombre));
}
