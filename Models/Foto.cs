namespace VisorFotografias.Models;

/// <summary>
/// Dato que guarda cada nodo de la lista: la direccion (ruta relativa a la
/// carpeta de fotografias) y el nombre del archivo de la imagen.
/// </summary>
public class Foto
{
    /// <summary>Direccion donde se encuentra el archivo, ej. "fotos/".</summary>
    public string Ruta { get; set; }

    /// <summary>Nombre del archivo con extension, ej. "playa.svg".</summary>
    public string Nombre { get; set; }

    public Foto(string ruta, string nombre)
    {
        Ruta = ruta;
        Nombre = nombre;
    }

    /// <summary>URL desde la que el navegador puede cargar la imagen.</summary>
    public string Url => "/" + Ruta.Trim('/') + "/" + Nombre;
}
