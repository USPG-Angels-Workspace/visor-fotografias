namespace VisorFotografias.Models;

/// <summary>
/// Lista dinamica doblemente enlazada de fotografias.
/// Head apunta al primer nodo y Tail al ultimo; si ambos son null la lista esta vacia.
/// Cada operacion actualiza las referencias Prev y Next en ambos sentidos.
/// </summary>
public class ListaDoble
{
    private Nodo? Head;
    private Nodo? Tail;

    /// <summary>Cantidad de nodos actualmente en la lista.</summary>
    public int Count { get; private set; }

    public ListaDoble()
    {
        Head = null;
        Tail = null;
    }

    /// <summary>Primer nodo (o null si la lista esta vacia).</summary>
    public Nodo? Primero => Head;

    /// <summary>Ultimo nodo (o null si la lista esta vacia).</summary>
    public Nodo? Ultimo => Tail;

    // ---------------------------------------------------------------- Insertar

    public void InsertarFinal(Foto dato)
    {
        Nodo nuevo = new Nodo(dato);

        if (Head == null)
            Head = Tail = nuevo;
        else
        {
            nuevo.Prev = Tail;
            Tail!.Next = nuevo;
            Tail = nuevo;
        }
        Count++;
    }

    public void InsertarInicio(Foto dato)
    {
        Nodo nuevo = new Nodo(dato);

        if (Head == null)
            Head = Tail = nuevo;
        else
        {
            nuevo.Next = Head;
            Head.Prev = nuevo;
            Head = nuevo;
        }
        Count++;
    }

    // ---------------------------------------------------------------- Eliminar

    public void EliminarInicio()
    {
        if (Head == null) return;                 // lista vacia: nada que eliminar
        if (Head == Tail) Head = Tail = null;     // un solo nodo
        else { Head = Head.Next; Head!.Prev = null; }
        Count--;
    }

    public void EliminarFinal()
    {
        if (Tail == null) return;
        if (Head == Tail) Head = Tail = null;
        else { Tail = Tail.Prev; Tail!.Next = null; }
        Count--;
    }

    /// <summary>
    /// Elimina la primera foto con ese nombre (sin distinguir mayusculas).
    /// Devuelve false si no existe. Reconecta los vecinos para que ninguna
    /// referencia apunte a un nodo que ya no pertenece a la lista.
    /// </summary>
    public bool Eliminar(string nombre)
    {
        Nodo? nodo = BuscarNodo(nombre);
        if (nodo == null) return false;

        if (nodo == Head) EliminarInicio();
        else if (nodo == Tail) EliminarFinal();
        else
        {
            nodo.Prev!.Next = nodo.Next;
            nodo.Next!.Prev = nodo.Prev;
            Count--;
        }
        return true;
    }

    // ------------------------------------------------- Buscar y modificar

    private Nodo? BuscarNodo(string nombre)
    {
        Nodo? actual = Head;
        while (actual != null)
        {
            if (string.Equals(actual.Dato.Nombre, nombre, StringComparison.OrdinalIgnoreCase))
                return actual;
            actual = actual.Next;
        }
        return null;
    }

    /// <summary>Busca una foto por nombre; devuelve null si no esta.</summary>
    public Foto? Buscar(string nombre) => BuscarNodo(nombre)?.Dato;

    /// <summary>Busca el nodo por nombre (para navegar a Prev/Next desde el visor).</summary>
    public Nodo? ObtenerNodo(string nombre) => BuscarNodo(nombre);

    /// <summary>Cambia el nombre de archivo de una foto. False si no existe.</summary>
    public bool Modificar(string nombre, string nuevoNombre)
    {
        Nodo? nodo = BuscarNodo(nombre);
        if (nodo == null) return false;
        nodo.Dato.Nombre = nuevoNombre;
        return true;
    }

    // -------------------------------------------------------------- Recorridos

    /// <summary>Recorre desde Head avanzando por Next hasta llegar a null.</summary>
    public IEnumerable<Foto> MostrarAdelante()
    {
        Nodo? actual = Head;
        while (actual != null)
        {
            yield return actual.Dato;
            actual = actual.Next;
        }
    }

    /// <summary>Recorre desde Tail retrocediendo por Prev hasta llegar a null.</summary>
    public IEnumerable<Foto> MostrarAtras()
    {
        Nodo? actual = Tail;
        while (actual != null)
        {
            yield return actual.Dato;
            actual = actual.Prev;
        }
    }
}
