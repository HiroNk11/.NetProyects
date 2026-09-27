namespace MesaAyuda.Web.Models;
public static class StatusText
{
    public static string Label(TicketStatus status) => status == TicketStatus.EnCurso ? "En curso" : status.ToString();
}
