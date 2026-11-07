namespace VisorFotografias.Models;

/// <summary>
/// Nodo de la lista doblemente enlazada: guarda un dato y dos referencias,
/// una al nodo anterior (Prev) y otra al siguiente (Next).
/// </summary>
public class Nodo
{
    public Foto Dato;
    public Nodo? Prev;
    public Nodo? Next;

    public Nodo(Foto dato)
    {
        Dato = dato;
        Prev = null;
        Next = null;
    }
}
