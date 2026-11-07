# Visor de fotografías

Laboratorio de la Semana 16 (Listas doblemente enlazadas) — Programación II.

Aplicación web ASP.NET Core MVC (C#) que lee los nombres de las fotografías desde `Data/fotos.txt`, los carga en una **lista dinámica doblemente enlazada** (`Models/ListaDoble.cs`) y permite:

- **Insertar** al inicio o al final.
- **Buscar** por nombre.
- **Modificar** el nombre de archivo.
- **Eliminar** por nombre, el primero o el último.
- **Recorrer** desde el inicio (`Next`) y desde el final (`Prev`).
- **Visor** con botones Anterior / Siguiente que usan `Prev` y `Next` del nodo.

Los cambios se guardan de vuelta en `Data/fotos.txt`. Las imágenes viven en `wwwroot/fotos/`.

## Ejecutar

```bash
dotnet run
```

Abrir <http://localhost:5080>.
