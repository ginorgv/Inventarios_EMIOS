namespace Inventario.Web.Services;

/// <summary>
/// Utilidades para manejar excepciones en la UI de forma legible.
/// </summary>
public static class ErrorUtil
{
    /// <summary>
    /// Devuelve el mensaje de la excepción más interna de la cadena
    /// (la causa real: p. ej. el error de MariaDB/MySQL que EF Core envuelve
    /// en un DbUpdateException con un mensaje genérico).
    /// </summary>
    public static string MensajeRaiz(Exception ex)
    {
        var raiz = ex;
        while (raiz.InnerException is not null)
            raiz = raiz.InnerException;
        return raiz.Message;
    }
}
