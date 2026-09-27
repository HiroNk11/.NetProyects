using System.ComponentModel.DataAnnotations;
namespace MesaAyuda.Web.Models;
public class LoginInput
{
    [Required(ErrorMessage = "Ingresá tu correo."), EmailAddress, Display(Name = "Correo")]
    public string Email { get; set; } = "";
    [Required(ErrorMessage = "Ingresá tu contraseña."), DataType(DataType.Password), Display(Name = "Contraseña")]
    public string Password { get; set; } = "";
    public string? ReturnUrl { get; set; }
}
public class CreateTicketInput
{
    [Required(ErrorMessage = "Escribí un título."), StringLength(120, MinimumLength = 5), Display(Name = "Título")]
    public string Title { get; set; } = "";
    [Required(ErrorMessage = "Describí el problema."), StringLength(4000, MinimumLength = 10), Display(Name = "Descripción")]
    public string Description { get; set; } = "";
    [EnumDataType(typeof(TicketPriority)), Display(Name = "Prioridad")]
    public TicketPriority Priority { get; set; } = TicketPriority.Media;
}
public class TicketFilter
{
    [StringLength(100)] public string? Query { get; set; }
    [EnumDataType(typeof(TicketStatus))] public TicketStatus? Status { get; set; }
    [EnumDataType(typeof(TicketPriority))] public TicketPriority? Priority { get; set; }
    [Range(1, 100000)] public int Page { get; set; } = 1;
}
public record TicketListView(List<Ticket> Tickets, TicketFilter Filter, int Total, int Pages,
    int Open, int InProgress, int Resolved, int Urgent);
public record TicketDetailView(Ticket Ticket, List<AppUser> Technicians, bool CanManage, bool CanAssign);
public class ChangeStatusInput
{
    [EnumDataType(typeof(TicketStatus))] public TicketStatus Status { get; set; }
    [Range(1, int.MaxValue)] public int Version { get; set; }
}
public class AssignInput
{
    [Required] public string TechnicianId { get; set; } = "";
    [Range(1, int.MaxValue)] public int Version { get; set; }
}
public class CommentInput
{
    [Required, StringLength(2000, MinimumLength = 1)] public string Body { get; set; } = "";
    [Range(1, int.MaxValue)] public int Version { get; set; }
}
